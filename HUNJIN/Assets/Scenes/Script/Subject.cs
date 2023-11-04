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

    //������ ������ 2���Ǽ� 200, 202�� �Ⱥ���

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
            myId = ScrollViewController.Instance.GetId(); // GetId() �Լ��� �� �� ȣ���ϰ� �� ���� myId ������ ����

            if (GameManager.Instance.MySearchData.Count != 0)
            {
                if (GameManager.Instance.isSubject)
                {
                    image.gameObject.SetActive(true);
                    SubjectDate.gameObject.SetActive(true);
                    SubjectReceiving.gameObject.SetActive(true);
                    Remaining.gameObject.SetActive(true);
                    SubjectName.gameObject.SetActive(true);
                }
                else
                {
                    image.gameObject.SetActive(true);
                    SubjectDate.gameObject.SetActive(true);
                    SubjectReceiving.gameObject.SetActive(false);
                    Remaining.gameObject.SetActive(true);
                    SubjectName.gameObject.SetActive(true);
                }

                string[] searchSubject = GameManager.Instance.GetSearch(myId);

                SubjectDate.text = searchSubject[0].Trim().Replace("-", "/");
                SubjectName.text = searchSubject[1].Trim();

                if (GameManager.Instance.isSubject)
                {
                    SubjectReceiving.text = GameManager.Instance.GetSubjectRemaining(myId).ToString();
                }
                if(GameManager.Instance.curType == "Lost")
                {
                    if (int.Parse(searchSubject[3].Trim()) > 0)
                    {
                        SetArrowInfo(Color.blue, "���", searchSubject[3].Trim());
                    }
                    else
                    {
                        SetArrowInfo(Color.black, "���", "???");
                    }
                }
                else
                {
                    if (int.Parse(searchSubject[3].Trim()) > 0)
                    {
                        SetArrowInfo(Color.blue, "���", searchSubject[3].Trim());
                    }
                    else if (int.Parse(searchSubject[2].Trim()) > 0)
                    {
                        SetArrowInfo(Color.red, "�԰�", searchSubject[2].Trim());
                    }
                    else
                    {
                        SetArrowInfo(Color.black, "���", "???");
                    }
                }

                if (GameManager.Instance.isSed)
                {
                    SubjectReceiving.gameObject.SetActive(false);
                    SubjectDate.gameObject.SetActive(false);
                    SetArrowInfo(Color.black, myId.ToString(), "ȸ��");
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
        image.sprite = Sprites[releaseText == "���" ? 0 : 1];
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
