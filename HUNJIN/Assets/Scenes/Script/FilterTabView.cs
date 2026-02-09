using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class FilterTabView : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Image background;
    [SerializeField] private TMP_Text label;

    [Header("Colors")]
    [SerializeField] private Color onBg = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color offBg = new Color(1f, 1f, 1f, 0.15f);
    [SerializeField] private Color onText = new Color(0.1f, 0.1f, 0.1f, 1f);
    [SerializeField] private Color offText = new Color(1f, 1f, 1f, 0.85f);

    private Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();

        // 자동 탐색(필요하면 인스펙터로 직접 지정해도 됨)
        if (!background) background = GetComponentInChildren<Image>(true);
        if (!label) label = GetComponentInChildren<TMP_Text>(true);

        _toggle.onValueChanged.AddListener(ApplyVisual);
        ApplyVisual(_toggle.isOn);
    }

    void OnDestroy()
    {
        if (_toggle != null)
            _toggle.onValueChanged.RemoveListener(ApplyVisual);
    }

    private void ApplyVisual(bool isOn)
    {
        if (background) background.color = isOn ? onBg : offBg;
        if (label) label.color = isOn ? onText : offText;
    }
}
