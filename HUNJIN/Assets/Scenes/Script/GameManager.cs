// == ��ü�� ==
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
    public string Name = "�׽�Ʈ", Company = "�׽�Ʈ";
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
    public bool isSed;                  // ���/���������̼� ����
    public bool isSubject = false;      // �� ����
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
    public float loadingMinDuration = 0.35f;   // �ּ� ǥ�� �ð�(��)
    public float loadingMaxTimeout = 15f;     // (�ɼ�) �ִ� ǥ�� �ð�(������ġ)

    private float _loadingShownAt = -1f;
    private int _loadingCount = 0;             // ��ġ�� �ε� ī��Ʈ
    private Coroutine _loadingHideCo = null;
    private int _loadingTicket = 0;            // �� �ε� ���۸��� ���� �� ��� �ڷ�ƾ ��ȿȭ��
    private float _loadingHardDeadline = -1f;  // (�ɼ�) ���� ���� �ð�

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

    // Google Apps Script (��Ͽ�)
    string CodeURL = "https://script.google.com/macros/s/AKfycbw3D8WZBrlTQU6q003Vi7u7Mn91NM-4nUotIIuY1qI1iUD_gN1xdcSh3UjyCaSZnHO-2A/exec";
    public int CompanySearch;
    public int SeachIndex = 1;

    void Awake()
    {
        inst = this;
        repository = new InventoryRepository(inventoryConfig);

        // ù ȭ�� Ȱ��ȭ
        if (Scenes != null)
        {
            for (int i = 0; i < Scenes.Length; i++)
                Scenes[i].SetActive(i == 0);
        }

        // ��Ӵٿ� ���� ������
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
            _loadingTicket++;                          // �� �ε� ����
            _loadingShownAt = Time.realtimeSinceStartup;
            _loadingHardDeadline = _loadingShownAt + loadingMaxTimeout;

            if (_loadingHideCo != null)                // ���� ���� ��� ���̸� ���
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
        if (_loadingCount > 0) return;                 // ���� ���� ���� �ε� ����

        float elapsed = Time.realtimeSinceStartup - _loadingShownAt;
        float remain = loadingMinDuration - elapsed;

        if (remain <= 0f)                               // �ּ� ǥ�ýð� ���� �� ��� ����
        {
            if (Loading != null) Loading.SetActive(false);
            _loadingHideCo = null;
        }
        else                                            // �ּ� �ð� ä��� ����
        {
            int expected = _loadingTicket;              // �� ������ Ƽ��
            if (_loadingHideCo != null) StopCoroutine(_loadingHideCo);
            _loadingHideCo = StartCoroutine(HideLoadingAfter(remain, expected));
        }
    }

    private System.Collections.IEnumerator HideLoadingAfter(float delay, int expectedTicket)
    {
        yield return new WaitForSeconds(delay);

        // ��� �� �� �ε��� ���۵Ǿ��ų�(_loadingTicket ����) �ٸ� �ε��� ���ư��� ������ ����
        if (_loadingCount == 0 && expectedTicket == _loadingTicket)
        {
            if (Loading != null) Loading.SetActive(false);
        }
        _loadingHideCo = null;
    }

    // ---------- Enrollment ----------
    bool SetIDPass()
    {
        // ������ �� ���. �ʿ� �� validation ��ȭ ����
        return true;
    }

    public void Register()
    {
        if (!SetIDPass()) { print("�߸��� �Է��Դϴ�."); return; }

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
            else print("���� ������ �����ϴ�.");
        }
        // ��� �� �ֽ� ������ ���� (All ����)
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
        NoneDrop.captionText.text = "�μ�";
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
    // ���� ���͸� �����ϰ� UI�� ����
    public void ApplyFiltersAndRefreshUI(System.Collections.Generic.List<FilterCondition> filters)
    {
        // repository�� ������ GameManager���� �����ص� �ʵ��� ����
        if (repository == null)
        {
            Debug.LogError("[GameManager] repository�� �����ϴ�.");
            return;
        }

        // 1) ���ڵ� ����
        var filtered = repository.ApplyFilters(filters);

        // 2) ���� ���� ������ ����
        MyCompanyDatabase = new System.Collections.Generic.List<CompanyList>();
        for (int i = 0; i < filtered.Count; i++)
        {
            var r = filtered[i];
            MyCompanyDatabase.Add(new CompanyList(
                r.Date, r.SubjectName, r.Receiving, r.Release, r.CompanyName, r.ReceivingTime, r.Gugo
            ));
        }

        // 3) ��Ӵٿ�/�ε��� �籸��
        BuildIndicesAndDropdowns();

        // 4) ȭ�� ���� (���� �帧 ����)
        if (ScrollViewController.Instance != null)
            ScrollViewController.Instance.DoSearch();

        // ������/���� �ؽ�Ʈ ���� �� ���� �ʿ��ϸ� ���⼭ ó��
        UpdateAllCountBanner(MyCompanyDatabase, null);
    }

    // ---------- ȭ�� �� ��ȯ ----------
    public void TabClick(string tabName)
    {
        CompanyDrop.captionText.text = "�⺻";
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

    // ---------- ������ �ε� ----------
    IEnumerator Lookup(string deptKey)
    {
        StartLoading();
        yield return repository.Refresh(deptKey);
        EndLoading();

        // �޸� ������ �� ���� ������ ���� ����
        MyCompanyDatabase = repository.Records.Select(r =>
            new CompanyList(r.Date, r.SubjectName, r.Receiving, r.Release, r.CompanyName, r.ReceivingTime, r.Gugo)
        ).ToList();

        BuildIndicesAndDropdowns();

        // ��� ǥ�� ����
        UpdateAllCountBanner(MyCompanyDatabase, null);

        if (curType != "Enrollment" && curType != "Main" && curType != "Lost")
            ScrollViewController.Instance.DoSearch();


    }

    void BuildIndicesAndDropdowns()
    {
        // �ʱ�ȭ
        CompanyData.Clear(); NameData.Clear(); curData.Clear(); NameCompanyData.Clear();
        if (dropdown != null && dropdown.Length > curScene && dropdown[curScene] != null)
            dropdown[curScene].options.Clear();

        // ����ũ ��ǰ/ȸ��
        var uniqueBySubject = MyCompanyDatabase
            .GroupBy(p => p.SubjectName).Select(g => g.First()).ToList();
        var uniqueCompany = MyCompanyDatabase.Select(p => p.CompanyName).Distinct().ToList();

        if (dropdown != null && dropdown.Length > curScene && dropdown[curScene] != null)
            dropdown[curScene].options.Add(new Dropdown.OptionData("��ü"));

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
        // �⺻�� ����
        if (CompanyDrop != null) CompanyDrop.value = 0;
        if (dropdown != null && dropdown.Length > curScene && dropdown[curScene] != null)
            dropdown[curScene].value = 0;
    }

    // ---------- �˻� (���� �帧 ����) ----------
    public int AAASearch() // �ŷ�ó �˻�
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

    public int ProductSearch() // �ؽ�Ʈ �˻�
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
        // count ��� ����
        UpdateAllCountBanner(MySearchData, null);

        return SetupPaging(count);
    }
    public void searchButtonDown(int id)
    {
        // id ���� üũ
        if (curData == null || id < 0 || id >= curData.Count) return;

        // ������ �̸��� ��� ȭ���� �Է�â�� �ݿ�
        if (SubjectInput != null)
        {
            SubjectInput.text = curData[id];
            // Ŀ���� �� ������
            try { SubjectInput.caretPosition = SubjectInput.text.Length; } catch { }
        }

        // ���� �� �ڵ��ϼ� ����Ʈ �ݱ�
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
        if (rem == 0 && save > 0) rem = PageObject;     // �� ������ �� ����

        int count = (Maxpage == Curpage) ? rem : PageObject;

        int ui = Curpage + 1;
        int uiMax = Maxpage + 1;
        if (AllSubjectCountText != null && AllSubjectCountText.Length > curScene && AllSubjectCountText[curScene] != null)
            AllSubjectCountText[curScene].text = $"[{ui}/{uiMax}]";

        RefreshPagingUI();                               // ȭ��ǥ ǥ�� ����
        return count;
    }



    public int SEadSearch() // ���
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
                if (dex < 0) AllCount[curScene].text = $"{MySearchData[0].SubjectName.Trim()} - ��� ���� : {dex}";
                else if (dex > 0) AllCount[curScene].text = $"{MySearchData[0].SubjectName.Trim()} - ���� ��� : {dex}";
                else AllCount[curScene].text = " - ��� ���� : 0";
            }
        }
        return dex;
    }
void UpdateAllCountBanner(List<CompanyList> source, string subjectFilter)
{
    if (AllCount == null || AllCount.Length == 0) return;

    int idx = curScene;
        // ���˻� + �� �ƴ� �� ��� ����
        if (isSed && !isSubject)
        {
            idx = curScene;
            if (AllCount != null && AllCount.Length > idx && AllCount[idx] != null)
                AllCount[idx].gameObject.SetActive(false);
            // ������ ī���� �ؽ�Ʈ�� �����ϰ� ������ ���⼭ ���� �ٷ缼��.
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

    string head = string.IsNullOrEmpty(subjectFilter) ? "��ü" : subjectFilter.Trim();
    string msg = dex < 0 ? (head + " - ��� ���� : " + dex) : (head + " - ���� ��� : " + dex);
    label.text = msg + "  (�԰� " + inQty + ", ��� " + outQty + ", �׸� " + rows + ")";
    label.gameObject.SetActive(true);

    // ������ ī���� �ؽ�Ʈ�� ���̰�
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
    // GameManager Ŭ���� �ȿ� �߰�
    void RefreshPagingUI()
    {
        // Maxpage: 0�̸� ������ 1��(0���)
        bool multiplePages = Maxpage > 0;
        bool showArrows = multiplePages && curType != "Lost";

        isArrow = showArrows; // ���� Update()���� �� ������ ǥ�� ��� �ϴ� ����
        if (LeftArrow != null) LeftArrow.SetActive(showArrows);
        if (RightArrow != null) RightArrow.SetActive(showArrows);
    }

    public void NextPage()
    {
        if (Maxpage <= 0) return;      // ������ 1���� �̵� �Ұ�
        if (Curpage < Maxpage) Curpage++;
        RefreshPagingUI();
        ScrollViewController.Instance?.DoSearch();
    }

    public void PrevPage()
    {
        if (Maxpage <= 0) return;      // ������ 1���� �̵� �Ұ�
        if (Curpage > 0) Curpage--;
        RefreshPagingUI();
        ScrollViewController.Instance?.DoSearch();
    }

    // Android ����Ű ó�� �� ȭ��ǥ ǥ�� ����
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
                // �ʹ� ���� ���� ������ ���� ���� (�����/������ġ)
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
