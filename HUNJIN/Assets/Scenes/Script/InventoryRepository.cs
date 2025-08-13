using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InventoryRepository
{
    private readonly InventoryConfig _config;
    class CacheEntry
    {
        public float ts;
        public List<ProductRecord> data;
        public CacheEntry(float ts, List<ProductRecord> data) { this.ts = ts; this.data = data; }
    }

    private readonly Dictionary<string, CacheEntry> _cache = new Dictionary<string, CacheEntry>();

    public List<ProductRecord> Records { get; private set; } = new List<ProductRecord>();

    public InventoryRepository(InventoryConfig config)
    {
        _config = config;
    }

    public IEnumerator Refresh(string deptKey)
    {
        // 1) �μ� ��Ʈ ���� (������ "All"�� ����)
        InventoryConfig.DepartmentSheet dep = new InventoryConfig.DepartmentSheet();
        bool found = false;

        if (_config != null && _config.departments != null)
        {
            for (int i = 0; i < _config.departments.Length; i++)
            {
                if (_config.departments[i].key == deptKey)
                {
                    dep = _config.departments[i];
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                for (int i = 0; i < _config.departments.Length; i++)
                {
                    if (_config.departments[i].key == "All")
                    {
                        dep = _config.departments[i];
                        found = true;
                        break;
                    }
                }
            }
        }

        // 2) URL ���� (���ڿ� ���� ��� + ���� ���)
        string baseUrl = "https://docs.google.com/spreadsheets/d/";
        string url = baseUrl + _config.sheetId
                   + "/export?format=tsv&gid=" + dep.gid
                   + "&range=" + UnityEngine.Networking.UnityWebRequest.EscapeURL(dep.range);

        // 3) ĳ�� Ȯ��
        CacheEntry entry;
        bool useCache = _cache.TryGetValue(deptKey, out entry)
                        && (Time.realtimeSinceStartup - entry.ts) < _config.refreshIntervalSeconds;

        if (useCache)
        {
            Records = entry.data;
            yield break;
        }

        // 4) �ٿ�ε�
        using (var req = UnityEngine.Networking.UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            // 5) ���� ó�� (Unity ������ �б�)
#if UNITY_2020_2_OR_NEWER
            if (req.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
#else
        if (req.isNetworkError || req.isHttpError)
#endif
            {
                Debug.LogError("[InventoryRepository] Refresh failed: " + req.error + "\nURL: " + url);
                yield break;
            }

            // 6) �Ľ� �� �޸� �ݿ� + ĳ�� ����
            string tsv = req.downloadHandler.text;
            List<ProductRecord> parsed = ParseTsv(tsv); // ���� Ŭ���� �ȿ� ���ǵ� �־�� �մϴ�.
            Records = parsed;
            _cache[deptKey] = new CacheEntry(Time.realtimeSinceStartup, parsed);
        }
    }
    List<ProductRecord> ParseTsv(string tsv)
    {
        var result = new List<ProductRecord>();
        if (string.IsNullOrEmpty(tsv)) return result;

        var lines = tsv.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            var row = lines[i].TrimEnd('\r');
            if (string.IsNullOrWhiteSpace(row)) continue;
            var cols = row.Split('\t');

            // ���� �÷� ���� �ٸ� �� �־� ��������� ó��
            // K..R (8��) �Ǵ� Lost(A..G 7��) ����
            var rec = new ProductRecord();
            try
            {
                // �ε����� ������ �� ���ڿ�
                string C(int idx) => (idx < cols.Length ? cols[idx] : "").Trim();

                rec.Date = C(0).Replace("-", "/");
                rec.SubjectName = C(1).Replace(" ", "");
                rec.Receiving = C(2).Replace(",", "");
                rec.Release = C(3).Replace(",", "");
                rec.CompanyName = C(4);
                rec.ReceivingTime = C(6);
                rec.Gugo = C(7).Replace(" ", "");
            }
            catch { /* �����ϰ� ���� �� */ }

            result.Add(rec);
        }
        return result;
    }

    public List<ProductRecord> ApplyFilters(List<FilterCondition> filters)
    {
        if (filters == null || filters.Count == 0) return new List<ProductRecord>(Records);

        bool Match(string src, FilterOp op, string val)
        {
            src = (src ?? "").ToLower().Trim();
            val = (val ?? "").ToLower().Trim();
            switch (op)
            {
                case FilterOp.Contains: return src.Contains(val);
                case FilterOp.Equals: return src == val;
                case FilterOp.StartsWith: return src.StartsWith(val);
                case FilterOp.EndsWith: return src.EndsWith(val);
                default: return false;
            }
        }

        var list = new List<ProductRecord>();
        foreach (var r in Records)
        {
            bool ok = true;
            foreach (var f in filters)
            {
                string fieldVal =
                    f.field == "Date" ? r.Date :
                    f.field == "SubjectName" ? r.SubjectName :
                    f.field == "Receiving" ? r.Receiving :
                    f.field == "Release" ? r.Release :
                    f.field == "CompanyName" ? r.CompanyName :
                    f.field == "ReceivingTime" ? r.ReceivingTime :
                    f.field == "Gugo" ? r.Gugo : "";

                if (!Match(fieldVal, f.op, f.value)) { ok = false; break; }
            }
            if (ok) list.Add(r);
        }
        return list;
    }
}
