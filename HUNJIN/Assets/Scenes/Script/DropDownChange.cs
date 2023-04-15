using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownChange : MonoBehaviour
{
    [SerializeField]
    private Dropdown dropdown;
    [SerializeField]
    private Text text;

    void Awake()
    {
        dropdown.onValueChanged.AddListener(OnDropdownEvent);
    }

    public void OnDropdownEvent(int index) // 이렇게하면 index가 알아서 바뀜
    {
        //text.text = $"Dropdown Index : { index}";
    }
}
