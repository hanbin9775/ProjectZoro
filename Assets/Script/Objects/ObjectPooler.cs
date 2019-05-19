using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;  // Pool 이름       
        public GameObject prefab; // pool에 넣을 prefab
        public int size;    // total size of Pool
    }
    
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Pool이 들어갈 list
    public List<Pool> pools; 

    //사용자가 데이터에 키 값을 설정할 수 있는 Dictonary 자료구조.
    public Dictionary<string, Queue<GameObject>> poolDictionary;


    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>(); 

        foreach (Pool pool in pools)
        {
            //큐 생성
            Queue<GameObject> objectPool = new Queue<GameObject>(); 
            for(int i=0; i<pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                // 생성한 큐에 instantiate한 prefab 집어넣음
                objectPool.Enqueue(obj);    
            }

            poolDictionary.Add(pool.tag, objectPool);

        }

    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag" + tag + " doesn't exist");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();

        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }


}
