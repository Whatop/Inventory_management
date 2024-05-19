using UnityEngine;
using UnityEngine.UI;

public class Subject : MonoBehaviour
{
    public Text SubjectName;
    public Text SubjectDate;
    public Text SubjectRelease;
    public Text SubjectReceiving;
    public Text Remaining;
    public Text Gugo;
    public Image Bg;
    public Image image;
    public Sprite[] Sprites;
    [SerializeField] private int myId;

    //문제점 생성이 2번되서 200, 202가 안보임

    private void OnDisable()
    {
        myId = -99;
        ScrollViewController.ReturnToPool(gameObject);
    }

    private void OnEnable()
    {
        if (!ScrollViewController.Instance.dont)
        {
            if(myId == -99)
            myId = ScrollViewController.Instance.GetId(); // GetId() 함수를 한 번 호출하고 그 값을 myId 변수에 저장

            if (GameManager.Instance.MySearchData.Count != 0)
            {
                int searchCount = 0;
                string[] searchSubject;
                if (GameManager.Instance.isSubject)
                {
                    image.gameObject.SetActive(true);
                    SubjectDate.gameObject.SetActive(true);
                    SubjectReceiving.gameObject.SetActive(true);
                    Remaining.gameObject.SetActive(true);
                    SubjectName.gameObject.SetActive(true);
                    Gugo.gameObject.SetActive(true);
                }
                else
                {
                    image.gameObject.SetActive(true);
                    SubjectDate.gameObject.SetActive(true);
                    SubjectReceiving.gameObject.SetActive(false);
                    Remaining.gameObject.SetActive(true);
                    SubjectName.gameObject.SetActive(true);
                    Gugo.gameObject.SetActive(true);
                }
                if (GameManager.Instance.isSed)
                {
                    searchCount = myId + GameManager.Instance.Curpage * GameManager.Instance.PageObject + 1;
                    searchSubject = GameManager.Instance.GetSearch(searchCount - 1);
                }
                else
                {

                    searchCount = myId ;
                    searchSubject = GameManager.Instance.GetSearch(searchCount);
                }
                SubjectDate.text = searchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = searchSubject[1].Trim();
                Gugo.text = searchSubject[7].Trim();

                if (GameManager.Instance.isSubject)
                {
                    SubjectReceiving.text = GameManager.Instance.GetSubjectRemaining(myId).ToString();
                }
                if(GameManager.Instance.curType == "Lost")
                {
                    SubjectDate.gameObject.SetActive(false);
                    if (int.Parse(searchSubject[3].Trim()) > 0)
                    {
                        SetArrowInfo(Color.blue, "재고", searchSubject[3].Trim());
                    }
                    else
                    {
                        SetArrowInfo(Color.black, "대기", "???");
                    }
                }
                else
                {
                    if (int.Parse(searchSubject[3].Trim()) > 0)
                    {
                        SetArrowInfo(Color.blue, "출고", searchSubject[3].Trim());
                    }
                    else if (int.Parse(searchSubject[2].Trim()) > 0)
                    {
                        SetArrowInfo(Color.red, "입고", searchSubject[2].Trim());
                    }
                    else
                    {
                        SetArrowInfo(Color.black, "대기", "???");
                    }
                }

                if (GameManager.Instance.isSed)
                {
                    SubjectReceiving.gameObject.SetActive(false);
                    SubjectDate.gameObject.SetActive(false);
                    SetArrowInfo(Color.black, searchCount.ToString(), searchSubject[4]);
                }

                ShowAllSubjectCountText();
            }
            else
            {
                myId = ScrollViewController.Instance.GetId();
                string[] AllsearchSubject = GameManager.Instance.DoGetSearch(myId);
                SubjectDate.text = AllsearchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = AllsearchSubject[1].Trim();
                Gugo.text = AllsearchSubject[7].Trim();
                SetColorBasedOnDate();
            }
        }
    }

    private void SetArrowInfo(Color arrowColor, string releaseText, string remainingText)
    {
        image.sprite = Sprites[releaseText == "출고" ? 0 : 1];
        image.color = arrowColor;
        SubjectRelease.color = arrowColor;
        SubjectRelease.text = releaseText;
        Remaining.text = remainingText;
        Remaining.color = arrowColor;
    }

    private void SetColorBasedOnDate()
    {
        if (SubjectReceiving.text != "None")
        {
            if (int.Parse(SubjectDate.text) % 2 == 0)
            {
                image.color = new Color(0.9607844f, 0.3607843f, 0.1372549f, 1f); // carrot
                // SubjectName.color = new Color(0.9607844f, 0.3607843f, 0.1372549f, 1f);
            }
            else
            {
                image.color = Color.black;
                SubjectName.color = Color.black;
                Bg.color = Color.white;
            }
        }
    }

    private void ShowAllSubjectCountText()
    {
        GameManager.Instance.AllSubjectCountText[GameManager.Instance.curScene].gameObject.SetActive(true);
        GameManager.Instance.AllCount[GameManager.Instance.curScene].gameObject.SetActive(true);
    }

    public void SearchButton(Text text)
    {
        if (!GameManager.Instance.isSubject)
            ScrollViewController.Instance.ReTextSearch(text);
    }
}
