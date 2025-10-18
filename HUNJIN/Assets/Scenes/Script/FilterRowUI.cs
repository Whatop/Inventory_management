using UnityEngine;
using UnityEngine.UI;

public class FilterRowUI : MonoBehaviour
{
    public Dropdown fieldDropdown;
    public Dropdown opDropdown;
    public InputField valueInput;

    public void Init(FilterUIConfig cfg)
    {
        if (fieldDropdown != null)
        {
            fieldDropdown.ClearOptions();

            // labels 존재하면 라벨(한국어)로 표시, 없으면 키(영문)로 표시
            var labels = (cfg.fieldLabels_ko != null && cfg.fieldLabels_ko.Length == cfg.fields.Length)
                ? new System.Collections.Generic.List<string>(cfg.fieldLabels_ko)
                : new System.Collections.Generic.List<string>(cfg.fields);

            fieldDropdown.AddOptions(labels);
        }
        if (opDropdown != null)
        {
            opDropdown.ClearOptions();
            opDropdown.AddOptions(new System.Collections.Generic.List<string>(cfg.operators_ko));
        }
        if (valueInput != null) valueInput.text = "";
    }

    // 선택된 항목의 "키"를 반환(내부 필터링은 계속 키로 동작)
    public string GetField(FilterUIConfig cfg)
    {
        if (fieldDropdown == null || cfg == null || cfg.fields.Length == 0) return "";
        int idx = fieldDropdown.value;
        if (idx < 0 || idx >= cfg.fields.Length) return "";
        return cfg.fields[idx]; // <- 키 반환(변경 없음)
    }

    public FilterOp GetOp()
    {
        if (opDropdown == null) return FilterOp.Contains;
        switch (opDropdown.value)
        {
            case 0: return FilterOp.Contains;   // 포함
            case 1: return FilterOp.Equals;     // 같음
            case 2: return FilterOp.StartsWith; // 시작
            case 3: return FilterOp.EndsWith;   // 끝남
            default: return FilterOp.Contains;
        }
    }

    public string GetValue()
    {
        return valueInput != null ? valueInput.text : "";
    }
}
