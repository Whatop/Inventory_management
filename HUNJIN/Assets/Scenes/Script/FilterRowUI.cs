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
            fieldDropdown.AddOptions(new System.Collections.Generic.List<string>(cfg.fields));
        }
        if (opDropdown != null)
        {
            opDropdown.ClearOptions();
            opDropdown.AddOptions(new System.Collections.Generic.List<string>(cfg.operators_ko));
        }
        if (valueInput != null) valueInput.text = "";
    }

    public string GetField(FilterUIConfig cfg)
    {
        if (fieldDropdown == null || cfg == null || cfg.fields.Length == 0) return "";
        int idx = fieldDropdown.value;
        if (idx < 0 || idx >= cfg.fields.Length) return "";
        return cfg.fields[idx];
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
