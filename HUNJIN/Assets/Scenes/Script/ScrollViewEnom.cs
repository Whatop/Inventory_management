using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ScrollViewEnom : MonoBehaviour
{
    public GameObject prefab; // �������� Inspector���� �����մϴ�.
    public Transform content; // ��ũ�Ѻ��� Content Transform�� Inspector���� �����մϴ�.
    public List<string> MyNames; // ����� Inspector���� �����ϰų� ��ũ��Ʈ�� ���� ������Ʈ�մϴ�.

    private List<GameObject> instantiatedObjects = new List<GameObject>();

    // ����� ����� ������ ȣ���� �Լ�
    public void UpdateScrollView()
    {
        MyNames = GameManager.Instance.curData;
        // ������ ������ �����յ��� ����
        foreach (var obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();

        // MyNames ��Ͽ� ���� ������ ����
        float yPos = 136;
        for (int i = 0; i < MyNames.Count; i++)
        {
            GameObject newObj = Instantiate(prefab, content);
            TextMeshProUGUI text = newObj.GetComponentInChildren<TextMeshProUGUI>();
            text.text = MyNames[i];

            // �������� RectTransform�� ����Ͽ� ��ġ�� ����
            RectTransform rectTransform = newObj.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(0, yPos);

            yPos -= 21.3f; // ������ ������ �������� ���̷� ����

            instantiatedObjects.Add(newObj);
        }
    }
}
