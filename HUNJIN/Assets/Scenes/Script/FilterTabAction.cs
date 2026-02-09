using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class FilterTabAction : MonoBehaviour
{
    [Header("Target")]
    public DeliveryListPanelController controller;

    [Tooltip("예: ALL / CREATED / ACCEPTED / PICKED_UP / DELIVERED / CANCELED")]
    public string statusCode = "ALL";

    [Header("Options")]
    [Tooltip("한글 라벨(예: 등록됨/수락완료/픽업완료/배달완료/취소됨)로 넣어도 영문 코드로 변환")]
    public bool allowKoreanLabel = true;

    private Toggle _toggle;
    private bool _wired;

    void Awake()
    {
        _toggle = GetComponent<Toggle>();
        if (!_wired)
        {
            _toggle.onValueChanged.AddListener(OnChanged);
            _wired = true;
        }
    }

    void OnDestroy()
    {
        if (_toggle != null && _wired)
        {
            _toggle.onValueChanged.RemoveListener(OnChanged);
            _wired = false;
        }
    }

    private void OnChanged(bool isOn)
    {
        if (!controller) return;

        string code = (statusCode ?? "").Trim();
        if (allowKoreanLabel) code = NormalizeStatus(code);

        // ✅ ON/OFF 둘 다 전달해야 "전체 ON일 때 누르면 전체 OFF" 같은 동작을 구현 가능
        controller.OnFilterTabChanged(code, isOn, _toggle);
    }

    private static string NormalizeStatus(string s)
    {
        if (string.IsNullOrEmpty(s)) return "ALL";

        switch (s)
        {
            case "전체":
            case "ALL":
                return "ALL";

            case "등록됨":
                return "CREATED";

            case "수락완료":
                return "ACCEPTED";

            case "픽업완료":
                return "PICKED_UP";

            case "배달완료":
                return "DELIVERED";

            case "취소":
            case "취소됨":
                return "CANCELED";

            default:
                return s; // 이미 코드면 그대로
        }
    }
}
