using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ScrollViewEnom : MonoBehaviour
{
    public GameObject prefab; // 프리팹을 Inspector에서 연결합니다.
    public Transform content; // 스크롤뷰의 Content Transform을 Inspector에서 연결합니다.
    public List<string> MyNames; // 목록을 Inspector에서 연결하거나 스크립트를 통해 업데이트합니다.

    private List<GameObject> instantiatedObjects = new List<GameObject>();

    // 목록이 변경될 때마다 호출할 함수
    public void UpdateScrollView()
    {
        MyNames = GameManager.Instance.curData;
        // 기존에 생성된 프리팹들을 삭제
        foreach (var obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();

        // MyNames 목록에 따라 프리팹 생성
        float yPos = 136;
        for (int i = 0; i < MyNames.Count; i++)
        {
            GameObject newObj = Instantiate(prefab, content);
            TextMeshProUGUI text = newObj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = MyNames[i];

            // 프리팹의 RectTransform을 사용하여 위치를 조절
            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, yPos);

            yPos -= 21.3f; // 프리팹 간격을 프리팹의 높이로 설정

            instantiatedObjects.Add(newObj);
        }
    }
}
