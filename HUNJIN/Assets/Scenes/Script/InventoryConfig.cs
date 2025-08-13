using System;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryConfig", menuName = "Inventory/Config")]
public class InventoryConfig : ScriptableObject
{
    [Header("Google Sheet")]
    public string sheetId = "1LomcEbXhTuuskx7AT60yoTnH18NYLHvm3mvGD0g4MkM";

    [Serializable]
    public struct DepartmentSheet
    {
        public string key;     // "All", "Press", "Welding", "Assembly", "Lost"
        public int gid;        // 시트 GID
        public string range;   // 예: "K2:R" 또는 "A2:G"
    }

    [Header("Department Sheets")]
    public DepartmentSheet[] departments = new DepartmentSheet[]
    {
        new DepartmentSheet { key="All",      gid=1973018837, range="K2:R" },
        new DepartmentSheet { key="Press",    gid=0,          range="K2:R" },
        new DepartmentSheet { key="Welding",  gid=1809169708, range="K2:R" },
        new DepartmentSheet { key="Assembly", gid=334896260,  range="K2:R" },
        new DepartmentSheet { key="Lost",     gid=632245483,  range="A2:G" }
    };

    [Header("Caching")]
    public float refreshIntervalSeconds = 600f; // 10분
}
