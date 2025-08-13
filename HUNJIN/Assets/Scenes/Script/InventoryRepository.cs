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
        // 1) 부서 시트 선택 (없으면 "All"로 폴백)
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

        // 2) URL 구성 (문자열 보간 대신 + 연산 사용)
        string baseUrl = "https://docs.google.com/spreadsheets/d/";
        string url = baseUrl + _config.sheetId
                   + "/export?format=tsv&gid=" + dep.gid
                   + "&range=" + UnityEngine.Networking.UnityWebRequest.EscapeURL(dep.range);

        // 3) 캐시 확인
        CacheEntry entry;
        bool useCache = _cache.TryGetValue(deptKey, out entry)
                        && (Time.realtimeSinceStartup - entry.ts) < _config.refreshIntervalSeconds;

        if (useCache)
        {
            Records = entry.data;
            yield break;
        }

        // 4) 다운로드
        using (var req = UnityEngine.Networking.UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();

            // 5) 에러 처리 (Unity 버전별 분기)
#if UNITY_2020_2_OR_NEWER
            if (req.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
#else
        if (req.isNetworkError || req.isHttpError)
#endif
            {
                Debug.LogError("[InventoryRepository] Refresh failed: " + req.error + "\nURL: " + url);
                yield break;
            }

            // 6) 파싱 → 메모리 반영 + 캐시 저장
            string tsv = req.downloadHandler.text;
            List<ProductRecord> parsed = ParseTsv(tsv); // 같은 클래스 안에 정의돼 있어야 합니다.
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

            // 예상 컬럼 수가 다를 수 있어 방어적으로 처리
            // K..R (8개) 또는 Lost(A..G 7개) 기준
            var rec = new ProductRecord();
            try
            {
                // 인덱스가 없으면 빈 문자열
                string C(int idx) => (idx < cols.Length ? cols[idx] : "").Trim();

                rec.Date = C(0).Replace("-", "/");
                rec.SubjectName = C(1).Replace(" ", "");
                rec.Receiving = C(2).Replace(",", "");
                rec.Release = C(3).Replace(",", "");
                rec.CompanyName = C(4);
                rec.ReceivingTime = C(6);
                rec.Gugo = C(7).Replace(" ", "");
            }
            catch { /* 무시하고 다음 행 */ }

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
