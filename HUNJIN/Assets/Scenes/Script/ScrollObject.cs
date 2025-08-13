using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    public int itemID;

    public void OnEnable()
    {
        if (ScrollViewController.Instance != null)
            itemID = ScrollViewController.Instance.GetEnId();
    }

    public void ScrollButtonDown()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.searchButtonDown(itemID);
        if (ScrollViewController.Instance != null)
            ScrollViewController.Instance.ResetEnId();
    }
}
