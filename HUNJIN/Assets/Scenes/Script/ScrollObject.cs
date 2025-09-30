using UnityEngine;

public class ScrollObject : MonoBehaviour
{
    [Tooltip("자동완성 항목의 인덱스. ScrollViewEnom.UpdateScrollView에서 세팅한다.")]
    public int itemID = -1;

    /// <summary>
    /// 버튼 클릭 시 호출. 인덱스 범위 체크 후 GameManager에 전달.
    /// </summary>
    public void ScrollButtonDown()
    {
        // 현재 데이터 목록 가져오기
        var list = GameManager.Instance != null ? GameManager.Instance.curData : null;
        if (list == null || list.Count == 0) return;

        // 안전한 범위 체크
        if (itemID < 0 || itemID >= list.Count) return;

        // 실제 검색/선택 로직 호출
        GameManager.Instance.searchButtonDown(itemID);
    }

    // 만약 기존에 OnEnable에서 GetEnId()로 세팅하던 프로젝트라면,
    // 아래 코드를 남겨 호환할 수 있다. (itemID가 세팅되지 않은 경우에만)
    private void OnEnable()
    {
        if (itemID < 0 && ScrollViewController.Instance != null)
            itemID = ScrollViewController.Instance.GetEnId();
    }
}
