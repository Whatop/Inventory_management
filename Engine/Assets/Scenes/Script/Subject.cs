using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subject : MonoBehaviour
{
    public Text SubjectName;
    public Text SubjectDate;
    public Text SubjectRelease;
    public Text SubjectReceiving;
    public Text Remaining;

    public Sprite[] Sprites;
    [SerializeField]
    private int myId;


    bool First = false;


    private void OnDisable()
    {
        ScrollViewController.Instance.ResetId();
        if (!First)
            myId = ScrollViewController.Instance.GetId();

        ScrollViewController.ReturnToPool(gameObject);
        First = true;

    }
    private void OnEnable()
    {
        if (!ScrollViewController.Instance.dont)
        {
            if (GameManager.Instance.MySearchData.Count != 0)
            {
                myId = ScrollViewController.Instance.GetId();
                string[] searchSubject = GameManager.Instance.GetSearch(myId);
                SubjectDate.text = searchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = searchSubject[1].Trim();
                SubjectRelease.text = searchSubject[3].Trim();
                SubjectReceiving.text = searchSubject[2].Trim();
                if (GameManager.Instance.CompanyType == 0)
                {

                    Remaining.text = GameManager.Instance.GetSubjectRemaining(myId).ToString();
                    //if (GameManager.Instance.isCompanyName)
                    //{
                    //    Remaining.text = "";
                    //}
                }
            }
            else
            {
                if (First)
                    myId = ScrollViewController.Instance.GetId();
                string[] AllsearchSubject = GameManager.Instance.AllGetSearch(myId);
                SubjectDate.text = AllsearchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = AllsearchSubject[1].Trim();
                SubjectRelease.text = AllsearchSubject[3].Trim();
                SubjectReceiving.text = AllsearchSubject[2].Trim();
                Remaining.text = AllsearchSubject[4].Trim();
                //if (GameManager.Instance.isCompanyName)
                //{
                //    Remaining.text = "";
                //}
            }
        }
    }

        
        void DeactiveDelay() => gameObject.SetActive(false);
    public void SearchButton(Text text)
    {
        ScrollViewController.Instance.ReTextSearch(text);
    }
}
