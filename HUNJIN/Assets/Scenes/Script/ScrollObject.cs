using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ScrollObject : MonoBehaviour
{
    public int itemID; // 아이템의 고유한 ID

    public void Start()
    {
        if (ScrollViewController.Instance != null)
        {
            itemID = ScrollViewController.Instance.GetEnId();
            // 다른 초기화 작업 수행
        }
    }

    public void ScrollButtonDown()
    {
        GameManager.Instance.searchButtonDown(itemID);
    }

}
