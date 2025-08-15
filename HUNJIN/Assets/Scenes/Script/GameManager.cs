// == 교체본 ==
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class CompanyList
{
    public CompanyList(string _Date, string _SubjectName, string _Receiving, string _Release, string _CompanyName, string _Time, string _Gugo)
    { Date = _Date; SubjectName = _SubjectName; Receiving = _Receiving; Release = _Release; CompanyName = _CompanyName; ReceivingTime = _Time; Gugo = _Gugo; }

    public string Date, SubjectName = "None", Receiving = "0", Release = "0", CompanyName = "None", ReceivingTime = "", Gugo = "";
}

[System.Serializable]
public class NameCompanyList
{
    public NameCompanyList(string _Name, string _Company)
    { Name = _Name; Company = _Company; }
    public string Name = "테스트", Company = "테스트";
}

public class GameManager : MonoBehaviour
{
    public static List<T> removeDuplicates<T>(List<T> list) => new HashSet<T>(list).ToList();

    static GameManager inst;
    public static GameManager Instance => inst;

    [Header("Config / Repository")]
    public InventoryConfig inventoryConfig;
    private InventoryRepository repository;

    [Header("Scene/UI")]
    public ScrollViewEnom scrollViewEnom;
    public ScrollViewController scrollViewController;
    public GameObject main;
    public GameObject[] Scenes;
    public Animator animator;

    [Header("State")]
    public int curScene;
    public bool isSed;                  // 요약/페이지네이션 여부
    public bool isSubject = false;      // 상세 여부
    public string curType = "Main";     // "Main","Enrollment","Press","Welding","Assembly","All","Lost"

    [Header("Data In-Memory")]
    public List<CompanyList> MyCompanyDatabase = new List<CompanyList>();
    public List<CompanyList> MySearchData = new List<CompanyList>();
    public List<CompanyList> DoSearchData = new List<CompanyList>();
    public List<NameCompanyList> NameCompanyData = new List<NameCompanyList>();
    public List<string> CompanyData = new List<string>();
    public List<string> NameData = new List<string>();
    public List<string> EnomData = new List<string>();
    public List<string> curData = new List<string>();

    [Header("UI Refs")]
    public TMP_InputField[] SubjectNameSearch;
    public TextMeshProUGUI[] AllSubjectCountText;
    public TextMeshProUGUI[] AllCount;
    public GameObject[] CheckBoxs;
    public Dropdown[] dropdown;
    public Dropdown[] Searchdropdown;

    // ENROLLMENT 
    public Dropdown CompanyDrop, NoneDrop;
    public TMP_InputField SubjectInput;
    public InputField DateInput, TimeInput, ReleaseInput, ReceivingInput, GugoInput;
    public GameObject Loading;
    public float loadingMinDuration = 0.35f;   // 최소 표시 시간(초)
    public float loadingMaxTimeout = 15f;     // (옵션) 최대 표시 시간(안전장치)

    private float _loadingShownAt = -1f;
    private int _loadingCount = 0;             // 겹치는 로딩 카운트
    private Coroutine _loadingHideCo = null;
    private int _loadingTicket = 0;            // 새 로딩 시작마다 증가 → 대기 코루틴 무효화용
    private float _loadingHardDeadline = -1f;  // (옵션) 강제 종료 시각

    // SearchScene
    public int Curpage = 0;
    private int Maxpage = 1;
    public int PageObject = 10;
    public GameObject LeftArrow;
    public GameObject RightArrow;

    // Flags
    public bool isCompanyName;
    private bool subfirst = true;
    private bool isArrow = false;
    public bool isloading;

    // Google Apps Script (등록용)
    string CodeURL = "https://script.google.com/macros/s/AKfycbw3D8WZBrlTQU6q003Vi7u7Mn91NM-4nUotIIuY1qI1iUD_gN1xdcSh3UjyCaSZnHO-2A/exec";
    public int CompanySearch;
    public int SeachIndex = 1;

    void Awake()
    {
        inst = this;
        repository = new InventoryRepository(inventoryConfig);

        // 첫 화면 활성화
        if (Scenes != null)
        {
            for (int i = 0; i < Scenes.Length; i++)
                Scenes[i].SetActive(i == 0);
        }

        // 드롭다운 안전 리스너
        if (dropdown != null)
        {
            int n = Mathf.Min(dropdown.Length, 4);
            for (int i = 0; i < n; i++)
                if (dropdown[i] != null) dropdown[i].onValueChanged.AddListener(OnDropdownEvent);
        }

        main.SetActive(true);
    }

    public void Start()
    {
        isCompanyName = true;
        subfirst = true;
        isSubject = false;

        DateInput.text = DateTime.Now.ToString("yy-MM-dd");
        TimeInput.text = DateTime.Now.ToString("HH:mm");

        if (AllSubjectCountText != null && AllSubjectCountText.Length > 0)
            AllSubjectCountText[curScene].text = "[0/0]";

        for (int i = 0; i < CheckBoxs.Length; i++)
            CheckBoxs[i].SetActive(false);

        SubjectInput.onValueChanged.AddListener(OnInputValueChanged);
        main.SetActive(false);

    }

    // ---------- UI helpers ----------
    public void Resetdropdown()
    {
        if (SubjectNameSearch != null && SubjectNameSearch.Length > curScene && SubjectNameSearch[curScene] != null)
            SubjectNameSearch[curScene].text = "";
    }

    public void StartLoading()
    {
        _loadingCount++;
        if (_loadingCount == 1)
        {
            _loadingTicket++;                          // 새 로딩 세션
            _loadingShownAt = Time.realtimeSinceStartup;
            _loadingHardDeadline = _loadingShownAt + loadingMaxTimeout;

            if (_loadingHideCo != null)                // 이전 숨김 대기 중이면 취소
            {
                StopCoroutine(_loadingHideCo);
                _loadingHideCo = null;
            }
            if (Loading != null) Loading.SetActive(true);
        }
    }

    public void EndLoading()
    {
        if (_loadingCount > 0) _loadingCount--;
        if (_loadingCount > 0) return;                 // 아직 진행 중인 로딩 있음

        float elapsed = Time.realtimeSinceStartup - _loadingShownAt;
        float remain = loadingMinDuration - elapsed;

        if (remain <= 0f)                               // 최소 표시시간 충족 → 즉시 숨김
        {
            if (Loading != null) Loading.SetActive(false);
            _loadingHideCo = null;
        }
        else                                            // 최소 시간 채우고 숨김
        {
            int expected = _loadingTicket;              // 이 세션의 티켓
            if (_loadingHideCo != null) StopCoroutine(_loadingHideCo);
            _loadingHideCo = StartCoroutine(HideLoadingAfter(remain, expected));
        }
    }

    private System.Collections.IEnumerator HideLoadingAfter(float delay, int expectedTicket)
    {
        yield return new WaitForSeconds(delay);

        // 대기 중 새 로딩이 시작되었거나(_loadingTicket 변경) 다른 로딩이 돌아가면 숨기지 않음
        if (_loadingCount == 0 && expectedTicket == _loadingTicket)
        {
            if (Loading != null) Loading.SetActive(false);
        }
        _loadingHideCo = null;
    }

    // ---------- Enrollment ----------
    bool SetIDPass()
    {
        // 간단히 빈값 허용. 필요 시 validation 강화 가능
        return true;
    }

    public void Register()
    {
        if (!SetIDPass()) { print("잘못된 입력입니다."); return; }

        CheckBoxs[0].SetActive(true);

        var form = new WWWForm();
        form.AddField("order", "register");
        form.AddField("date", DateInput.text.Trim());
        form.AddField("subject", SubjectInput.text.Trim());
        form.AddField("release", ReleaseInput.text.Trim());
        form.AddField("receiving", ReceivingInput.text.Trim());
        form.AddField("companyName", CompanyDrop.captionText.text.Trim());
        form.AddField("rtime", TimeInput.text.Trim());
        form.AddField("none", NoneDrop.captionText.text.Trim());
        form.AddField("gugo", GugoInput.text.Trim());

        ResetData();
        curData.Clear();
        scrollViewEnom.UpdateScrollView();
        CompanyDrop.value = 0;
        NoneDrop.value = 0;

        StartCoroutine(Post(form));
    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(CodeURL, form))
        {
            yield return www.SendWebRequest();
            if (www.result == UnityWebRequest.Result.Success) print(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }
        // 등록 후 최신 데이터 갱신 (All 기준)
        StartCoroutine(Lookup("All"));
    }

    // ---------- Search + Paging ----------
    public void ResetData()
    {
        isArrow = true;
        Curpage = 0;
        DateInput.text = DateTime.Now.ToString("yy/M/d");
        TimeInput.text = DateTime.Now.ToString("HH:mm");
        if (AllSubjectCountText != null && AllSubjectCountText.Length > curScene)
            AllSubjectCountText[curScene].text = "[0/0]";
        SubjectInput.text = "";
        ReleaseInput.text = "";
        ReceivingInput.text = "";
        GugoInput.text = "";
        NoneDrop.captionText.text = "부서";
        if (AllCount != null && AllCount.Length > curScene && AllCount[curScene] != null)
            AllCount[curScene].text = "";
        scrollViewController.ResetEnId();
        isCompanyName = true;
    }

    public void OnDropdownEvent(int index)
    {
        if (dropdown != null && dropdown.Length > curScene && dropdown[curScene] != null)
            dropdown[curScene].value = index;
        CompanySearch = index;
    }

    public void OnInputValueChanged(string text)
    {
        curData.Clear();
        var added = new HashSet<string>();
        int currentIndex = CompanyDrop.value;
        string company = CompanyDrop.options[currentIndex].text;

        foreach (var nc in NameCompanyData)
        {
            if (company == nc.Company)
            {
                string trimmed = nc.Name.Trim().ToLower();
                if (trimmed.Contains(SubjectInput.text.Trim().ToLower()) && added.Add(trimmed))
                    curData.Add(nc.Name);
            }
        }
        scrollViewEnom.UpdateScrollView();
    }
    // 동적 필터를 적용하고 UI를 갱신
    public void ApplyFiltersAndRefreshUI(System.Collections.Generic.List<FilterCondition> filters)
    {
        // repository는 기존에 GameManager에서 생성해둔 필드라고 가정
        if (repository == null)
        {
            Debug.LogError("[GameManager] repository가 없습니다.");
            return;
        }

        // 1) 레코드 필터
        var filtered = repository.ApplyFilters(filters);

        // 2) 오래 쓰는 구조로 매핑
        MyCompanyDatabase = new System.Collections.Generic.List<CompanyList>();
        for (int i = 0; i < filtered.Count; i++)
        {
            var r = filtered[i];
            MyCompanyDatabase.Add(new CompanyList(
                r.Date, r.SubjectName, r.Receiving, r.Release, r.CompanyName, r.ReceivingTime, r.Gugo
            ));
        }

        // 3) 드롭다운/인덱스 재구축
        BuildIndicesAndDropdowns();

        // 4) 화면 갱신 (기존 흐름 재사용)
        if (ScrollViewController.Instance != null)
            ScrollViewController.Instance.DoSearch();

        // 페이지/상태 텍스트 같은 거 갱신 필요하면 여기서 처리
        UpdateAllCountBanner(MyCompanyDatabase, null);
    }

    // ---------- 화면 탭 전환 ----------
    public void TabClick(string tabName)
    {
        CompanyDrop.captionText.text = "기본";
        MySearchData.Clear();
        if (SubjectNameSearch != null && SubjectNameSearch.Length > curScene) SubjectNameSearch[curScene].text = "";
        ResetData();
        OnDropdownEvent(0);
        curData.Clear();
        scrollViewEnom.UpdateScrollView();

        if (!subfirst && curType != "Main" && curType != "Enrollment")
            ScrollViewController.Instance.UIObjectReset();

        int tabNum = 0;
        switch (tabName)
        {
            case "Main": tabNum = 0; break;
            case "All": tabNum = 5; break;
            case "Press": tabNum = 1; break;
            case "Enrollment": tabNum = 2; break;
            case "Welding": tabNum = 3; break;
            case "Assembly": tabNum = 4; break;
            case "Lost": tabNum = 6; break;
            default: tabNum = 0; break;
        }

        for (int i = 0; i < Scenes.Length; i++)
            Scenes[i].SetActive(i == tabNum);

        if (subfirst) subfirst = false;

        if (tabName != "Enrollment" && tabName != "Main")
        {
            StartCoroutine(Lookup(tabName));
            main.SetActive(true);
            if (tabName == "Lost") StartCoroutine(Lookup("All"));
        }
        else
        {
            if (tabName == "Enrollment") StartCoroutine(Lookup("All"));
            main.SetActive(false);
        }
        switch (tabName)
        {
            case "Press": curScene = 0; break;
            case "Welding": curScene = 1; break;
            case "Assembly": curScene = 2; break;
            case "All": curScene = 3; break;
            case "Lost": curScene = 3; break;
            default: curScene = 0; break;
        }
        curType = tabName;

    }

    // ---------- 데이터 로딩 ----------
    IEnumerator Lookup(string deptKey)
    {
        StartLoading();
        yield return repository.Refresh(deptKey);
        EndLoading();

        // 메모리 데이터 → 기존 구조에 맞춰 매핑
        MyCompanyDatabase = repository.Records.Select(r =>
            new CompanyList(r.Date, r.SubjectName, r.Receiving, r.Release, r.CompanyName, r.ReceivingTime, r.Gugo)
        ).ToList();

        BuildIndicesAndDropdowns();

        // 배너 표시 제어
        UpdateAllCountBanner(MyCompanyDatabase, null);

        if (curType != "Enrollment" && curType != "Main" && curType != "Lost")
            ScrollViewController.Instance.DoSearch();


    }

    void BuildIndicesAndDropdowns()
    {
        // 초기화
        CompanyData.Clear(); NameData.Clear(); curData.Clear(); NameCompanyData.Clear();
        if (dropdown != null && dropdown.Length > curScene && dropdown[curScene] != null)
            dropdown[curScene].options.Clear();

        // 유니크 제품/회사
        var uniqueBySubject = MyCompanyDatabase
            .GroupBy(p => p.SubjectName).Select(g => g.First()).ToList();
        var uniqueCompany = MyCompanyDatabase.Select(p => p.CompanyName).Distinct().ToList();

        if (dropdown != null && dropdown.Length > curScene && dropdown[curScene] != null)
            dropdown[curScene].options.Add(new Dropdown.OptionData("전체"));

        foreach (var item in uniqueBySubject)
        {
            NameData.Add(item.SubjectName);
            NameCompanyData.Add(new NameCompanyList(item.SubjectName, item.CompanyName));
        }
        foreach (var comp in uniqueCompany)
        {
            CompanyData.Add(comp);
            if (dropdown != null && dropdown.Length > curScene && dropdown[curScene] != null)
                dropdown[curScene].options.Add(new Dropdown.OptionData(comp));
            CompanyDrop.options.Add(new Dropdown.OptionData(comp));
        }
        // 기본값 리셋
        if (CompanyDrop != null) CompanyDrop.value = 0;
        if (dropdown != null && dropdown.Length > curScene && dropdown[curScene] != null)
            dropdown[curScene].value = 0;
    }

    // ---------- 검색 (기존 흐름 유지) ----------
    public int AAASearch() // 거래처 검색
    {
        isArrow = true; isSed = true;
        var dist = MyCompanyDatabase.Select(p => new { p.SubjectName, p.CompanyName, p.Gugo }).Distinct().ToList();
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);

        int count = 0;
        string key = (dropdown[curScene].options[dropdown[curScene].value].text ?? "").Trim().ToLower();

        foreach (var obj in dist)
        {
            if ((obj.CompanyName ?? "").Trim().ToLower().Contains(key))
            {
                MySearchData.Add(new CompanyList("10/31", obj.SubjectName, "1", "1", obj.CompanyName, "10:10", obj.Gugo));
                count++;
            }
        }
        return SetupPaging(count);
    }

    public int ProductSearch() // 텍스트 검색
    {
        isArrow = true; isSed = true;
        var dist = MyCompanyDatabase.Select(p => new { p.SubjectName, p.CompanyName, p.Gugo }).Distinct().ToList();
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);

        int count = 0;
        string key = (SubjectNameSearch[curScene].text ?? "").Trim().ToLower();

        foreach (var obj in dist)
        {
            if ((obj.SubjectName ?? "").Trim().ToLower().Contains(key))
            {
                MySearchData.Add(new CompanyList("10/31", obj.SubjectName, "1", "1", obj.CompanyName, "10:10", obj.Gugo));
                count++;
            }
        }
        // count 계산 직후
        UpdateAllCountBanner(MySearchData, null);

        return SetupPaging(count);
    }
    public void searchButtonDown(int id)
    {
        // id 범위 체크
        if (curData == null || id < 0 || id >= curData.Count) return;

        // 선택한 이름을 등록 화면의 입력창에 반영
        if (SubjectInput != null)
        {
            SubjectInput.text = curData[id];
            // 커서를 맨 끝으로
            try { SubjectInput.caretPosition = SubjectInput.text.Length; } catch { }
        }

        // 선택 후 자동완성 리스트 닫기
        curData.Clear();
        if (scrollViewEnom != null) scrollViewEnom.UpdateScrollView();
    }

    public int ProductSearch(string text) // over
    {
        MySearchData.Clear();
        int count = 0;
        string key = (SubjectNameSearch[curScene].text ?? "").Trim().ToLower();

        foreach (var r in MyCompanyDatabase)
        {
            if ((r.SubjectName ?? "").Trim().ToLower().Contains(key))
            {
                MySearchData.Add(r);
                count++;
            }
        }
        AllSubjectCountText[curScene].text = "";
        isArrow = false;
        return count;
    }

    int SetupPaging(int total)
    {
        int save = total;
        Maxpage = total / PageObject;                 // 0-based pages
        int rem = save % PageObject;
        if (rem == 0 && save > 0) rem = PageObject;     // 딱 떨어질 때 보정

        int count = (Maxpage == Curpage) ? rem : PageObject;

        int ui = Curpage + 1;
        int uiMax = Maxpage + 1;
        if (AllSubjectCountText != null && AllSubjectCountText.Length > curScene && AllSubjectCountText[curScene] != null)
            AllSubjectCountText[curScene].text = $"[{ui}/{uiMax}]";

        RefreshPagingUI();                               // 화살표 표시 갱신
        return count;
    }



    public int SEadSearch() // 요약
    {
        isArrow = true; isSed = true;
        var dist = MyCompanyDatabase.Select(p => new { p.SubjectName, p.CompanyName, p.Gugo }).Distinct().ToList();
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);

        int count = 0;
        foreach (var obj in dist)
        {
            MySearchData.Add(new CompanyList("10/31", obj.SubjectName, "1", "1", obj.CompanyName, "10:10", obj.Gugo));
            count++;
        }
        return SetupPaging(count);
    }

    public int DOTextSearch(string text)
    {
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        string key = (text ?? "").Trim();

        foreach (var r in MyCompanyDatabase)
        {
            if ((r.SubjectName ?? "").Trim().Equals(key, StringComparison.OrdinalIgnoreCase))
            {
                MySearchData.Add(r);
                count++;
            }
        }

        SubjectNameSearch[curScene].text = key;
        AllSubjectCountText[curScene].text = $"[{count}/{count}]";

        int dex = 0;
        for (int i = 0; i < count; i++)
        {
            int rel = int.TryParse(MySearchData[i].Release.Replace(",", ""), out var r1) ? r1 : 0;
            int rec = int.TryParse(MySearchData[i].Receiving.Replace(",", ""), out var r2) ? r2 : 0;
            dex += rel; dex -= rec;
        }
        UpdateAllCountBanner(MySearchData, SubjectNameSearch[curScene].text);
        return count;
    }

    public int LostTextSearch()
    {
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = MyCompanyDatabase.Count;
        MySearchData.AddRange(MyCompanyDatabase);
        AllSubjectCountText[curScene].text = $"[{count}/{count}]";
        UpdateAllCountBanner(MySearchData, null);
        return count;
    }

    public int GetSubjectRemaining(int id)
    {
        int dex = 0;
        if (!isCompanyName && MySearchData.Count > 0)
        {
            for (int i = 0; i <= id && i < MySearchData.Count; i++)
            {
                if (MySearchData[id].SubjectName.Trim() == MySearchData[i].SubjectName.Trim())
                {
                    int rel = int.TryParse(MySearchData[i].Release.Replace(",", ""), out var r1) ? r1 : 0;
                    int rec = int.TryParse(MySearchData[i].Receiving.Replace(",", ""), out var r2) ? r2 : 0;
                    dex += rel; dex -= rec;
                }
            }
            if (AllCount != null && AllCount.Length > curScene && AllCount[curScene] != null)
            {
                if (dex < 0) AllCount[curScene].text = $"{MySearchData[0].SubjectName.Trim()} - 재고 부족 : {dex}";
                else if (dex > 0) AllCount[curScene].text = $"{MySearchData[0].SubjectName.Trim()} - 현재 재고 : {dex}";
                else AllCount[curScene].text = " - 재고 부족 : 0";
            }
        }
        return dex;
    }
void UpdateAllCountBanner(List<CompanyList> source, string subjectFilter)
{
    if (AllCount == null || AllCount.Length == 0) return;

    int idx = curScene;
        // 요약검색 + 상세 아님 → 배너 숨김
        if (isSed && !isSubject)
        {
            idx = curScene;
            if (AllCount != null && AllCount.Length > idx && AllCount[idx] != null)
                AllCount[idx].gameObject.SetActive(false);
            // 페이지 카운터 텍스트는 유지하고 싶으면 여기서 따로 다루세요.
            return;
        }
        if (idx < 0 || idx >= AllCount.Length) idx = 0;
    var label = AllCount[idx];
    if (label == null) return;

    int dex = 0, inQty = 0, outQty = 0, rows = 0;
    for (int i = 0; i < source.Count; i++)
    {
        var r = source[i];
        if (!string.IsNullOrEmpty(subjectFilter))
        {
            if (!string.Equals((r.SubjectName ?? "").Trim(),
                               subjectFilter.Trim(),
                               System.StringComparison.OrdinalIgnoreCase))
                continue;
        }

        int rel = 0, rec = 0;
        int.TryParse((r.Release ?? "0").Replace(",", "").Trim(), out rel);
        int.TryParse((r.Receiving ?? "0").Replace(",", "").Trim(), out rec);

        dex += rel - rec;
        inQty += rel;
        outQty += rec;
        rows++;
    }

    string head = string.IsNullOrEmpty(subjectFilter) ? "전체" : subjectFilter.Trim();
    string msg = dex < 0 ? (head + " - 재고 부족 : " + dex) : (head + " - 현재 재고 : " + dex);
    label.text = msg + "  (입고 " + inQty + ", 출고 " + outQty + ", 항목 " + rows + ")";
    label.gameObject.SetActive(true);

    // 페이지 카운터 텍스트도 보이게
    if (AllSubjectCountText != null && AllSubjectCountText.Length > idx && AllSubjectCountText[idx] != null)
        AllSubjectCountText[idx].gameObject.SetActive(true);
}

public string[] GetSearch(int id)
    {
        string[] j = { "", "", "", "", "", "", "", "" };
        if (id < 0 || id >= MySearchData.Count) return j;
        j[0] = MySearchData[id].Date; j[1] = MySearchData[id].SubjectName; j[2] = MySearchData[id].Release;
        j[3] = MySearchData[id].Receiving; j[4] = MySearchData[id].CompanyName; j[6] = MySearchData[id].ReceivingTime; j[7] = MySearchData[id].Gugo;
        return j;
    }

    public string[] DoGetSearch(int id)
    {
        string[] j = { "", "", "", "", "", "", "", "" };
        if (id < 0 || id >= MyCompanyDatabase.Count) return j;
        j[0] = MyCompanyDatabase[id].Date; j[1] = MyCompanyDatabase[id].SubjectName; j[2] = MyCompanyDatabase[id].Release;
        j[3] = MyCompanyDatabase[id].Receiving; j[4] = MyCompanyDatabase[id].CompanyName; j[6] = MyCompanyDatabase[id].ReceivingTime; j[7] = MyCompanyDatabase[id].Gugo;
        return j;
    }
    // GameManager 클래스 안에 추가
    void RefreshPagingUI()
    {
        // Maxpage: 0이면 페이지 1개(0기반)
        bool multiplePages = Maxpage > 0;
        bool showArrows = multiplePages && curType != "Lost";

        isArrow = showArrows; // 기존 Update()에서 이 값으로 표시 제어도 하니 유지
        if (LeftArrow != null) LeftArrow.SetActive(showArrows);
        if (RightArrow != null) RightArrow.SetActive(showArrows);
    }

    public void NextPage()
    {
        if (Maxpage <= 0) return;      // 페이지 1개면 이동 불가
        if (Curpage < Maxpage) Curpage++;
        RefreshPagingUI();
        ScrollViewController.Instance?.DoSearch();
    }

    public void PrevPage()
    {
        if (Maxpage <= 0) return;      // 페이지 1개면 이동 불가
        if (Curpage > 0) Curpage--;
        RefreshPagingUI();
        ScrollViewController.Instance?.DoSearch();
    }

    // Android 종료키 처리 및 화살표 표시 유지
    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home) || Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Menu))
                Exit();
        }
        if (curType == "Lost")
        {
            LeftArrow.SetActive(false); RightArrow.SetActive(false);
        }
        else
        {
            LeftArrow.SetActive(isArrow); RightArrow.SetActive(isArrow);
        }
        if (Loading != null && Loading.activeSelf)
        {
            if (loadingMaxTimeout > 0f && Time.realtimeSinceStartup > _loadingHardDeadline)
            {
                // 너무 오래 켜져 있으면 강제 종료 (디버그/안전장치)
                _loadingCount = 0;
                Loading.SetActive(false);
                _loadingHideCo = null;
            }
        }
    }

    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
