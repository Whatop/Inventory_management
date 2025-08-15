using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicFilterPanel : MonoBehaviour
{
    [Header("참조")]
    public FilterUIConfig config;
    public RectTransform rowsParent;   // Vertical Layout Group 권장
    public GameObject rowPrefab;       // FilterRowUI 붙은 프리팹 (필드/연산자/값 3개 UI 포함)
    public Button addRowButton;
    public Button clearButton;
    public Button applyButton;

    private readonly List<FilterRowUI> rows = new List<FilterRowUI>();

    void Awake()
    {
        if (addRowButton != null) addRowButton.onClick.AddListener(AddRow);
        if (clearButton != null) clearButton.onClick.AddListener(ClearRows);
        if (applyButton != null) applyButton.onClick.AddListener(ApplyFilters);

        // 최소 1행
        if (rows.Count == 0) AddRow();
    }

    void AddRow()
    {
        if (rowPrefab == null || rowsParent == null || config == null) return;

        GameObject go = Instantiate(rowPrefab, rowsParent);
        FilterRowUI ui = go.GetComponent<FilterRowUI>();
        if (ui != null)
        {
            ui.Init(config);
            rows.Add(ui);
        }
    }

    void ClearRows()
    {
        for (int i = rows.Count - 1; i >= 0; i--)
        {
            if (rows[i] != null && rows[i].gameObject != null)
                Destroy(rows[i].gameObject);
        }
        rows.Clear();
        AddRow();
    }

    void ApplyFilters()
    {
        List<FilterCondition> conds = new List<FilterCondition>();
        for (int i = 0; i < rows.Count; i++)
        {
            FilterRowUI r = rows[i];
            if (r == null) continue;

            string field = r.GetField(config);
            string val = (r.GetValue() ?? "").Trim();
            if (string.IsNullOrEmpty(field) || string.IsNullOrEmpty(val)) continue;

            FilterCondition fc = new FilterCondition();
            fc.field = field;
            fc.op = r.GetOp();
            fc.value = val;
            conds.Add(fc);
        }

        // GameManager에 적용 요청
        if (GameManager.Instance != null)
            GameManager.Instance.ApplyFiltersAndRefreshUI(conds);
    }
}
