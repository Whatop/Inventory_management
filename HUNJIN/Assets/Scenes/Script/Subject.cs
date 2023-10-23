
using UnityEngine;
using UnityEngine.UI;

public class Subject : MonoBehaviour
{
    public Text SubjectName;
    public Text SubjectDate;
    public Text SubjectRelease;
    public Text SubjectReceiving;
    public Text Remaining;
    public Image Bg;
    public Image image;
    public Sprite[] Sprites;
    [SerializeField] private int myId;
    private bool First = false;

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
                if (GameManager.Instance.isSubject)
                {
                    image.gameObject.SetActive(true);
                    SubjectDate.gameObject.SetActive(true);
                    SubjectReceiving.gameObject.SetActive(true);
                }
                else
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
                    SetArrowInfo(Color.black, "", "");
                }

                ShowAllSubjectCountText();
            }
            else
            {
                myId = ScrollViewController.Instance.GetId();
                string[] AllsearchSubject = GameManager.Instance.DoGetSearch(myId);
                SubjectDate.text = AllsearchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = AllsearchSubject[1].Trim();
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
        GameManager.Instance.AllSubjectCountText.gameObject.SetActive(true);
        GameManager.Instance.AllCount.gameObject.SetActive(true);
    }

    public void SearchButton(Text text)
    {
        if (!GameManager.Instance.isSubject)
            ScrollViewController.Instance.ReTextSearch(text);
    }
}
