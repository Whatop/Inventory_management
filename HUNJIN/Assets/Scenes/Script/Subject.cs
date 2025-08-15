using UnityEngine;
using UnityEngine.UI;

public class Subject : MonoBehaviour
{
    public Text SubjectName, SubjectDate, SubjectRelease, SubjectReceiving, Remaining, Gugo;
    public Image Bg, image;
    public Sprite[] Sprites;
    [SerializeField] private int myId;

    private void OnDisable()
    {
        myId = -99;
        ScrollViewController.ReturnToPool(gameObject);
    }

    private void OnEnable()
    {
        if (ScrollViewController.Instance != null && !ScrollViewController.Instance.dont)
        {
            if (myId == -99) myId = ScrollViewController.Instance.GetId();

            if (GameManager.Instance.MySearchData.Count != 0)
            {
                int searchCount;
                string[] s;

                if (GameManager.Instance.isSubject)
                {
                    image.gameObject.SetActive(true);
                    SubjectDate.gameObject.SetActive(true);
                    SubjectReceiving.gameObject.SetActive(true);
                    Remaining.gameObject.SetActive(true);
                    SubjectName.gameObject.SetActive(true);
                    Gugo.gameObject.SetActive(true);
                    SubjectReceiving.text = GameManager.Instance.GetSubjectRemaining(myId).ToString();
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
                    s = GameManager.Instance.GetSearch(searchCount - 1);
                }
                else
                {
                    searchCount = myId;
                    s = GameManager.Instance.GetSearch(searchCount);
                    Debug.Log(s);
                }

                SubjectDate.text = (s[0] ?? "").Trim().Replace("-", "/");
                SubjectName.text = (s[1] ?? "").Trim();
                Gugo.text = (s[7] ?? "").Trim();

                int release = int.TryParse((s[3] ?? "0").Trim(), out var r1) ? r1 : 0;
                int receiving = int.TryParse((s[2] ?? "0").Trim(), out var r2) ? r2 : 0;

                if (GameManager.Instance.curType == "Lost")
                {
                    SubjectDate.gameObject.SetActive(false);
                    if (receiving > 0) SetArrowInfo(Color.blue, "���", receiving.ToString());
                    else SetArrowInfo(Color.black, "���", "???");
                }
                else
                {
                    if (receiving > 0) SetArrowInfo(Color.blue, "�԰�", receiving.ToString());
                    else if (release > 0) SetArrowInfo(Color.red, "���", release.ToString());
                    else SetArrowInfo(Color.black, "���", "???");
                }

                if (GameManager.Instance.isSed)
                {
                    SubjectReceiving.gameObject.SetActive(false);
                    SubjectDate.gameObject.SetActive(false);
                    SetArrowInfo(Color.black, searchCount.ToString(), s[4]);
                }

                ShowAllSubjectCountText();
            }
            else
            {
                myId = ScrollViewController.Instance.GetId();
                var s = GameManager.Instance.DoGetSearch(myId);
                SubjectDate.text = (s[0] ?? "").Trim().Replace("-", "/");
                SubjectName.text = (s[1] ?? "").Trim();
                Gugo.text = (s[7] ?? "").Trim();
                // �ʿ�� ����/������ ���� ���� �߰� ����
            }
        }
    }

    private void SetArrowInfo(Color c, string label, string qty)
    {
        if (Sprites != null && Sprites.Length >= 2)
            image.sprite = Sprites[label == "���" ? 0 : 1];
        image.color = c;
        SubjectRelease.color = c;
        SubjectRelease.text = label;
        Remaining.text = qty;
        Remaining.color = c;
    }

    private void ShowAllSubjectCountText()
    {
        var gm = GameManager.Instance;
        if (gm == null) return;

        // ��� + �� �ƴ� �� AllCount�� �����, ������ ī��Ʈ�� �ʿ�� ����
        if (gm.isSed && !gm.isSubject)
        {
            if (gm.AllCount != null && gm.AllCount.Length > gm.curScene && gm.AllCount[gm.curScene] != null)
                gm.AllCount[gm.curScene].gameObject.SetActive(false);

            if (gm.AllSubjectCountText != null && gm.AllSubjectCountText.Length > gm.curScene && gm.AllSubjectCountText[gm.curScene] != null)
                gm.AllSubjectCountText[gm.curScene].gameObject.SetActive(true);
            return;
        }

        // ���� ����(��/��� �� ��Ȳ)
        gm.AllSubjectCountText[gm.curScene].gameObject.SetActive(true);
        gm.AllCount[gm.curScene].gameObject.SetActive(true);
    }


    public void SearchButton(Text text)
    {
        if (!GameManager.Instance.isSubject)
            ScrollViewController.Instance.ReTextSearch(text);
    }
}
