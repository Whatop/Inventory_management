using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanelController : MonoBehaviour
{
    [Header("UI")]
    public GameObject root;          // ChoicePanel (없으면 this.gameObject 사용)
    public TMP_Text messageText;     // BG/Text(TMP)
    public Button yesButton;         // Yes
    public Button noButton;          // NO

    [Header("Labels (Optional)")]
    public TMP_Text yesLabel;        // Yes/Text(TMP)
    public TMP_Text noLabel;         // NO/Text(TMP)

    Action _onYes;
    Action _onNo;

    void Awake()
    {
        if (!root) root = gameObject;

        if (yesButton) yesButton.onClick.AddListener(ClickYes);
        if (noButton) noButton.onClick.AddListener(ClickNo);

        HideImmediate();
    }

    public void Show(string message, Action onYes, Action onNo = null, string yesText = "예", string noText = "아니오")
    {
        _onYes = onYes;
        _onNo = onNo;

        if (messageText) messageText.text = message ?? "";

        if (yesLabel) yesLabel.text = yesText ?? "예";
        if (noLabel) noLabel.text = noText ?? "아니오";

        root.SetActive(true);
    }

    public void HideImmediate()
    {
        _onYes = null;
        _onNo = null;
        if (root) root.SetActive(false);
    }

    void ClickYes()
    {
        var cb = _onYes;
        HideImmediate();
        cb?.Invoke();
    }

    void ClickNo()
    {
        var cb = _onNo;
        HideImmediate();
        cb?.Invoke();
    }
}
