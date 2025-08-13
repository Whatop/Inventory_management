using UnityEngine;
using UnityEngine.UI;

public class DropDownChange : MonoBehaviour
{
    [SerializeField] private Dropdown dropdown;
    [SerializeField] private Text text;

    void Awake()
    {
        if (dropdown == null) dropdown = GetComponent<Dropdown>();
        if (dropdown != null)
            dropdown.onValueChanged.AddListener(OnDropdownEvent);
    }

    public void OnDropdownEvent(int index)
    {
        // �ʿ�� UI ����
        // if (text != null) text.text = $"Dropdown Index : {index}";
    }
}
