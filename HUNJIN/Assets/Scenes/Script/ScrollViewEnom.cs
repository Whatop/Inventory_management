using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class ScrollViewEnom : MonoBehaviour
{
    public GameObject prefab;
    public Transform content;
    public List<string> MyNames;

    private readonly List<GameObject> instantiated = new List<GameObject>();


    public void UpdateScrollView()
    {
        MyNames = GameManager.Instance?.curData ?? new List<string>();

        foreach (var obj in instantiated) Destroy(obj);
        instantiated.Clear();

        float yPos = 136f;
        for (int i = 0; i < MyNames.Count; i++)
        {
            var go = Instantiate(prefab, content);
            var label = go.GetComponentInChildren<TextMeshProUGUI>(true);
            if (label != null) label.text = MyNames[i];

            var rt = go.GetComponent<RectTransform>();
            if (rt != null) rt.anchoredPosition = new Vector2(0, yPos);
            yPos -= 21.3f;

            instantiated.Add(go);
        }
    }
}
