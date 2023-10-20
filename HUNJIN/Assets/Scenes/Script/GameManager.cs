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

    //��� �̵�(++��¥ , ȸ���, �ð�,

    public string Date, SubjectName = "None", Receiving = "0", Release = "0", CompanyName = "None", ReceivingTime = "";
}



public class GameManager : MonoBehaviour
{
    // �׳� �˻��� �޼����� �ִ� ������ ���� ó���ϱ�
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

    string ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&range=K2:P";//���ؾ�ü
    string CodeURL = "https://script.google.com/macros/s/AKfycbyTjQ7vaLoT1KB5XV6fQah_GohTipaadENBkjuAQi0FYN0XFmgqJocwOO5BvWV2aRMGrQ/exec";//�ڵ�

    public InputField DateInput, TimeInput, SubjectInput, ReleaseInput, ReceivingInput, CompanyNameInput, NoneInput;
    string Date, SubjectName, Release, Receiving, CompanyName, ReceivingTime, None;

    DateTime _dateTime;
    public int CompanyType; /// �ǵ�� �ڵ� ���������� ����
    public int CompanySearch;
    public TextMeshProUGUI AllSubjectCountText;
    private bool subfirst = true;

    public TextMeshProUGUI AllCount;
    public GameObject[] CheckBoxs;
    [SerializeField]
    private Dropdown dropdown;

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
    public void UpSeachCount()
    {
        SeachIndex++;
        TabClick("Subject");
    }

    public void DownSeachCount()
    {
        if (SeachIndex > 1)
        {
            SeachIndex--;
            TabClick("Subject");
        }
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
    public void OnDropdownEvent(int index) // �̷����ϸ� index�� �˾Ƽ� �ٲ�
    {
        dropdown.value = index;
        CompanySearch = index;
        // ��ü,����,õ��,��â,û��,�ϳ�,j&f,�񿤾���,��Ư

    }
    public void ResetData()
    {
        DateInput.text = DateTime.Now.ToString("yy/M/d");
        TimeInput.text = DateTime.Now.ToString("HH:mm");
        AllSubjectCountText.text = "[" + GetSeachResult() + "/" + DoSearchData.Count.ToString() + "]";
        SubjectInput.text = "";
        ReleaseInput.text = "";
        ReceivingInput.text = "";
        CompanyNameInput.text = "";
        NoneInput.text = "";
        AllCount.text = "�ܰ�";
        if (Time.frameCount % 30 == 0)
        {
            System.GC.Collect(); // û���ڵ� �ΰ��� ���̾ƴ϶� �ε����ϋ� 
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
                case "Main": tabNum = 0; break;
                case "Subject": tabNum = 1; ; break;
                case "Enrollment": tabNum = 2; break;
                case "None": tabNum = 3; break;
                case "ReSubject": tabNum = 1; break;
                case "EndProduct": tabNum = 1; break;
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

        if(tabName == "Subject")
        {
            Image1.color = new Color(0.4273228f, 0.409434f, 1f); //OUT
            Image2.color = new Color(0.5273228f, 0.409434f, 1f); // IN
            text1.text = "����� ����";
        }
        else if(tabName == "ReSubject")
        {
            Image1.color = new Color(0.03588462f, 0.5943396f, 0.04296107f); // OUT
            Image2.color = new Color(0.3629665f, 0.8773585f, 0.1522962f);  // IN
            text1.text = "������ ����";
        } 
        else if(tabName == "EndProduct")
        {
            Image1.color = new Color(0.9150943f, 0.2495712f, 0f); //OUT
            Image2.color = new Color(0.9607844f, 0.3607843f, 0.1372549f); // IN
            text1.text = "����ǰ ����";
        }
        if (curType != "Enrollment")
             StartCoroutine(Lookup(curType));
        
    }
    // ��ü,����,õ��,��â,û��

    public int ProductSearch()//
    {
        MySearchData.Clear();

        int count = 0;
            for (int i = 0; i < MyCompanyDatabase.Count; i++)
            {
                if (MyCompanyDatabase[i].SubjectName.Trim().ToLower().Contains(SubjectNameSearch.text.Trim().ToLower()))
                {
                    if (CompanySearch == 0)
                    {
                        MySearchData.Add(MyCompanyDatabase[i]);
                        count++;
                    }
                    
                    else if(CompanySearch == 1 && MyCompanyDatabase[i].CompanyName == "����")
                        {
                            MySearchData.Add(MyCompanyDatabase[i]);
                            count++;
                        }
                        else if(CompanySearch == 2 && MyCompanyDatabase[i].CompanyName == "õ��")
                        {
                            MySearchData.Add(MyCompanyDatabase[i]);
                            count++;
                        }
                        else if (CompanySearch == 3 && MyCompanyDatabase[i].CompanyName == "��â")
                        {
                            MySearchData.Add(MyCompanyDatabase[i]);
                            count++;
                        }
                        else if (CompanySearch == 4 && MyCompanyDatabase[i].CompanyName == "û��")
                        {
                            MySearchData.Add(MyCompanyDatabase[i]);
                            count++;
                        }
                        else if (CompanySearch == 5 && MyCompanyDatabase[i].CompanyName == "�ϳ�")
                        {
                            MySearchData.Add(MyCompanyDatabase[i]);
                            count++;
                        }
                }
        }
        AllSubjectCountText.text = "[" + count.ToString() + "/" + MyCompanyDatabase.Count.ToString() + "]";

        return count;
    }
    public int SEadSearch()//���˻�
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
    {
        var distPerson = MyCompanyDatabase.Select(person => new { person.SubjectName}).Distinct().ToList();
        DoSearchData.Clear();
        OnDropdownEvent(0);
        int count = 0;
        foreach (var obj in distPerson)
        {
            DoSearchData.Add(new CompanyList(count.ToString(), obj.SubjectName, "1", "1", "1", "1"));
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
                AllCount.text = MySearchData[0].SubjectName.Trim() + " - ȸ�� ���� : " + dex.ToString();
            }
            else if (dex > 0)
            {
                AllCount.text = MySearchData[0].SubjectName.Trim() + " - ȸ�� �ʰ� : " + dex.ToString();
            }
            else
            {
                AllCount.text = " - ȸ���� ��� ���ſϷ� : " + dex.ToString();
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
                MyCompanyDatabase.Add(new CompanyList(row[0].Replace("-","/"), row[1].Replace(" ", ""), row[2].Replace(",", ""), row[3].Replace(",", ""), row[4], "None"));
            }

        }
        ResetData();
        ScrollViewController.Instance.DoSearch();
    }

    // ���⼭ ����   
    bool SetIDPass()
    {
        SubjectName = SubjectInput.text.Trim();
        Release = ReleaseInput.text.Trim();
        Receiving = ReceivingInput.text.Trim();
        CompanyName = CompanyNameInput.text.Trim();
        ReceivingTime = TimeInput.text.Trim();
        Date = DateInput.text.Trim();
        None = NoneInput.text.Trim();

        if (SubjectName == "" || Release == "" || Receiving == "" || CompanyName == "" && TimeInput.text.Length >= 4) return false;
        else return true;
    }
    public void Register() //���
    {
        if (!SetIDPass())
        {
            print("�߸��� �Է��Դϴ�.");
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
        using (UnityWebRequest www = UnityWebRequest.Post(CodeURL, form)) // �ݵ�� using�� ����Ѵ�
        {
            yield return www.SendWebRequest();

            if (www.isDone) print(www.downloadHandler.text);
            else print("���� ������ �����ϴ�.");
        }
        Load();
    }

    IEnumerator Lookup(string curType)
    {
        if (curType == "Subject")
            ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&range=K2:P";
        else if (curType == "ReSubject")
            ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=1973018837&range=K2:P";
        else
            ElectrolyteURL = "https://docs.google.com/spreadsheets/d/1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM/export?format=tsv&gid=1809169708&range=K2:P";

        UnityWebRequest www = UnityWebRequest.Get(ElectrolyteURL);

        yield return www.SendWebRequest();

        if (www.isDone)
        {
            File.WriteAllText(filePath, www.downloadHandler.text);
            Load();
            //animator.SetTrigger("End");
        }

        else print("���� ������ �����ϴ�.");
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