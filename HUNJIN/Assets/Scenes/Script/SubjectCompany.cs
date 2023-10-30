using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubjectCompany : MonoBehaviour
{
    public Text SubjectDate;
    public Text SubjectName;
    public Text SubjectPrev;
    public Text SubjectCur;
    public Text SubjectCompanyName;
    public Text Company;

    [SerializeField]
    private int myId;

    bool First = false;

    private void OnDisable()
    {
        if (!First)
            myId = ScrollViewController.Instance.GetId();

        ScrollViewController.ReturnToPool(gameObject);
        First = true;
    }
    private void OnEnable()
    {
        if (First)
        {
            if (GameManager.Instance.MySearchData.Count != 0)
            {
                myId = ScrollViewController.Instance.GetId();
                string[] searchSubject = GameManager.Instance.GetSearch(myId);
                SubjectName.text = searchSubject[1].Trim();
                //SubjectPrev.text = searchSubject[1].Trim();
                SubjectCur.text = searchSubject[0].Trim();
                Company.text = searchSubject[4].Trim();
            }
            else
            {
                string[] AllsearchSubject = GameManager.Instance.AllGetSearch(myId);
                SubjectName.text = AllsearchSubject[1].Trim();
                //SubjectPrev.text = AllsearchSubject[1].Trim();
                SubjectCur.text = AllsearchSubject[0].Trim();
                Company.text = AllsearchSubject[4].Trim();
            }
        }
    }
    void DeactiveDelay() => gameObject.SetActive(false);

}
