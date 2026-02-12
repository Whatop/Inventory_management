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
        // 필요시 UI 갱신
        // if (text != null) text.text = $"Dropdown Index : {index}";
    }
}
