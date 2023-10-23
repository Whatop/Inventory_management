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

    public List<CompanyList> MyCompanyDatabase;
    public List<CompanyList> MySearchData;
    public List<CompanyList> DoSearchData;

    public Animator animator;

    public string curType = "Main";
    int curInt;
    public GameObject[] Scenes;


    public TMP_InputField SubjectNameSearch;
    string filePath;

    string ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=1973018837&range=K2:Q";//전해업체
    string CodeURL = "https://script.google.com/macros/s/AKfycbyTjQ7vaLoT1KB5XV6fQah_GohTipaadENBkjuAQi0FYN0XFmgqJocwOO5BvWV2aRMGrQ/exec";//코드

    // ENROLLMENT 
    public Dropdown CompanyDrop, NoneDrop;
    public InputField DateInput, TimeInput, SubjectInput, ReleaseInput, ReceivingInput;
    string Date, SubjectName, Release, Receiving, CompanyName, ReceivingTime, None;
    public List<string> CompanyData;
    public List<string> NameData;
    Dictionary<string, int> uniqueDictionary = new Dictionary<string, int>();
    Dictionary<string, int> uniqueNameDictionary = new Dictionary<string, int>();

    DateTime _dateTime;
    public int CompanyType; /// 건들면 코드 망가질수도 있음
    public int CompanySearch;
    public TextMeshProUGUI AllSubjectCountText;
    private bool subfirst = true;

    public TextMeshProUGUI AllCount;
    public GameObject[] CheckBoxs;
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Dropdown SearchDropdown;

    public bool isSubject = false;

    public int SeachIndex = 1;

    public Image Image1;
    public Image Image2;
    public TextMeshProUGUI text1;


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
        dropdown.onValueChanged.AddListener(OnDropdownEvent);
    }

    public void Resetdropdown()
    {
        SubjectNameSearch.text = "";
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
        AllSubjectCountText.text = "[" + DoSearchData.Count.ToString() + "/" + DoSearchData.Count.ToString() + "]";
        for (int i = 0; i < CheckBoxs.Length; i++)
        {
            CheckBoxs[i].SetActive(false);
        }

        PopulateDropdown(NameData);
        SubjectNameSearch.onValueChanged.AddListener(OnInputValueChanged);
    }
    private void PopulateDropdown(List<string> items) // 초기화하고 리스트 넣기
    {
        SearchDropdown.ClearOptions();
        SearchDropdown.AddOptions(items);
    }
    public void OnDropdd()
    {
        int selectedIndex = SearchDropdown.value;

        // 현재 선택된 옵션의 텍스트 얻기
        string selectedText = SearchDropdown.options[selectedIndex].text;

        SubjectNameSearch.text = selectedText;
    }
    private void OnInputValueChanged(string text)
    {
        List<string> filteredOptions = NameData.FindAll(option => option.StartsWith(text, System.StringComparison.OrdinalIgnoreCase));
        PopulateDropdown(filteredOptions);
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
        dropdown.value = index;
       CompanySearch = index;

    }
    public void ResetData()
    {
        DateInput.text = DateTime.Now.ToString("yy/M/d");
        TimeInput.text = DateTime.Now.ToString("HH:mm");
        AllSubjectCountText.text = "[" + GetSeachResult() + "/" + DoSearchData.Count.ToString() + "]";
        SubjectInput.text = "제품명";
        ReleaseInput.text = "출고";
        ReceivingInput.text = "입고";
        CompanyDrop.captionText.text = "거래처";
        NoneDrop.captionText.text = "부서";
        AllCount.text = "잔고";
        if (Time.frameCount % 30 == 0)
        {
            System.GC.Collect(); // 청소코드 인게임 중이아니라 로딩씬일떄 
        }
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
        // if (tabName == "Subject" || tabName == "Material")
        //     animator.SetTrigger("Start");
        MySearchData.Clear();
        SubjectNameSearch.text = "";
      
        OnDropdownEvent(0);
        if (!subfirst)
        {
            ScrollViewController.Instance.UIObjectReset();
            subfirst = true;
        }

        curType = tabName;
        if (tabName != "None")
        {
            int tabNum = 5;
            switch (tabName)
            {
                //Press, Welding, Assembly, All
                case "Main": tabNum = 0; break;
                case "All": tabNum = 1; ; break;
                case "Press": tabNum = 1; ; break;
                case "Enrollment": tabNum = 2; break;
                case "None": tabNum = 3; break;
                case "Welding": tabNum = 1; break;
                case "Assembly": tabNum = 1; break;
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

        }
        if (tabName == "Press")
        {
            Image1.color = new Color(0.4273228f, 0.409434f, 1f); //OUT
            Image2.color = new Color(0.5273228f, 0.409434f, 1f); // IN
            text1.text = "유압 관리";
        }
        else if (tabName == "All")
        {
            Image1.color = new Color(0.03588462f, 0.5943396f, 0.04296107f); // OUT
            Image2.color = new Color(0.3629665f, 0.8773585f, 0.1522962f);  // IN
            text1.text = "모든제품 관리";
        }
        else if (tabName == "Welding")
        {
            Image1.color = new Color(0.9150943f, 0.2495712f, 0f); //OUT
            Image2.color = new Color(0.9607844f, 0.3607843f, 0.1372549f); // IN
            text1.text = "용접 관리";
        }
        else if (tabName == "Assembly")
        {
            Image1.color = new Color(0.8207547f, 0.1664738f, 0.5439883f); //OUT
            Image2.color = new Color(0.8607844f, 0.13607843f, 0.1372549f); // IN
            text1.text = "조립 관리";
        }

        if(tabName != "Enrollment")
        StartCoroutine(Lookup(curType));
        else
        StartCoroutine(Lookup("All"));

    }
    public int AAASearch()// 거래처 검색용
    {
        MySearchData.Clear();

        int count = 0;
        for (int i = 0; i < MyCompanyDatabase.Count; i++)
        {
            if (MyCompanyDatabase[i].CompanyName.Trim().ToLower().Contains(dropdown.options[dropdown.value].text.Trim().ToLower()))
            {
                MySearchData.Add(MyCompanyDatabase[i]);
                count++;

            }
        }
        AllSubjectCountText.text = "[" + count.ToString() + "/" + MyCompanyDatabase.Count.ToString() + "]";

        return count;
    }
    public int ProductSearch()//텍스트 들어가는 검색
    {
        MySearchData.Clear();

        int count = 0;
        for (int i = 0; i < MyCompanyDatabase.Count; i++)
        {
            if (MyCompanyDatabase[i].SubjectName.Trim().ToLower().Contains(SubjectNameSearch.text.Trim().ToLower()))
            {
                    MySearchData.Add(MyCompanyDatabase[i]);
                    count++;
               
            }
        }
        AllSubjectCountText.text = "[" + count.ToString() + "/" + MyCompanyDatabase.Count.ToString() + "]";

        return count;
    }
    public int SEadSearch()//요약검색
    {
        var distPerson = MyCompanyDatabase.Select(person => new { person.SubjectName }).Distinct().ToList();
        DoSearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        foreach (var obj in distPerson)
        {
            DoSearchData.Add(new CompanyList(count.ToString(), obj.SubjectName, "1", "1", "1", "1"));
        }

        if (CompanyType == 0)
        {
            for (int i = 0; i < DoSearchData.Count; i++)
            {
                if (DoSearchData[i].SubjectName.Trim().ToLower().Contains(SubjectNameSearch.text.Trim().ToLower()))
                {
                    MySearchData.Add(DoSearchData[i]);
                    count++;
                }
            }
        }

        AllSubjectCountText.text = "[" + count.ToString() + "/" + DoSearchData.Count.ToString() + "]";

        return count;
    } 

    public int TextSearch(String text) 
    {
        MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        for (int i = 0; i < MyCompanyDatabase.Count; i++)
        {
            if (MyCompanyDatabase[i].SubjectName.Trim().Contains(text.Trim()))
            {
                MySearchData.Add(MyCompanyDatabase[i]);
                count++;
            }
        }
        SubjectNameSearch.text = text.Trim();
        AllSubjectCountText.text = "[" + count.ToString() + "/" + GetSeachResult() + "]";

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

        SubjectNameSearch.text = text.Trim();
        AllSubjectCountText.text = "[" + count.ToString() + "/" + count.ToString() + "]";
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
    public int ALLDOTextSearch()
    {   MySearchData.Clear();
        OnDropdownEvent(CompanySearch);
        int count = 0;
        for (int i = 0; i < MyCompanyDatabase.Count; i++)
        {
                MySearchData.Add(MyCompanyDatabase[i]);
                count++;
        }
        AllSubjectCountText.text = "[" + count.ToString() + "/" + count.ToString() + "]";
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
                AllCount.text = MySearchData[0].SubjectName.Trim() + " - 회수 예정 : " + dex.ToString();
            }
            else if (dex > 0)
            {
                AllCount.text = MySearchData[0].SubjectName.Trim() + " - 회수 초과 : " + dex.ToString();
            }
            else
            {
                AllCount.text = " - 회수분 모두 수거완료 : " + dex.ToString();
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
        dropdown.options.Clear();
        uniqueDictionary.Clear();
        uniqueNameDictionary.Clear();
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
                CompanyData.Add(row[4]);
                NameData.Add(row[1]);
            }

        }
        ResetData();
        dropdown.options.Add(new Dropdown.OptionData("전체"));
        foreach (string item in CompanyData)
        {
            if (!uniqueDictionary.ContainsKey(item))
            {
                uniqueDictionary.Add(item, 0);
            }
        }
        List<string> uniqueList = uniqueDictionary.Keys.ToList();
        foreach (string item in uniqueList)
        {
            dropdown.options.Add(new Dropdown.OptionData(item));
            Debug.Log(item);

        } foreach (string item in NameData)
        {
            if (!uniqueNameDictionary.ContainsKey(item))
            {
                uniqueNameDictionary.Add(item, 0);
            }
        }
        List<string> uniqueNList = uniqueNameDictionary.Keys.ToList();
        foreach (string item in uniqueList)
        {
            dropdown.options.Add(new Dropdown.OptionData(item));
            Debug.Log(item);
        }
        if (curType != "Enrollment" && curType != "Main")
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
        
        UnityWebRequest www = UnityWebRequest.Get(ElectrolyteURL);

        yield return www.SendWebRequest();

        if (www.isDone)
        {
            File.WriteAllText(filePath, www.downloadHandler.text);
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
    }
}