using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryListPanelController : MonoBehaviour, IFilterTabController
{
    [Header("Refs")]
    public AppFlow flow;
    public DeliveryService service;

    [Header("Loading")]
    public GameObject loadingRoot;
    [Tooltip("로딩이 최소로 떠있어야 하는 시간(초)")]
    public float loadingMinDuration = 0.25f;

    [Header("Filter Toggles")]
    public Toggle allButton;        // ALL
    public Toggle createdButton;    // CREATED
    public Toggle acceptedButton;   // ACCEPTED
    public Toggle pickedUpButton;   // PICKED_UP
    public Toggle deliveredButton;  // DELIVERED
    public Toggle canceledButton;   // CANCELED

    [Header("Search (Optional)")]
    public TMP_InputField searchInput;

    [Header("List")]
    public Transform contentRoot;
    public GameObject deliveryItemPrefab;
    public Button refreshButton;
    public Button openCreateButton;

    [Header("Confirm Popup (Optional)")]
    public ChoicePanelController choice;

    // ===== runtime =====
    private readonly List<DeliveryDto> _cache = new List<DeliveryDto>();
    private readonly HashSet<string> _activeStatusCodes = new HashSet<string>();
    private string _search = "";
    private bool _hasFetchedOnce = false;

    // 토글 변경 중 컨트롤러 콜백 막기 (뷰 갱신은 되게 두는 용도)
    private bool _suppressToggleCallback = false;

    // loading runtime
    private bool _isLoading = false;
    private float _loadingShownAt = 0f;

    void Awake()
    {
        if (!flow) flow = FindObjectOfType<AppFlow>();
        if (!service) service = FindObjectOfType<DeliveryService>();

        // ✅ 시작 시 로딩은 무조건 꺼둠 (프리팹에서 켜져있어도 강제 OFF)
        if (loadingRoot) loadingRoot.SetActive(false);

        if (refreshButton) refreshButton.onClick.AddListener(OnClickRefresh);
        if (openCreateButton) openCreateButton.onClick.AddListener(() => flow.ShowCreate());

        if (searchInput)
        {
            searchInput.onValueChanged.AddListener(s =>
            {
                _search = (s ?? "").Trim();
                RenderFromCacheSafe();
            });
        }
    }

    void OnEnable()
    {
        // ✅ 토글 초기화는 먼저 (필터 꼬임 방지)
        InitDefaultFilter();

        // ✅ 데이터는 그 다음
        if (!_hasFetchedOnce) OnClickRefresh();
        else RenderFromCacheSafe();
    }

    void OnDisable()
    {
        // ✅ 씬 전환/패널 숨김 시 로딩이 남지 않게 강제 종료
        ForceHideLoading();
    }

    /// <summary>
    /// SetIsOnWithoutNotify()는 FilterTabView(onValueChanged 기반) 갱신을 깨트릴 수 있어서,
    /// isOn으로 바꾸되 컨트롤러 콜백만 suppress로 막는다.
    /// </summary>
    private void SetToggleQuiet(Toggle t, bool on)
    {
        if (!t) return;
        _suppressToggleCallback = true;
        t.isOn = on;     // Notify 발생 (FilterTabView 정상 갱신)
        _suppressToggleCallback = false;
    }

    private void InitDefaultFilter()
    {
        if (!allButton) return;

        // 기본: ALL만 ON, 나머지 OFF
        SetToggleQuiet(allButton, true);
        SetToggleQuiet(createdButton, false);
        SetToggleQuiet(acceptedButton, false);
        SetToggleQuiet(pickedUpButton, false);
        SetToggleQuiet(deliveredButton, false);
        SetToggleQuiet(canceledButton, false);

        SyncActiveStatusSetFromToggles();
        RenderFromCacheSafe();
    }

    // FilterTabAction -> 여기로 들어옴
    public void OnFilterTabChanged(string code, bool isOn, Toggle source)
    {
        if (_suppressToggleCallback) return;
        code = (code ?? "").Trim();

        // ✅ 핵심 수정: "전체" 누르면 다른 토글을 전부 꺼준다
        if (code == "ALL")
        {
            if (isOn)
            {
                _suppressToggleCallback = true;

                if (allButton) allButton.isOn = true;

                if (createdButton) createdButton.isOn = false;
                if (acceptedButton) acceptedButton.isOn = false;
                if (pickedUpButton) pickedUpButton.isOn = false;
                if (deliveredButton) deliveredButton.isOn = false;
                if (canceledButton) canceledButton.isOn = false;

                _suppressToggleCallback = false;
            }

            SyncActiveStatusSetFromToggles();
            RenderFromCacheSafe();
            return;
        }

        // 개별 토글이 켜지면: 그 토글만 ON, ALL 포함 나머지 OFF
        if (isOn)
        {
            SetOnlyOneOn(source);
        }

        SyncActiveStatusSetFromToggles();

        if (_activeStatusCodes.Count == 0 && !(allButton && allButton.isOn))
        {
            ClearSearchAndList();
            return;
        }

        RenderFromCacheSafe();
    }

    private void OnClickRefresh()
    {
        StartCoroutine(CoRefreshFromServer());
    }
    public void Refresh()
    {
        OnClickRefresh();
    }
    private IEnumerator CoRefreshFromServer()
    {
        if (!service)
        {
            Debug.LogError("[DeliveryList] service가 null 입니다.");
            yield break;
        }

        BeginLoading();

        ApiResponse resp = null;
        bool abort = false;

        // 1) 서버 호출
        yield return service.FetchDeliveriesAdmin("", r => resp = r);

        // 2) 결과 검증
        if (resp == null || resp.result != "OK")
        {
            Debug.LogError($"[DeliveryList] Fetch fail: {(resp != null ? resp.msg : "resp null")}");
            abort = true;
            goto END;
        }

        // 3) 파싱/캐시
        _cache.Clear();
        _cache.AddRange(service.ParseDeliveryRows(resp.value));
        _hasFetchedOnce = true;

        // 4) 렌더
        RenderFromCacheSafe();

    END:
        // ✅ finally 대신 “공통 꼬리”에서만 yield
        yield return EndLoadingMin();
        if (abort) yield break;
    }
    // ===== UI =====

    private void RenderFromCacheSafe()
    {
        try
        {
            RenderFromCache();
        }
        catch (Exception ex)
        {
            // Render 도중 NRE로 코루틴이 죽는 경우 방지
            Debug.LogException(ex);
            ForceHideLoading();
        }
    }

    private void RenderFromCache()
    {
        // 필수 참조 체크 (여기서 터지면 무한로딩/패널 고장으로 이어짐)
        if (!contentRoot)
        {
            Debug.LogError("[DeliveryList] contentRoot가 비어있습니다.");
            ClearListOnly();
            return;
        }
        if (!deliveryItemPrefab)
        {
            Debug.LogError("[DeliveryList] deliveryItemPrefab이 비어있습니다.");
            ClearListOnly();
            return;
        }

        IEnumerable<DeliveryDto> q = _cache;

        if (!(allButton && allButton.isOn))
        {
            if (_activeStatusCodes.Count == 0) { ClearListOnly(); return; }
            q = q.Where(d => _activeStatusCodes.Contains((d.status ?? "").Trim()));
        }

        if (!string.IsNullOrEmpty(_search))
        {
            q = q.Where(d =>
                (d.customerName ?? "").Contains(_search) ||
                (d.address ?? "").Contains(_search));
        }

        RebuildList(q.ToList());
    }

    private void RebuildList(List<DeliveryDto> rows)
    {
        ClearListOnly();

        foreach (var d in rows)
        {
            var go = Instantiate(deliveryItemPrefab, contentRoot);
            BindDeliveryItem(go.transform, d);
        }
    }

    private void BindDeliveryItem(Transform item, DeliveryDto d)
    {
        SetTMP(item, "CustomerNameText", d.customerName);
        SetTMP(item, "AddressText", d.address);
        SetTMP(item, "StatusText", StatusToKorean(d.status));
        SetTMP(item, "PointsText", d.points);

        ApplyStatusObjects(item, d.status);

        var assignBtn = Find<Button>(item, "AssignButton");
        if (assignBtn)
        {
            bool driverLoggedIn = !string.IsNullOrEmpty(service.currentDriverId);
            bool canClaim = driverLoggedIn && d.status == "CREATED";

            assignBtn.gameObject.SetActive(canClaim);
            assignBtn.onClick.RemoveAllListeners();

            if (canClaim)
            {
                assignBtn.onClick.AddListener(() =>
                {
                    choice.gameObject.SetActive(true);
                    choice.transform.SetAsLastSibling();
                    if (choice)
                    {
                        choice.Open(
                            "배차 수락",
                            "배차를 수락하시겠습니까?",
                            onYes: () => StartCoroutine(CoClaim(d.deliveryId)),
                            onNo: null
                        );
                    }
                    else
                    {
                        StartCoroutine(CoClaim(d.deliveryId));
                    }
                });
            }
        }
    }

    private IEnumerator CoClaim(string deliveryId)
    {
        if (string.IsNullOrEmpty(service.currentDriverId))
            yield break;

        BeginLoading();

        ApiResponse resp = null;
        bool abort = false;

        // 1) 서버 호출
        yield return service.Claim(deliveryId, service.currentDriverId, r => resp = r);

        // 2) 결과 검증
        if (resp == null || resp.result != "OK")
        {
            Debug.LogError($"[Claim] fail: {(resp != null ? resp.msg : "resp null")}");
            abort = true;
            goto END;
        }

        // 3) 로컬 캐시 반영
        var found = _cache.FirstOrDefault(x => x.deliveryId == deliveryId);
        if (found != null)
        {
            found.status = "ACCEPTED";
            found.assignedDriverId = service.currentDriverId;
            found.acceptedDriverName = service.currentDriverName;
        }

        // 4) 렌더
        RenderFromCacheSafe();

    END:
        yield return EndLoadingMin();
        if (abort) yield break;
    }
    private void ClearSearchAndList()
    {
        if (searchInput)
        {
            searchInput.SetTextWithoutNotify("");
            _search = "";
        }
        ClearListOnly();
    }

    private void ClearListOnly()
    {
        if (!contentRoot) return;
        for (int i = contentRoot.childCount - 1; i >= 0; i--)
            Destroy(contentRoot.GetChild(i).gameObject);
    }

    private void SyncActiveStatusSetFromToggles()
    {
        _activeStatusCodes.Clear();

        // ALL = 필터 없음
        if (allButton && allButton.isOn)
            return;

        if (createdButton && createdButton.isOn) _activeStatusCodes.Add("CREATED");
        else if (acceptedButton && acceptedButton.isOn) _activeStatusCodes.Add("ACCEPTED");
        else if (pickedUpButton && pickedUpButton.isOn) _activeStatusCodes.Add("PICKED_UP");
        else if (deliveredButton && deliveredButton.isOn) _activeStatusCodes.Add("DELIVERED");
        else if (canceledButton && canceledButton.isOn) _activeStatusCodes.Add("CANCELED");
    }

    private void SetOnlyOneOn(Toggle keepOn)
    {
        _suppressToggleCallback = true;

        if (allButton) allButton.isOn = false;

        if (createdButton) createdButton.isOn = (createdButton == keepOn);
        if (acceptedButton) acceptedButton.isOn = (acceptedButton == keepOn);
        if (pickedUpButton) pickedUpButton.isOn = (pickedUpButton == keepOn);
        if (deliveredButton) deliveredButton.isOn = (deliveredButton == keepOn);
        if (canceledButton) canceledButton.isOn = (canceledButton == keepOn);

        _suppressToggleCallback = false;
    }

    // ===== Loading =====

    private void BeginLoading()
    {
        _isLoading = true;
        _loadingShownAt = Time.unscaledTime;
        if (loadingRoot) loadingRoot.SetActive(true);
    }

    private IEnumerator EndLoadingMin()
    {
        // 이 코루틴은 finally에서 항상 호출되도록 구성했음
        float elapsed = Time.unscaledTime - _loadingShownAt;
        float remain = Mathf.Max(0f, loadingMinDuration - elapsed);
        if (remain > 0f)
            yield return new WaitForSecondsRealtime(remain);

        _isLoading = false;
        if (loadingRoot) loadingRoot.SetActive(false);
    }

    private void ForceHideLoading()
    {
        _isLoading = false;
        if (loadingRoot) loadingRoot.SetActive(false);
    }

    // ===== helpers =====

    private static void SetTMP(Transform root, string path, string text)
    {
        var t = root.Find(path);
        if (!t) return;
        var tmp = t.GetComponent<TMP_Text>();
        if (!tmp) tmp = t.GetComponentInChildren<TMP_Text>(true);
        if (!tmp) return;
        tmp.text = text ?? "";
    }

    private static T Find<T>(Transform root, string path) where T : Component
    {
        var t = root.Find(path);
        return t ? t.GetComponent<T>() : null;
    }

    private static string StatusToKorean(string code)
    {
        switch (code)
        {
            case "CREATED": return "등록됨";
            case "ASSIGNED": return "배차됨";
            case "ACCEPTED": return "수락됨";
            case "PICKED_UP": return "픽업완료";
            case "DELIVERED": return "배달완료";
            case "CANCELED": return "취소됨";
            default: return code ?? "";
        }
    }

    private static void ApplyStatusObjects(Transform itemRoot, string statusCode)
    {
        statusCode = (statusCode ?? "").Trim();

        for (int i = 0; i < itemRoot.childCount; i++)
        {
            var child = itemRoot.GetChild(i);
            if (!child) continue;

            string n = child.name ?? "";
            if (n.StartsWith("State_"))
            {
                string code = n.Substring("State_".Length).Trim();
                child.gameObject.SetActive(string.Equals(code, statusCode));
            }
            else if (n.StartsWith("Status_"))
            {
                string code = n.Substring("Status_".Length).Trim();
                child.gameObject.SetActive(string.Equals(code, statusCode));
            }
        }
    }
}
