using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ScrollViewController))]
public class ObjectPoolerEditor : Editor
{
    const string INFO = "Ǯ���� ������Ʈ�� ������ �������� \nvoid OnDisable()\n{\n" +
    "    ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� \n" +
    "    CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� \n}";

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox(INFO, MessageType.Info);
        base.OnInspectorGUI();
    }


}
#endif

public class ScrollViewController : MonoBehaviour
{
    static ScrollViewController inst;
    void Awake() => inst = this;

    public GameManager gameManager;
    public ScrollRect scrollRect;
    public float space = 10f;
    public List<RectTransform> uiObjects = new List<RectTransform>();
    private int spId = -1;
    public bool dont = false;
    bool first;
    public int reveprod = 0;

    //����, ����, ����, ��ü

    [Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    [SerializeField] Pool[] pools;
    List<GameObject> spawnObjects;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    readonly string INFO = " ������Ʈ�� ������ �������� \\nvoid OnDisable()\\n{\\n" +
        "    ObjectPooler.ReturnToPool(gameObject);    // �� ��ü�� �ѹ��� \\n" +
        "    CancelInvoke();    // Monobehaviour�� Invoke�� �ִٸ� \\n}";

    public int GetId()
    {
        spId++;
        return spId;
    }

    public void ResetId()
    {
        spId = -1;
    }

    public static ScrollViewController Instance
    {
        get
        {
            if (null == inst)
            {
                return null;
            }
            return inst;
        }
    }

    public static GameObject SpawnFromPool(string tag, Vector3 position) =>
        inst._SpawnFromPool(tag, position, Quaternion.identity);

    public static GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation) =>
        inst._SpawnFromPool(tag, position, rotation);

    public static T SpawnFromPool<T>(string tag, Vector3 position) where T : Component
    {
        GameObject obj = inst._SpawnFromPool(tag, position, Quaternion.identity);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    public static T SpawnFromPool<T>(string tag, Vector3 position, Quaternion rotation) where T : Component
    {
        GameObject obj = inst._SpawnFromPool(tag, position, rotation);
        if (obj.TryGetComponent(out T component))
            return component;
        else
        {
            obj.SetActive(false);
            throw new Exception($"Component not found");
        }
    }

    public static List<GameObject> GetAllPools(string tag)
    {
        if (!inst.poolDictionary.ContainsKey(tag))
            throw new Exception($"Pool with tag {tag} doesn't exist.");

        return inst.spawnObjects.FindAll(x => x.name == tag);
    }

    public static List<T> GetAllPools<T>(string tag) where T : Component
    {
        List<GameObject> objects = GetAllPools(tag);

        if (!objects[0].TryGetComponent(out T component))
            throw new Exception("Component not found");

        return objects.ConvertAll(x => x.GetComponent<T>());
    }

    public static void ReturnToPool(GameObject obj)
    {
        if (!inst.poolDictionary.ContainsKey(obj.name))
            throw new Exception($"Pool with tag {obj.name} doesn't exist.");

        inst.poolDictionary[obj.name].Enqueue(obj);
    }

    [ContextMenu("GetSpawnObjectsInfo")]
    void GetSpawnObjectsInfo()
    {
        foreach (var pool in pools)
        {
            int count = spawnObjects.FindAll(x => x.name == pool.tag).Count;
            Debug.Log($"{pool.tag} count : {count}");
        }
    }

    GameObject _SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
            throw new Exception($"Pool with tag {tag} doesn't exist.");

        // ť�� ������ ���� �߰�
        Queue<GameObject> poolQueue = poolDictionary[tag];
        if (poolQueue.Count <= 0)
        {
            Pool pool = Array.Find(pools, x => x.tag == tag);
            var obj = CreateNewObject(pool.tag, pool.prefab);
            ArrangePool(obj);
        }

        // ť���� ������ ���
        GameObject objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    public void Start()
    {
        spawnObjects = new List<GameObject>();
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // �̸� ����
        foreach (Pool pool in pools)
        {
            poolDictionary.Add(pool.tag, new Queue<GameObject>());
            for (int i = 0; i < pool.size; i++)
            {
                var obj = CreateNewObject(pool.tag, pool.prefab);
                ArrangePool(obj);
            }

            // OnDisable�� ReturnToPool �������ο� �ߺ����� �˻�
            if (poolDictionary[pool.tag].Count <= 0)
                Debug.LogError($"{pool.tag}{INFO}");
            else if (poolDictionary[pool.tag].Count != pool.size)
                Debug.LogError($"{pool.tag}�� ReturnToPool�� �ߺ��˴ϴ�");
        }
        Invoke("Init", 0.1f);
    }

    GameObject CreateNewObject(string tag, GameObject prefab)
    {
        var obj = Instantiate(prefab, scrollRect.content).GetComponent<RectTransform>();
        obj.name = tag;
        uiObjects.Add(obj);
        obj.gameObject.SetActive(false); // ��Ȱ��ȭ�� ReturnToPool�� �ϹǷ� Enqueue�� ��
        return obj.gameObject;
    }

    public void Init()
    {
    }

    // �˻� �� �� �˻� �縸ŭ ����
    public void Inquiry()
    {
        float y = 5f;
        for (int i = 0; i < uiObjects.Count; i++)
        {
            if (uiObjects[i].gameObject.activeSelf)
            {
                uiObjects[i].anchoredPosition = new Vector2(0f, -y);
                y += uiObjects[i].sizeDelta.y + space;
            }
        }
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
    }

    public void UIObjectReset()
    {
        if (uiObjects != null)
        {
            foreach (var obj in uiObjects)
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    public void Search()
    {
        int c = uiObjects.FindAll(x => x.name == "Subject").Count;
        gameManager.ResetData();
        gameManager.isSubject = false;
        ResetId();
        dont = true;
        int count = gameManager.ProductSearch();
        if (count > c - reveprod)
        {
            for (int i = 0; i < c - reveprod; i++)
            {
                SpawnFromPool("Subject", transform.position);
            }
            reveprod = 0;
        }
        dont = false;
        gameManager.isCompanyName = false;
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < count; i++)
        {
            SpawnFromPool("Subject", transform.position);
        }
        reveprod += gameManager.MySearchData.Count;
        Inquiry();
    }

    public void DropSearch()
    {
        int c = uiObjects.FindAll(x => x.name == "Subject").Count;
        gameManager.isSubject = false;
        ResetId();
        dont = true;
        int count = 0;
        if (gameManager.CompanySearch == 0)
        {
            Search();
        }
        else
        {
            count = gameManager.AAASearch();
            if (count > c - reveprod)
            {
                for (int i = 0; i < c - reveprod; i++)
                {
                    SpawnFromPool("Subject", transform.position);
                }
                reveprod = 0;
            }
            dont = false;
            gameManager.isCompanyName = false;
            for (int i = 0; i < uiObjects.Count; i++)
            {
                uiObjects[i].gameObject.SetActive(false);
            }
            for (int i = 0; i < count; i++)
            {
                SpawnFromPool("Subject", transform.position);
            }
        }
        reveprod += gameManager.MySearchData.Count;
        gameManager.Resetdropdown();
        Inquiry();
    }


    public void TextSearch(Text text)
    {
        int c = uiObjects.FindAll(x => x.name == "Subject").Count;
        gameManager.isSubject = true;
        ResetId();
        dont = true;
        int count = gameManager.ProductSearch();
        if (count > c - reveprod)
        {
            for (int i = 0; i < c - reveprod; i++)
            {
                SpawnFromPool("Subject", transform.position);
            }
            reveprod = 0;
        }
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].gameObject.SetActive(false);
        }
        dont = false;
        gameManager.isCompanyName = false;
        for (int i = 0; i < count; i++)
        {
            SpawnFromPool("Subject", transform.position);
        }
        reveprod += gameManager.MySearchData.Count;
        Inquiry();
    }

    public void ReTextSearch(Text text)
    {
        int c = uiObjects.FindAll(x => x.name == "Subject").Count;
        gameManager.isSubject = true;
        ResetId();
        dont = true;
        int count = gameManager.DOTextSearch(text.text.Trim());
        if (count > c - reveprod)
        {
            for (int i = 0; i < c - reveprod; i++)
            {
                SpawnFromPool("Subject", transform.position);
            }
            reveprod = 0;
        }
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].gameObject.SetActive(false);
        }
        dont = false;
        gameManager.isCompanyName = false;
        for (int i = 0; i < count; i++)
        {
            SpawnFromPool("Subject", transform.position);
        }
        reveprod += gameManager.MySearchData.Count;
        Inquiry();
    }

    public void DoSearch()
    {
        gameManager.OnDropdownEvent(0);

        int c = uiObjects.FindAll(x => x.name == "Subject").Count;
        gameManager.isSubject = false;
        ResetId();
        dont = true;
        int count = gameManager.SEadSearch();
        if (count > c - reveprod)
        {
            for (int i = 0; i < c - reveprod; i++)
            {
                SpawnFromPool("Subject", transform.position);
            }
            reveprod = 0;
        }
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].gameObject.SetActive(false);
        }
        dont = false;
        gameManager.isCompanyName = false;
        for (int i = 0; i < count; i++)
        {
            SpawnFromPool("Subject", transform.position);
        }
        reveprod += gameManager.MySearchData.Count;
        Inquiry();
        gameManager.isSed = false;
    }

    public void LostSearch()
    {
        gameManager.OnDropdownEvent(0);
        int c = uiObjects.FindAll(x => x.name == "Subject").Count;
        gameManager.isSubject = false;
        ResetId();
        dont = true;
        int count = gameManager.LostTextSearch();
        if (count > c - reveprod)
        {
            for (int i = 0; i < c - reveprod; i++)
            {
                SpawnFromPool("Subject", transform.position);
            }
            reveprod = 0;
        }
        for (int i = 0; i < uiObjects.Count; i++)
        {
            uiObjects[i].gameObject.SetActive(false);
        }
        dont = false;
        gameManager.isCompanyName = false;
        for (int i = 0; i < count; i++)
        {
            SpawnFromPool("Subject", transform.position);
        }
        reveprod += gameManager.MySearchData.Count;
        gameManager.isSed = false;
        Inquiry();
    }

    void ArrangePool(GameObject obj)
    {
        // �߰��� ������Ʈ ��� ����
        bool isFind = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i == transform.childCount - 1)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
            else if (transform.GetChild(i).name == obj.name)
                isFind = true;
            else if (isFind)
            {
                obj.transform.SetSiblingIndex(i);
                spawnObjects.Insert(i, obj);
                break;
            }
        }
    }


}