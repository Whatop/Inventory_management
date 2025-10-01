using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ScrollViewEnom : MonoBehaviour
{
    public GameObject prefab;
    public Transform content;
    public List<string> MyNames;

    private readonly List<GameObject> instantiated = new List<GameObject>();

    // display limit
    const int MAX_ITEMS = 30;

    // layout params
    const float ItemH = 21.33f;
    const float TopPadding = 0f;
    const float BottomPadding = 8f;

    public void UpdateScrollView()
    {
        MyNames = GameManager.Instance?.curData ?? new List<string>();

        // clear old
        foreach (var obj in instantiated) Destroy(obj);
        instantiated.Clear();

        var contentRT = content as RectTransform;
        // normalize content anchors/pivot (top-left)
        contentRT.pivot = new Vector2(0f, 1f);
        contentRT.anchorMin = new Vector2(0f, 1f);
        contentRT.anchorMax = new Vector2(1f, 1f);

        // limit how many items we instantiate
        int displayCount = Mathf.Min(MyNames.Count, MAX_ITEMS);

        for (int i = 0; i < displayCount; i++)
        {
            var go = Instantiate(prefab, content);
            var so = go.GetComponent<ScrollObject>();
            so.itemID = i;
            go.SetActive(true); // 비활성 프리팹이라면 여기서 활성화
            var label = go.GetComponentInChildren<TextMeshProUGUI>(true);
            if (label != null) label.text = MyNames[i];

            var rt = go.GetComponent<RectTransform>();
            if (rt != null)
            {
                // child to top-left + stretch horizontally
                rt.pivot = new Vector2(0f, 1f);
                rt.anchorMin = new Vector2(0f, 1f);
                rt.anchorMax = new Vector2(1f, 1f);

                // remove horizontal margins
                rt.offsetMin = new Vector2(0f, rt.offsetMin.y);
                rt.offsetMax = new Vector2(0f, rt.offsetMax.y);

                // fixed height
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, ItemH);

                // place from top to bottom (negative y)
                float y = -(TopPadding + i * ItemH);
                rt.anchoredPosition = new Vector2(0f, y);
            }

            instantiated.Add(go);
        }

        // content height only for displayed items
        float totalH = TopPadding + (displayCount * ItemH) + BottomPadding;
        contentRT.sizeDelta = new Vector2(contentRT.sizeDelta.x, totalH);
    }
}
