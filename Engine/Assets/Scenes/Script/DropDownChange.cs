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

    public void OnDropdownEvent(int index) // �̷����ϸ� index�� �˾Ƽ� �ٲ�
    {
        //text.text = $"Dropdown Index : { index}";
    }
}
