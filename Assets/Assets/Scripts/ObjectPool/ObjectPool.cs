using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string Tag;
        public GameObject Prefab;
        public int Size;         
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.Size; i++)
            {
                GameObject obj = Instantiate(pool.Prefab);
                obj.SetActive(false);
                obj.transform.parent = transform;
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.Tag, objectPool);
        }
    }

    public GameObject SpawnFromPool (string tag, Vector3 position, Vector3 rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.Log("Pool with tag " + tag + " doesn't exist");
            return null;
        }       

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.eulerAngles = rotation;

        IpooledObject pooledObj = objectToSpawn.GetComponent<IpooledObject>();

        if(pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }
}
