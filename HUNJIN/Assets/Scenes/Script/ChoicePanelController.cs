using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChoicePanelController : MonoBehaviour
{
    [Header("Root")]
    public GameObject root;

    [Header("UI")]
    public TMP_Text titleText;   // 없으면 비워도 됨
    public TMP_Text messageText;
    public Button yesButton;
    public Button noButton;

    private Action _onYes;
    private Action _onNo;

    void Awake()
    {
        if (yesButton) yesButton.onClick.AddListener(OnYes);
        if (noButton) noButton.onClick.AddListener(OnNo);
        Hide();
    }

    public void Open(string title, string message, Action onYes, Action onNo)
    {
        if (titleText) titleText.text = title ?? "";
        if (messageText) messageText.text = message ?? "";
        _onYes = onYes;
        _onNo = onNo;

        if (root) root.SetActive(true);
        else gameObject.SetActive(true);
    }

    public void Show(string message)
    {
        Open("", message, null, null);
    }

    public void Hide()
    {
        if (root) root.SetActive(false);
        else gameObject.SetActive(false);

        _onYes = null;
        _onNo = null;
    }

    private void OnYes()
    {
        var cb = _onYes;
        Hide();
        cb?.Invoke();
    }

    private void OnNo()
    {
        var cb = _onNo;
        Hide();
        cb?.Invoke();
    }
}
