using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(ScrollViewController))]
public class ObjectPoolerEditor : Editor
{
	const string INFO = "풀링한 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
		"    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";

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
	readonly string INFO = " 오브젝트에 다음을 적으세요 \nvoid OnDisable()\n{\n" +
		"    ObjectPooler.ReturnToPool(gameObject);    // 한 객체에 한번만 \n" +
		"    CancelInvoke();    // Monobehaviour에 Invoke가 있다면 \n}";

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

		// 큐에 없으면 새로 추가
		Queue<GameObject> poolQueue = poolDictionary[tag];
		if (poolQueue.Count <= 0)
		{
			Pool pool = Array.Find(pools, x => x.tag == tag);
			var obj = CreateNewObject(pool.tag, pool.prefab);
			ArrangePool(obj);
		}

		// 큐에서 꺼내서 사용
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

		// 미리 생성
		foreach (Pool pool in pools)
		{
			poolDictionary.Add(pool.tag, new Queue<GameObject>());
			for (int i = 0; i < pool.size; i++)
			{
				var obj = CreateNewObject(pool.tag, pool.prefab);
				ArrangePool(obj);
			}

			// OnDisable에 ReturnToPool 구현여부와 중복구현 검사
			if (poolDictionary[pool.tag].Count <= 0)
				Debug.LogError($"{pool.tag}{INFO}");
			else if (poolDictionary[pool.tag].Count != pool.size)
				Debug.LogError($"{pool.tag}에 ReturnToPool이 중복됩니다");
		}
		Invoke("Init", 0.1f);
	}

	GameObject CreateNewObject(string tag, GameObject prefab)
	{
		var obj = Instantiate(prefab, scrollRect.content).GetComponent<RectTransform>();
		obj.name = tag;
		uiObjects.Add(obj);
		obj.gameObject.SetActive(false); // 비활성화시 ReturnToPool을 하므로 Enqueue가 됨
		return obj.gameObject;
	}

	public void Init()
	{
	}

	//검색후 그 검색 양만큼 저거되도록 변경
	public void Inquiry() //조회
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
			for (int i = 0; i < uiObjects.Count; i++)
			{
				uiObjects[i].gameObject.SetActive(false);
			}
		}
	}
	public void Search() //검색
	{
		int c = uiObjects.FindAll(x => x.name == "Subject").Count;
		//검색한 내용과 비교해서 
		// 개수 생성
		GameManager.Instance.ResetData();
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
		GameManager.Instance.isCompanyName = false;
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

	public void DropSearch() //검색
	{
		int c = uiObjects.FindAll(x => x.name == "Subject").Count;
		//검색한 내용과 비교해서 
		// 개수 생성
		gameManager.isSubject = false;
		ResetId();
		dont = true;
		int count = 0;
		if (gameManager.CompanySearch == 0)
		{
			int a = uiObjects.Count;
			for (int i = 0; i < a; i++)
			{
				uiObjects[i].gameObject.SetActive(false);
			}

			count = gameManager.ALLDOTextSearch();
			for (int i = 0; i < count; i++)
			{
				SpawnFromPool("SubjectAll", transform.position);
			}
			Inquiry();
		}
		else
		{
			count = gameManager.ProductSearch();

			if (count > c - reveprod)
			{
				for (int i = 0; i < c - reveprod; i++)
				{
					SpawnFromPool("Subject", transform.position);
				}
				reveprod = 0;
			}
			dont = false;
			GameManager.Instance.isCompanyName = false;
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
	public void TextSearch(Text text) //검색
	{
		int
			c = uiObjects.FindAll(x => x.name == "Subject").Count;
		//검색한 내용과 비교해서 
		// 개수 생성
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
		GameManager.Instance.isCompanyName = false;
		for (int i = 0; i < count; i++)
		{
			SpawnFromPool("Subject", transform.position);

		}
		reveprod += gameManager.MySearchData.Count;

		Inquiry();
	}
	public void ReTextSearch(Text text) //검색
	{
		//검색한 내용과 비교해서 
		// 개수 생성
		int
			c = uiObjects.FindAll(x => x.name == "Subject").Count;
		gameManager.isSubject = true;
		ResetId();
		dont = true;
		int count = gameManager.DOTextSearch(text.text.Trim());
		if (count > c - reveprod) // 검색된 양 > Subject 전체 - 이전 검색양
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
		GameManager.Instance.isCompanyName = false;
		for (int i = 0; i < count; i++)
		{
			SpawnFromPool("Subject", transform.position);

		}
		reveprod += gameManager.MySearchData.Count;
		Inquiry();
	}

	public void DoSearch() //검색
	{
		GameManager.Instance.AllSubjectCountText.gameObject.SetActive(false);
		GameManager.Instance.AllCount.gameObject.SetActive(false);
		gameManager.isSubject = false;
		//검색한 내용과 비교해서 
		// 개수 생성
		ResetId();
		if (!first)
		{
			first = true;
		}
		dont = true;
		dont = false;
		int a = uiObjects.Count;
		for (int i = 0; i < a; i++)
		{
			uiObjects[i].gameObject.SetActive(false);
		}

		int count = gameManager.ALLDOTextSearch();
		//for (int i = 0; i < count; i++)
		//{
		//	SpawnFromPool("SubjectAll", transform.position);
		//}
		Inquiry();

	}


	void ArrangePool(GameObject obj)
	{
		// 추가된 오브젝트 묶어서 정렬
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


