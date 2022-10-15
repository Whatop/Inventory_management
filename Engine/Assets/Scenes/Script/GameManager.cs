using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using System;

[System.Serializable]
public class CompanyList
{
    public CompanyList(string _Date, string _SubjectName, string _Receiving, string _Release,  string _CompanyName, string _Time)
    { Date = _Date; SubjectName = _SubjectName; Receiving = _Receiving; Release = _Release;  CompanyName = _CompanyName; ReceivingTime = _Time; }

    //재고 이동(++날짜 , 회사명, 시간,

    public string Date, SubjectName = "None", Receiving = "0", Release = "0",  CompanyName = "None", ReceivingTime = "";
}



public class GameManager : MonoBehaviour
{
    // 그냥 검색된 메서스를 넣는 변수를 만들어서 처리하기

    public bool isCompanyName;
    static GameManager inst;
    public TextAsset SubjectDatebase;

    public List<CompanyList> MyCompanyDatabase;
    public List<CompanyList> MySearchData;

    public string curType = "Main";
    int curInt;
    public GameObject[] Scenes;

    public Text text1;

    public TMP_InputField SubjectNameSearch;
    string filePath;

    const string ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&range=A2:F";//전해업체
    const string CodeURL = "https://script.google.com/macros/s/AKfycbzPItu8K0sqevCvWmi82w8BUY-U0K2F446NmZfnZ1OLrcPwm7hotZiRRMUK81ePg3eONQ/exec";//코드

    public InputField DateInput, TimeInput, SubjectInput, ReleaseInput, ReceivingInput, CompanyNameInput;
    string Date, SubjectName, Release, Receiving, CompanyName, ReceivingTime;

    public int CompanyType;
    public TextMeshProUGUI AllSubjectCountText;
    private bool subfirst = false;

    public TextMeshProUGUI AllCount;
    public GameObject[] CheckBoxs ;
    [SerializeField]
    private Dropdown dropdown;
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

    public void Start()
    {
		isCompanyName = true;
        subfirst = true;
        for (int i = 0; i < Scenes.Length; i++)
        {
            if (i == 1)
                Scenes[i].gameObject.SetActive(true);
            else
                Scenes[i].gameObject.SetActive(false);
        }
        filePath = Application.persistentDataPath + "/MySubjectText.txt";
        Debug.Log(filePath);
        DateInput.text = DateTime.Now.ToString("M/d");
        TimeInput.text = DateTime.Now.ToString("t");
        Date = DateInput.text;
        ReceivingTime = TimeInput.text;
        StartCoroutine(Lookup());
        Load();
        AllSubjectCountText.text = "[" + MyCompanyDatabase.Count.ToString() + "/" + MyCompanyDatabase.Count.ToString() + "]";
        for(int i = 0; i < CheckBoxs.Length; i++)
        {
            CheckBoxs[i].SetActive(false);
        }
    }



    public void OnDropdownEvent(int index) // 이렇게하면 index가 알아서 바뀜
    {
        dropdown.value = index;
        CompanyType = index;
        if (index == 1)
        {
            AllCount.text = "잔고";
        }
    }
    public void ResetData()
    {
        DateInput.text = DateTime.Now.ToString("M/d");
        TimeInput.text = DateTime.Now.ToString("t");
        AllSubjectCountText.text = "[" + MyCompanyDatabase.Count.ToString() + "/" + MyCompanyDatabase.Count.ToString() + "]";
        SubjectInput.text = "";
        ReleaseInput.text = "";
        ReceivingInput.text = "";
        CompanyNameInput.text = "";
        AllCount.text = "잔고";
        if(Time.frameCount % 30 == 0)
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
        MySearchData.Clear();
        SubjectNameSearch.text = "";
        curType = tabName;
        if (tabName != "None")
        {
            int tabNum = 5;
            switch (tabName)
            {
                case "Main": tabNum = 0; break;
                case "Subject": tabNum = 1; ; break;
                case "Enrollment": tabNum = 2; break;
                case "None": tabNum = 3; break;
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

        StartCoroutine(Lookup());
        Load();
        if (curType == "Subject")
        {
            ResetData();
            if (subfirst)
                ScrollViewController.Instance.AllSearch();
            subfirst = true;

        }
    }
    public int ProductSearch()
    {
        MySearchData.Clear();

        int count = 0;
        if (CompanyType == 0)
        {
            for (int i = 0; i < MyCompanyDatabase.Count; i++)
            {
                if (MyCompanyDatabase[i].SubjectName.Trim().ToLower().Contains(SubjectNameSearch.text.Trim()))
                {
                    MySearchData.Add(MyCompanyDatabase[i]);
                    count++;
                }
            }
        }
        else
        {
            for (int i = 0; i < MyCompanyDatabase.Count; i++)
            {
                if (MyCompanyDatabase[i].CompanyName.Trim().ToLower().Contains(SubjectNameSearch.text.Trim()))
                {
                    MySearchData.Add(MyCompanyDatabase[i]);
                    count++;
                }
            }
        }
        AllSubjectCountText.text = "[" + count.ToString() + "/" + MyCompanyDatabase.Count.ToString() + "]";

        return count;
    }

    public int TextSearch(String text)
    {
        MySearchData.Clear();
        OnDropdownEvent(0);
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
        AllSubjectCountText.text = "[" + count.ToString() + "/" + MyCompanyDatabase.Count.ToString() + "]";
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
    public int DOTextSearch(String text)
    {
        MySearchData.Clear();
        OnDropdownEvent(0);
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
        AllSubjectCountText.text = "[" + count.ToString() + "/" + MyCompanyDatabase.Count.ToString() + "]";
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
    public int GetSubjectRemaining(int id)
    {
        int dex = 0;
        for(int i = 0; i <= id; i++)
        {
            if(MySearchData[id].SubjectName.Trim() == MySearchData[i].SubjectName.Trim())
            {
                MySearchData[i].Release.Replace(",", "");
                MySearchData[i].Receiving.Replace(",", "");
                dex += int.Parse(MySearchData[i].Release);
                dex -= int.Parse(MySearchData[i].Receiving);
            }
        }
        AllCount.text = MySearchData[0].SubjectName.Trim() + "의 잔고 : " + dex.ToString();

        return dex;
    }
    public string[] AllGetSearch(int id)
    {
        string[] jdata = { "오류", "오류", "오류", "오류", "오류", "오류", "오류" };

        jdata[0] = MyCompanyDatabase[id].Date;
        jdata[1] = MyCompanyDatabase[id].SubjectName;
        jdata[2] = MyCompanyDatabase[id].Release;
        jdata[3] = MyCompanyDatabase[id].Receiving;
        jdata[4] = MyCompanyDatabase[id].CompanyName;
        jdata[6] = MyCompanyDatabase[id].ReceivingTime;

        return jdata;
    }
    public string[] GetSearch(int id)// Date Name Rel Rece ComName Com
    {
        string[] jdata = { "오류", "오류", "오류", "오류", "오류", "오류", "오류" };

        jdata[0] = MySearchData[id].Date;
        jdata[1] = MySearchData[id].SubjectName;
        jdata[2] = MySearchData[id].Release;
        jdata[3] = MySearchData[id].Receiving;
        jdata[4] = MySearchData[id].CompanyName;
        jdata[6] = MySearchData[id].ReceivingTime;

        return jdata;
    }
    void Load()
    {
        MyCompanyDatabase.Clear();
        string jdata = File.ReadAllText(filePath);
        string[] line = jdata.Substring(0, jdata.Length).Split('\n');
        for (int i = 0; i < line.Length; i++)
        {
            string[] row = line[i].Split('\t');
            for (int j = 0; j < 6; j++)
            {
                if (row[j] == "")
                {
                    row[j] = "0";
                }

            }
            MyCompanyDatabase.Add(new CompanyList(row[0], row[1], row[2].Replace(",", ""), row[3].Replace(",", ""), row[4], row[5]));


        }
    }

    // 여기서 부터   
    bool SetIDPass()
    {
        SubjectName = SubjectInput.text.Trim();
        Release = ReleaseInput.text.Trim();
        Receiving = ReceivingInput.text.Trim();
        CompanyName = CompanyNameInput.text.Trim();

        if (SubjectName == "" || Release == "" || Receiving == "" || CompanyName == "") return false;
        else return true;
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

    IEnumerator Lookup()
    {
        UnityWebRequest www = UnityWebRequest.Get(ElectrolyteURL);

        yield return www.SendWebRequest();

        if (www.isDone)
            File.WriteAllText(filePath, www.downloadHandler.text);

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
        if (isCompanyName) {
            text1.text = "회사";
        }
        else
        {
            text1.text = "회수";
        }
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