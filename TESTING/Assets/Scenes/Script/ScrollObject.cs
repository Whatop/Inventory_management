using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    [Tooltip("자동완성 항목의 인덱스. ScrollViewEnom.UpdateScrollView에서 세팅한다.")]
    public int itemID = -1;

    public void ScrollButtonDown()
    {
        var list = GameManager.Instance != null ? GameManager.Instance.curData : null;
        if (list == null || list.Count == 0) return;
        if (itemID < 0 || itemID >= list.Count) return;

        GameManager.Instance.searchButtonDown(itemID);
    }

    // ★ OnEnable fallback 제거 (명시 세팅 전용으로 운용)
}
