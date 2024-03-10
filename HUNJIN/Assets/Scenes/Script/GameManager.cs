using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;
using System.Linq;
[System.Serializable]
public class CompanyList
{
    public CompanyList(string _Date, string _SubjectName, string _Receiving, string _Release, string _CompanyName, string _Time)
    { Date = _Date; SubjectName = _SubjectName; Receiving = _Receiving; Release = _Release; CompanyName = _CompanyName; ReceivingTime = _Time; }

    //재고 이동(++날짜 , 회사명, 시간,

    public string Date, SubjectName = "None", Receiving = "0", Release = "0", CompanyName = "None", ReceivingTime = "";
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
    // 그냥 검색된 메서스를 넣는 변수를 만들어서 처리하기
    public static List<T> removeDuplicates<T>(List<T> list)
    {
        return new HashSet<T>(list).ToList();
    }


    public bool isCompanyName;
    static GameManager inst;
    public TextAsset SubjectDatebase;
    public ScrollViewEnom scrollViewEnom;
    public ScrollViewController scrollViewController;
    public int curScene;

    //Subject 관리함수
    public bool isSed;
    public bool isSubject = false;


    public GameObject main;
    public List<CompanyList> MyCompanyDatabase;
    public List<CompanyList> MySearchData;
    public List<CompanyList> DoSearchData;
    public List<NameCompanyList> NameCompanyData;

    public Animator animator;

    public string curType = "Main";

    public GameObject[] Scenes;

    string ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEdropdown[curScene].bXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=1973018837&range=K2:Q";//전해업체
    string CodeURL = "https://script.google.com/macros/s/AKfycbyTjQ7vaLoT1KB5XV6fQah_GohTipaadENBkjuAQi0FYN0XFmgqJocwOO5BvWV2aRMGrQ/exec";//코드

    public int CompanyType;
    public int CompanySearch;
    private bool subfirst = true;
    string filePath;
    public int SeachIndex = 1;

    public TMP_InputField[] SubjectNameSearch;
    public TextMeshProUGUI[] AllSubjectCountText;
    public TextMeshProUGUI[] AllCount;
    public GameObject[] CheckBoxs;
    public Dropdown[] dropdown;
    public Dropdown[] Searchdropdown;
    // SearchScene
    public int Curpage = 0;
    private int Maxpage = 1;
    public int PageObject = 10;
    public GameObject LeftArrow;
    public GameObject RightArrow;
    bool isArrow = false;

    // ENROLLMENT 
    public Dropdown CompanyDrop, NoneDrop;
    public TMP_InputField SubjectInput;
    public InputField DateInput, TimeInput, ReleaseInput, ReceivingInput;
    string Date, SubjectName, Release, Receiving, CompanyName, ReceivingTime, None;
    public List<string> CompanyData;
    public List<string> NameData;
    public List<string> EnomData;
    public List<string> curData;
    public GameObject Loading;
    public bool isloading;

    public static GameManager Instance
    {
        get
        {
            if (null == inst)
            {
                return null;
            }
            return inst;
        }
    }
    void Awake()
    {
        for (int i = 0; i < Scenes.Length; i++)
        {
            if (i == 0)
                Scenes[i].gameObject.SetActive(true);
            else
                Scenes[i].gameObject.SetActive(false);
        }
        inst = this;
        for (int i = 0; i < 4; i++)
            dropdown[i].onValueChanged.AddListener(OnDropdownEvent);
    }

    public void Resetdropdown()
    {
        SubjectNameSearch[curScene].text = "";
    }

    public void SetLoading()
    {
        isloading = true;
    }
    public void searchButtonDown(int i)
    {
        SubjectInput.text = curData[i];
        curData.Clear();
        scrollViewEnom.UpdateScrollView();
    }
    public void Start()
    {
        isCompanyName = true;
        subfirst = true;
        isSubject = false;
        filePath = Application.persistentDataPath + "/MySubjectText.txt";
        Debug.Log(filePath);
        DateInput.text = DateTime.Now.ToString("yy-MM-dd");
        TimeInput.text = DateTime.Now.ToString("HH:mm");
        Date = DateInput.text;
        ReceivingTime = TimeInput.text;
        AllSubjectCountText[curScene].text = "[" + DoSearchData.Count.ToString() + "/" + DoSearchData.Count.ToString() + "]";
        for (int i = 0; i < CheckBoxs.Length; i++)
        {
            CheckBoxs[i].SetActive(false);
        }


        SubjectInput.onValueChanged.AddListener(OnInputValueChanged);
        // 여기 
    }

    private void PopulateDropdown(List<string> items) // 초기화하고 리스트 넣기
    {
        Searchdropdown[curScene].ClearOptions();
        Searchdropdown[curScene].AddOptions(items);
    }
    public void OnDropdd()
    {
        int selectedIndex = Searchdropdown[curScene].value;

        // 현재 선택된 옵션의 텍스트 얻기
        string selectedText = Searchdropdown[curScene].options[selectedIndex].text;

        SubjectNameSearch[curScene].text = selectedText;
    }
    public void OnInputValueChanged(string text)
    {
        curData.Clear();
        int currentIndex = CompanyDrop.value;
        for (int i = 0; i < NameCompanyData.Count; i++)
        {
            if (CompanyDrop.options[currentIndex].text == NameCompanyData[i].Company)
            {
                if (NameCompanyData[i].Name.Trim().ToLower().Contains(SubjectInput.text.Trim().ToLower()))
                    curData.Add(NameCompanyData[i].Name);
            }
        }


        scrollViewEnom.UpdateScrollView();
    }

    public int GetSeachResult()
    {
        // DayRemove();

        int result = DoSearchData.Count;

        if (result >= 50)
        {
            if (result > SeachIndex * 50)
            {
                result = SeachIndex * 50;
            }
        }

        return result;
    }

    public int GetIndexResult(int a)
    {

        int result = a;

        if (a >= 50)
        {
            if (a > SeachIndex * 50)
            {
                result = SeachIndex * 50;
            }
        }
        return result;
    }
    public void OnDropdownEvent(int index) // 이렇게하면 index가 알아서 바뀜
    {
        dropdown[curScene].value = index;
        CompanySearch = index;

    }
    public void ResetData()
    {
        isArrow = true;
        Curpage = 0;
        DateInput.text = DateTime.Now.ToString("yy/M/d");
        TimeInput.text = DateTime.Now.ToString("HH:mm");
        AllSubjectCountText[curScene].text = "[" + GetSeachResult() + "/" + DoSearchData.Count.ToString() + "]";
        SubjectInput.text = "";
        ReleaseInput.text = "";
        ReceivingInput.text = "";
        NoneDrop.captionText.text = "부서";
        AllCount[curScene].text = "";
        scrollViewController.ResetEnId();
        //main.SetActive(false);
        //if (Time.frameCount % 30 == 0)
        //{
        //    System.GC.Collect(); // 청소코드 인게임 중이아니라 로딩씬일떄 
        //}
        isCompanyName = true;
    }
    public void CheckBoxF(int a)
    {
        CheckBoxs[a].SetActive(false);
    }
    public void CheckBoxT(int a)
    {
        CheckBoxs[a].SetActive(true);
    }
    public void TabClick(string tabName)
    {
        CompanyDrop.captionText.text = "거래처";
        // if (tabName == "Subject" || tabName == "Material")
        //     animator.SetTrigger("Start");
        MySearchData.Clear();
        SubjectNameSearch[curScene].text = "";
        ResetData();
        OnDropdownEvent(0);
        curData.Clear();
        scrollViewEnom.UpdateScrollView();
        if (!subfirst && curType != "Main" && curType != "Enrollment")
        {
            ScrollViewController.Instance.UIObjectReset();
        }


        if (tabName != "None")
        {
            int tabNum = 5;
            switch (tabName)
            {
                //Press, Welding, Assembly, All
                case "Main": tabNum = 0; break;
                case "All": tabNum = 5; ; break;
                case "Press": tabNum = 1; ; break;
                case "Enrollment": tabNum = 2; break;
                case "Welding": tabNum = 3; break;
                case "Assembly": tabNum = 4; break;
                case "Lost": tabNum = 6; break;
            }
            for (int i = 0; i < Scenes.Length; i++)
            {
                if (tabNum == i)
                    Scenes[tabNum].gameObject.SetActive(true);
                else
                {
                    for (int j = 0; j < Scenes.Length; j++)
                    {
                        if (tabNum != j)
                            Scenes[j].gameObject.SetActive(false);
                    }
                }
            }
            if (subfirst)
                subfirst = false;
        }

        if (tabName != "Enrollment" && tabName != "Main")
        {
            StartCoroutine(Lookup(tabName));
            main.SetActive(true);
            if (tabName == "Lost")
                StartCoroutine(Lookup("All"));
        }
        else
        {
            if (tabName == "Enrollment")
                StartCoroutine(Lookup("All"));

            main.SetActive(false);
        }
        switch (tabName)
        {
            case "Press":
                curScene = 0;
                break;
            case "Welding":
                curScene = 1;
                break;
            case "Assembly":
                curScene = 2;
                break;
            case "All":
                curScene = 3;
                break;
            case "Lost":
                curScene = 3;
                break;
            default: //main 이거나 enrollment 추가됨!,Lost 일떄 또는 none
                curScene = 0;
                isArrow = false;
                break;
        }

        curType = tabName;

    }
    public int AAASearch()// 거래처 검색용
    {
        isArrow = true;
        isSed = true;
        var distPerson = MyCompanyDatabase.Select(person => new { person.SubjectName, person.CompanyName }).Distinct().ToList();
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        int saveCount = 0;
        foreach (var obj in distPerson)
        {
            // "22", "22", "22", "22", "22", "22", "22" 
            if (obj.CompanyName.Trim().ToLower().Contains(dropdown[curScene].options[dropdown[curScene].value].text.Trim().ToLower()))
            { 
                MySearchData.Add(new CompanyList("10/31", obj.SubjectName, "1", "1", obj.CompanyName, "10:10"));
                count++;
            }
        }

        saveCount = count;
        count /= PageObject;
        Maxpage = count;
        count = PageObject;
        if (Maxpage == Curpage)
        {
            saveCount %= PageObject;
            count = saveCount;
        }
        //for (int i = 0; i < DoSearchData.Count; i++)
        //{
        //    if (DoSearchData[i].SubjectName.Trim().ToLower().Contains(SubjectNameSearch[curScene].text.Trim().ToLower()))
        //    {
        //        MySearchData.Add(DoSearchData[i]);
        //        count++;
        //    }
        //}
        int TextUI = Curpage + 1;
        int MaxTextUI = Maxpage + 1;
        AllSubjectCountText[curScene].text = "[" + TextUI.ToString() + "/" + MaxTextUI.ToString() + "]";
        return count;
    }
    public int ProductSearch()//텍스트 들어가는 검색
    {
        isArrow = true;
        isSed = true;
        var distPerson = MyCompanyDatabase.Select(person => new { person.SubjectName, person.CompanyName }).Distinct().ToList();
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        int saveCount = 0;
        foreach (var obj in distPerson)
        {
            // "22", "22", "22", "22", "22", "22", "22" 
            if (obj.SubjectName.Trim().ToLower().Contains(SubjectNameSearch[curScene].text.Trim().ToLower()))
            {
                MySearchData.Add(new CompanyList("10/31", obj.SubjectName, "1", "1", obj.CompanyName, "10:10"));
                count++;
            }
        }

        saveCount = count;
        count /= PageObject;
        Maxpage = count;
        count = PageObject;
        if (Maxpage == Curpage)
        {
            saveCount %= PageObject;
            count = saveCount;
        }
        //for (int i = 0; i < DoSearchData.Count; i++)
        //{
        //    if (DoSearchData[i].SubjectName.Trim().ToLower().Contains(SubjectNameSearch[curScene].text.Trim().ToLower()))
        //    {
        //        MySearchData.Add(DoSearchData[i]);
        //        count++;
        //    }
        //}
        int TextUI = Curpage + 1;
        int MaxTextUI = Maxpage + 1;
        AllSubjectCountText[curScene].text = "[" + TextUI.ToString() + "/" + MaxTextUI.ToString() + "]";
        return count;


    }
    public int ProductSearch(string text)//over 들어가는 검색
    {
        MySearchData.Clear();

        int count = 0;
        for (int i = 0; i < MyCompanyDatabase.Count; i++)
        {
            if (MyCompanyDatabase[i].SubjectName.Trim().ToLower().Contains(SubjectNameSearch[curScene].text.Trim().ToLower()))
            {
                MySearchData.Add(MyCompanyDatabase[i]);
                count++;

            }
        }
        AllSubjectCountText[curScene].text = "";
        isArrow = false;

        return count;
    }
    public void NextPage()
    {
        if (Curpage < Maxpage)
            Curpage++;
    }

    public void PrevPage()
    {
        if (Curpage > 0)
            Curpage--;
    }

    public int SEadSearch()//요약검색
    {
        isArrow = true;
        isSed = true;
        var distPerson = MyCompanyDatabase.Select(person => new { person.SubjectName, person.CompanyName }).Distinct().ToList();
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        int saveCount = 0;
        foreach (var obj in distPerson)
        {
            // "22", "22", "22", "22", "22", "22", "22" 
            MySearchData.Add(new CompanyList("10/31", obj.SubjectName, "1", "1", obj.CompanyName, "10:10"));
            count++;
        }

        saveCount = count;
        count /= PageObject;
        Maxpage = count;
        count = PageObject;
        if (Maxpage == Curpage)
        {
            saveCount %= PageObject;
            count = saveCount;
        }
        //for (int i = 0; i < DoSearchData.Count; i++)
        //{
        //    if (DoSearchData[i].SubjectName.Trim().ToLower().Contains(SubjectNameSearch[curScene].text.Trim().ToLower()))
        //    {
        //        MySearchData.Add(DoSearchData[i]);
        //        count++;
        //    }
        //}
        int TextUI = Curpage + 1;
        int MaxTextUI = Maxpage + 1;
        AllSubjectCountText[curScene].text = "[" + TextUI.ToString() + "/" + MaxTextUI.ToString() + "]";
        return count;
    }
    public int DOTextSearch(String text)
    {
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        for (int i = 0; i < MyCompanyDatabase.Count; i++)
        {
            if (MyCompanyDatabase[i].SubjectName.Trim() == (text.Trim()))
            {
                MySearchData.Add(MyCompanyDatabase[i]);
                count++;
            }
        }

        SubjectNameSearch[curScene].text = text.Trim();
        AllSubjectCountText[curScene].text = "[" + count.ToString() + "/" + count.ToString() + "]";
        int dex = 0;
        for (int i = 0; i < count; i++)
        {
            MySearchData[i].Release.Replace(",", "");
            MySearchData[i].Receiving.Replace(",", "");
            dex += int.Parse(MySearchData[i].Release);
            dex -= int.Parse(MySearchData[i].Receiving);
        }
        return count;
    }
    public int LostTextSearch() // Lost
    {
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        for (int i = 0; i < DoSearchData.Count; i++)
        {
            MySearchData.Add(DoSearchData[i]);
            count++;
        }
        AllSubjectCountText[curScene].text = "[" + count.ToString() + "/" + count.ToString() + "]";
        return count;
    }
    public int GetSubjectRemaining(int id)
    {

        int dex = 0;
        if (!isCompanyName)
        {
            for (int i = 0; i <= id; i++)
            {
                if (MySearchData[id].SubjectName.Trim() == MySearchData[i].SubjectName.Trim())
                {
                    MySearchData[i].Release.Replace(",", "");
                    MySearchData[i].Receiving.Replace(",", "");
                    dex += int.Parse(MySearchData[i].Release);
                    dex -= int.Parse(MySearchData[i].Receiving);
                }
            }
            if (dex < 0)
            {
                AllCount[curScene].text = MySearchData[0].SubjectName.Trim() + " - 재고 부족 : " + dex.ToString();
            }
            else if (dex > 0)
            {
                AllCount[curScene].text = MySearchData[0].SubjectName.Trim() + " - 현재 재고 : " + dex.ToString();
            }
            else
            {
                AllCount[curScene].text = " - 재고 부족 : " + dex.ToString();
            }

        }

        return dex;
    }
    public string[] AllGetSearch(int id)
    {
        string[] jdata = { "22", "22", "22", "22", "22", "22", "22" };

        jdata[0] = MyCompanyDatabase[id].Date;
        jdata[1] = MyCompanyDatabase[id].SubjectName;
        jdata[2] = MyCompanyDatabase[id].Release;
        jdata[3] = MyCompanyDatabase[id].Receiving;
        jdata[4] = MyCompanyDatabase[id].CompanyName;
        jdata[6] = MyCompanyDatabase[id].ReceivingTime;

        return jdata;
    }
    public string[] DoGetSearch(int id)
    {
        string[] jdata = { "22", "22", "22", "22", "22", "22", "22" };

        if (DoSearchData.Count < id)
            return jdata;
        jdata[0] = DoSearchData[id].Date;
        jdata[1] = DoSearchData[id].SubjectName;
        jdata[2] = DoSearchData[id].Release;
        jdata[3] = DoSearchData[id].Receiving;
        jdata[4] = DoSearchData[id].CompanyName;
        jdata[6] = DoSearchData[id].ReceivingTime;

        return jdata;
    }
    public string[] GetSearch(int id)// Date Name Rel Rece ComName Com
    {
        string[] jdata = { "22", "22", "22", "22", "22", "22", "22" };


        jdata[0] = MySearchData[id].Date;
        jdata[1] = MySearchData[id].SubjectName;
        jdata[2] = MySearchData[id].Release;
        jdata[3] = MySearchData[id].Receiving;
        jdata[4] = MySearchData[id].CompanyName;
        jdata[6] = MySearchData[id].ReceivingTime;

        return jdata;
    }
    public void Load()
    {
        MyCompanyDatabase.Clear();
        CompanyData.Clear();
        NameData.Clear();
        curData.Clear();
        NameCompanyData.Clear();
        dropdown[curScene].options.Clear();
        string jdata = File.ReadAllText(filePath);
        string[] line = jdata.Substring(0, jdata.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            if (row.Length > 5)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (row[j] == "")
                    {
                        row[j] = "0";
                    }
                }
                MyCompanyDatabase.Add(new CompanyList(row[0].Replace("-", "/"), row[1].Replace(" ", ""), row[2].Replace(",", ""), row[3].Replace(",", ""), row[4], "None"));
            }

        }
        ResetData();
        var AehdIa = MyCompanyDatabase
                    .GroupBy(person => person.SubjectName) // SubjectName을 기준으로 그룹화
                    .Select(group => group.First())
                    .Distinct()
                    .ToList();
        var AehdCompany = MyCompanyDatabase
            .Select(person => person.CompanyName)
            .Distinct()
            .ToList();

        dropdown[curScene].options.Add(new Dropdown.OptionData("전체"));
        foreach (var item in AehdIa)
        {
            NameData.Add(item.SubjectName);
            NameCompanyData.Add(new NameCompanyList(item.SubjectName, item.CompanyName));
        }
        foreach (var item in AehdCompany)
        {
            CompanyData.Add(item);
        }
        for (int i = 0; i < CompanyData.Count; i++)
        {
            dropdown[curScene].options.Add(new Dropdown.OptionData(CompanyData[i]));
            CompanyDrop.options.Add(new Dropdown.OptionData(CompanyData[i]));
        }
        if (curType != "Enrollment" && curType != "Main" && curType != "Lost")
            ScrollViewController.Instance.DoSearch();
    }
    // 여기서 부터   
    bool SetIDPass()
    {
        SubjectName = SubjectInput.text.Trim();
        Release = ReleaseInput.text.Trim();
        Receiving = ReceivingInput.text.Trim();
        CompanyName = CompanyDrop.captionText.text.Trim();
        ReceivingTime = TimeInput.text.Trim();
        Date = DateInput.text.Trim();
        None = NoneDrop.captionText.text.Trim();
        return true;
    }
    public void Register() //등록
    {
        if (!SetIDPass())
        {
            print("잘못된 입력입니다.");
            return;
        }
        else
        {
            CheckBoxs[0].SetActive(true);
        }
        WWWForm form = new WWWForm();
        form.AddField("order", "register");

        form.AddField("date", Date);
        form.AddField("subject", SubjectName);
        form.AddField("release", Release);
        form.AddField("receiving", Receiving);
        form.AddField("companyName", CompanyName);
        form.AddField("rtime", ReceivingTime);
        form.AddField("none", None);

        ResetData();
        curData.Clear();
        scrollViewEnom.UpdateScrollView();
        CompanyDrop.value = 0;
        NoneDrop.value = 0;
        StartCoroutine(Post(form));
    }
    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(CodeURL, form)) // 반드시 using을 써야한다
        {
            yield return www.SendWebRequest();

            if (www.isDone) print(www.downloadHandler.text);
            else print("웹의 응답이 없습니다.");
        }
        Load();
    }
    void StartLoading()
    {
        Loading.SetActive(true);
    }
    void EndLoading()
    {
        Loading.SetActive(false);
    }
    IEnumerator Lookup(string curType)
    {
        //@@ 추가 Press, Welding, Assembly, All
        if (curType == "Press")
            ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=0&range=K2:Q";
        else if (curType == "All")
            ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=1973018837&range=K2:Q";
        else if (curType == "Welding")
            ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=1809169708&range=K2:Q";
        else if (curType == "Assembly")
            ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=334896260&range=K2:Q";
        else if (curType == "Lost")
            ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=632245483&range=A2:G";

        UnityWebRequest www = UnityWebRequest.Get(ElectrolyteURL);
        StartLoading();
        yield return www.SendWebRequest();
        yield return new WaitForSeconds(0.35f);

        if (www.isDone)
        {
            File.WriteAllText(filePath, www.downloadHandler.text);
            EndLoading();
            if (isloading)
            {
                CheckBoxs[1].SetActive(true);
                isloading = false;
            }

            Load();
            //animator.SetTrigger("End");
        }

        else print("웹의 응답이 없습니다.");
    }

    public static void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Home))
            {
                Exit();
            }
            else if (Input.GetKey(KeyCode.Escape))
            {
                Exit();
            }
            else if (Input.GetKey(KeyCode.Menu))
            {
                Exit();
            }
        }
        if (curType == "Lost")
        {
            LeftArrow.SetActive(false);
            RightArrow.SetActive(false);
        }
        else
        {
            LeftArrow.SetActive(isArrow);
            RightArrow.SetActive(isArrow);
        }
    }
}