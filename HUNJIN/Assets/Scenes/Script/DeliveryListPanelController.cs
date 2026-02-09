using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryListPanelController : MonoBehaviour
{
    [Header("Refs")]
    public AppFlow flow;
    public DeliveryService service;
    public NaverMapLink naverMapLink;

    [Header("Confirm UI")]
    public ChoicePanelController choicePanel;

    [Header("List")]
    public Transform contentRoot;
    public GameObject deliveryItemPrefab;
    public Button refreshButton;

    // "" = ALL
    private string statusFilter = "";

    // 탭 토글 동기화 중 콜백 루프 방지
    private bool _tabSyncing;

    void Awake()
    {
        if (refreshButton) refreshButton.onClick.AddListener(Refresh);
    }

    void OnEnable() => Refresh();

    /// <summary>
    /// FilterTabAction에서 호출됨.
    /// code: ALL / CREATED / ACCEPTED / PICKED_UP / DELIVERED / CANCELED
    /// isOn: 토글 켜짐/꺼짐
    /// sourceToggle: 호출한 토글(본인)
    /// </summary>
    public void OnFilterTabChanged(string code, bool isOn, Toggle sourceToggle)
    {
        if (_tabSyncing) return;

        code = (code ?? "").Trim();
        if (string.IsNullOrEmpty(code)) code = "ALL";

        // OFF 이벤트는: "현재 선택된 탭을 끄는 행위"일 때만 의미가 있음
        // (토글그룹 없이 구현할 수 있게 설계)
        if (!isOn)
        {
            // 현재 필터가 이 탭이었다면 => ALL로 복귀시키기
            bool wasThisSelected =
                (code == "ALL" && string.IsNullOrEmpty(statusFilter)) ||
                (code != "ALL" && statusFilter == code);

            if (wasThisSelected)
            {
                SetFilterInternal("ALL", sourceToggle);
                Refresh();
            }
            return;
        }

        // ON 이벤트
        SetFilterInternal(code, sourceToggle);
        Refresh();
    }

    private void SetFilterInternal(string code, Toggle sourceToggle)
    {
        // code==ALL이면 statusFilter=""
        statusFilter = (code == "ALL") ? "" : code;

        // 같은 탭 그룹에 있는 다른 토글들 OFF 처리
        // (FilterTabAction들이 같은 패널 아래에 붙어있다는 전제)
        _tabSyncing = true;
        try
        {
            var actions = GetComponentsInChildren<FilterTabAction>(true);
            foreach (var a in actions)
            {
                if (a == null) continue;
                var t = a.GetComponent<Toggle>();
                if (!t) continue;

                bool shouldBeOn =
                    (code == "ALL" && (a.statusCode == "ALL" || a.statusCode == "전체")) ||
                    (code != "ALL" && NormalizeStatusLikeFilterTabAction(a.statusCode) == code);

                // sourceToggle은 이미 ON이므로, 나머지는 OFF
                if (t == sourceToggle) continue;

                // Toggle 이벤트 루프 방지: SetIsOnWithoutNotify 사용
                t.SetIsOnWithoutNotify(shouldBeOn);
            }

            // sourceToggle도 혹시 다른 로직으로 꼬였으면 맞춰줌
            if (sourceToggle) sourceToggle.SetIsOnWithoutNotify(true);
        }
        finally
        {
            _tabSyncing = false;
        }
    }

    // FilterTabAction.NormalizeStatus와 같은 규칙으로 맞춰줌(컨트롤러 단에서도 안전하게)
    private static string NormalizeStatusLikeFilterTabAction(string s)
    {
        s = (s ?? "").Trim();
        if (string.IsNullOrEmpty(s)) return "ALL";

        switch (s)
        {
            case "전체":
            case "ALL": return "ALL";
            case "등록됨": return "CREATED";
            case "수락완료": return "ACCEPTED";
            case "픽업완료": return "PICKED_UP";
            case "배달완료": return "DELIVERED";
            case "취소":
            case "취소됨": return "CANCELED";
            default: return s; // 이미 코드면 그대로
        }
    }

    public void Refresh()
    {
        if (!service) return;
        StartCoroutine(CoFetch());
    }

    IEnumerator CoFetch()
    {
        ApiResponse resp = null;
        yield return service.FetchDeliveriesAdmin(statusFilter, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[DeliveryList] FAIL: {resp?.msg}");
            yield break;
        }

        var rows = service.ParseDeliveryRows(resp.value);
        Render(rows);
    }

    void Render(List<DeliveryDto> rows)
    {
        ClearChildren(contentRoot);

        bool loggedIn = service && service.IsLoggedIn();

        foreach (var d in rows)
        {
            var go = Instantiate(deliveryItemPrefab, contentRoot);

            // AssignButton(=수락 버튼) 처리
            var assignBtn = FindButton(go, "AssignButton");
            if (assignBtn)
            {
                bool canAccept = loggedIn && d.status == "CREATED";

                // "등록(CREATED) + 로그인"일 때만 활성
                assignBtn.gameObject.SetActive(canAccept);

                if (canAccept)
                {
                    var label = assignBtn.GetComponentInChildren<TMP_Text>();
                    if (label) label.text = "수락";

                    assignBtn.onClick.RemoveAllListeners();
                    assignBtn.onClick.AddListener(() => RequestAcceptConfirm(d.deliveryId));
                }
            }
        }
    }

    void RequestAcceptConfirm(string deliveryId)
    {
        if (choicePanel)
        {
            choicePanel.Show(
                "배차 수락하시겠습니까?",
                onYes: () => StartCoroutine(CoClaimAndGoMy(deliveryId)),
                onNo: null
            );
        }
        else
        {
            StartCoroutine(CoClaimAndGoMy(deliveryId));
        }
    }

    IEnumerator CoClaimAndGoMy(string deliveryId)
    {
        ApiResponse resp = null;
        yield return service.Claim(deliveryId, service.currentDriverId, r => resp = r);

        if (resp == null || resp.result != "OK")
        {
            Debug.LogWarning($"[Claim] FAIL: {resp?.msg}");
            yield break;
        }

        if (flow) flow.ShowDriverHome();
    }

    static void ClearChildren(Transform t)
    {
        if (!t) return;
        for (int i = t.childCount - 1; i >= 0; i--)
            Object.Destroy(t.GetChild(i).gameObject);
    }

    static Button FindButton(GameObject root, string childName)
    {
        var tf = root.transform.Find(childName);
        return tf ? tf.GetComponent<Button>() : null;
    }
}
