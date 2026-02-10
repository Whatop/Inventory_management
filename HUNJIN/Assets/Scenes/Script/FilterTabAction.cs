using UnityEngine;
using UnityEngine.UI;

public interface IFilterTabController
{
    void OnFilterTabChanged(string code, bool isOn, Toggle source);
}

[RequireComponent(typeof(Toggle))]
public class FilterTabAction : MonoBehaviour
{
    [Header("Target")]
    public MonoBehaviour controller; // DeliveryListPanelController

    [Tooltip("예: \"ALL\" / CREATED / ASSIGNED / ACCEPTED / PICKED_UP / DELIVERED / CANCELED")]
    public string statusCode = "ALL";

    [Header("Options")]
    public bool allowKoreanLabel = true;

    private Toggle _toggle;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnChanged);
    }

    void OnDestroy()
    {
        if (_toggle != null)
            _toggle.onValueChanged.RemoveListener(OnChanged);
    }

    private void OnChanged(bool isOn)
    {
        if (!controller) return;

        var target = controller as IFilterTabController;
        if (target == null)
        {
            Debug.LogError($"[FilterTabAction] controller에 IFilterTabController 구현이 필요함: {controller.name}");
            return;
        }

        string code = (statusCode ?? "").Trim();
        if (allowKoreanLabel) code = NormalizeStatus(code);

        target.OnFilterTabChanged(code, isOn, _toggle);
    }

    private static string NormalizeStatus(string s)
    {
        if (string.IsNullOrEmpty(s)) return "ALL";

        switch (s)
        {
            case "전체":
            case "ALL": return "ALL";
            case "등록됨": return "CREATED";
            case "배차됨":
            case "배차완료": return "ASSIGNED";
            case "수락됨":
            case "수락완료": return "ACCEPTED";
            case "픽업완료": return "PICKED_UP";
            case "배달완료": return "DELIVERED";
            case "취소":
            case "취소됨": return "CANCELED";
            default: return s;
        }
    }
}
