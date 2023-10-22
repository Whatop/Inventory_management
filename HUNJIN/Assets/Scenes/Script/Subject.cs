using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Subject : MonoBehaviour
{
    public Text SubjectName;
    public Text SubjectDate; // ����
    public Text SubjectRelease;
    public Text SubjectReceiving; // ����
    public Text Remaining;

    //  1 : UP ��� �Ķ�
    //  2 : DOWN �԰� ����
    public Image Bg; // Bg
    public Image image; // ȭ��ǥ
    public Sprite[] Sprites;
    [SerializeField]
    private int myId;


    bool First = false;
    bool s = false;

    private void OnDisable()
    {
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
                if (GameManager.Instance.isSubject) //�˻���
                {
                    image.gameObject.SetActive(true);
                    SubjectDate.gameObject.SetActive(true);
                    SubjectReceiving.gameObject.SetActive(true);
                }
                else //����
                {
                    SubjectReceiving.gameObject.SetActive(false);
                }
                myId = ScrollViewController.Instance.GetId();
                string[] searchSubject = GameManager.Instance.GetSearch(myId);
                SubjectDate.text = searchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = searchSubject[1].Trim();


                if (GameManager.Instance.isSubject)
                {
                    SubjectReceiving.text = GameManager.Instance.GetSubjectRemaining(myId).ToString();
                }
                //ȭ��ǥ
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
                else
                {
                    SubjectName.color = Color.black;
                }
                GameManager.Instance.AllSubjectCountText.gameObject.SetActive(true);
                GameManager.Instance.AllCount.gameObject.SetActive(true);
            }
            else // --------------------- AllSearch --------------------
            {
                if(First)
                myId = ScrollViewController.Instance.GetId();
                //ü���� �׽�Ʈ ;;
                string[] AllsearchSubject = GameManager.Instance.DoGetSearch(myId);
                SubjectDate.text = AllsearchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = AllsearchSubject[1].Trim();


                // COLOR
                // Date ������ 2 % 0 ture = white , false = blue
                // image, Name -> change : white or black
                if (SubjectReceiving.text != "None")
                {
                    if (0 == int.Parse(SubjectDate.text) % 2)
                    {
                        //image.color = Color.white;
                        //SubjectName.color = Color.white;
                        // Bg.color = new Color(0.4273228f, 0.409434f, 1f, 1f); //blue
                        //Bg.color = new Color(0.9607844f, 0.3607843f, 0.1372549f, 0.5f); // carrot
                        image.color = new Color(0.9607844f, 0.3607843f, 0.1372549f, 1f); // carrot
                        //SubjectName.color = new Color(0.9607844f, 0.3607843f, 0.1372549f, 1f);
                    }
                    else
                    {
                        image.color = Color.black;
                        SubjectName.color = Color.black;
                        Bg.color = Color.white;

                    }
                }

            }
        }
    }


    void DeactiveDelay() => gameObject.SetActive(false);
    public void SearchButton(Text text)
    {
        if (!GameManager.Instance.isSubject)
            ScrollViewController.Instance.ReTextSearch(text);
    }
}
