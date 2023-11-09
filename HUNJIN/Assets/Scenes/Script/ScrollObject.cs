using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class ScrollObject : MonoBehaviour
{
    public int itemID; // �������� ������ ID

    public void Start()
    {
        if (ScrollViewController.Instance != null)
        {
            itemID = ScrollViewController.Instance.GetEnId();
            // �ٸ� �ʱ�ȭ �۾� ����
        }
    }

    public void ScrollButtonDown()
    {
        GameManager.Instance.searchButtonDown(itemID);
    }

}
