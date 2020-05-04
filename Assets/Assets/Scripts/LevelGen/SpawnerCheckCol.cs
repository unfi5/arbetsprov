using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCheckCol : MonoBehaviour
{
    public GameObject[] objects;
    public int SpawnChance;

    private bool _Spawned = false;
    private GameObject _levelGen;
    private LevelGenerator _levelgenerator;

    void Awake()
    {
        _levelGen = GameObject.Find("LevelGeneration");
        _levelgenerator = _levelGen.GetComponent<LevelGenerator>();

        SpawnChance = Random.Range(0, 2);
        if (SpawnChance == 1)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {        
        if (_levelgenerator.StopGeneration == true && _Spawned == false)
        {
            int rand = Random.Range(0, objects.Length);
            GameObject instance = (GameObject)Instantiate(objects[rand], transform.position, Quaternion.identity);
            instance.transform.parent = transform;
            instance.transform.eulerAngles = new Vector3(0, Random.Range(-1, 360), 0);
            _Spawned = true;
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer != 11 && _Spawned == false)
        {
            Destroy(gameObject);
        }
    }
}
