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

    //  1 : UP ��� �Ķ�
    //  2 : DOWN �԰� ����
    public Image image;
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
               // SubjectRelease.text = searchSubject[3].Trim();
               // SubjectReceiving.text = searchSubject[2].Trim();
                if (GameManager.Instance.CompanyType == 0)
                {

                    // 
                    GameManager.Instance.GetSubjectRemaining(myId).ToString();
                    //if (GameManager.Instance.isCompanyName)
                    //{
                    //    Remaining.text = "";
                    //}
                }
                if (int.Parse(searchSubject[3].Trim()) > 0)
                {
                    image.sprite = Sprites[0];
                    image.color = Color.blue;
                    SubjectRelease.color = Color.blue;
                    SubjectRelease.text = "���";
                    Remaining.text = searchSubject[3].Trim();
                    Remaining.color = Color.blue;

                }
                if (int.Parse(searchSubject[2].Trim()) > 0)
                {
                    image.sprite = Sprites[1];
                    image.color = Color.red;
                    SubjectRelease.color = Color.red;
                    SubjectRelease.text = "�԰�";
                    Remaining.text = searchSubject[2].Trim();
                    Remaining.color = Color.red; 
                }
            }
            else
            {
              
                if (First)
                    myId = ScrollViewController.Instance.GetId();
                string[] AllsearchSubject = GameManager.Instance.AllGetSearch(myId);
                SubjectDate.text = AllsearchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = AllsearchSubject[1].Trim();
                //SubjectRelease.text = AllsearchSubject[3].Trim(); // ��� 
                //SubjectReceiving.text = AllsearchSubject[2].Trim(); // �԰�
                //Remaining.text = AllsearchSubject[4].Trim();

                //  0 : UP ��� �Ķ�
                //  1 : DOWN �԰� ����

                if (int.Parse(AllsearchSubject[3].Trim()) > 0)
                {
                    image.sprite = Sprites[0];
                    image.color = Color.blue;
                    SubjectRelease.color = Color.blue;
                    SubjectRelease.text = "���";
                    Remaining.text = AllsearchSubject[3].Trim();
                    Remaining.color = Color.blue;

                }
                else if (int.Parse(AllsearchSubject[2].Trim()) > 0)
                {
                    image.sprite = Sprites[1];
                    image.color = Color.red;
                    SubjectRelease.color = Color.red;
                    SubjectRelease.text = "�԰�";
                    Remaining.text = AllsearchSubject[2].Trim();
                    Remaining.color = Color.red;
                }
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
