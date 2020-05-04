using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] objects;
    private LevelGenerator _levelgenerator;
    private bool _spawned = false;

    private void Awake()
    {
        _levelgenerator = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>();
    }

    private void Update()
    {
        if (_spawned) return;
        if (_levelgenerator.SpawnedPlayer)
        {
            int rand = Random.Range(0, objects.Length);
            GameObject instance = (GameObject)Instantiate(objects[rand], transform.position, Quaternion.identity);
            instance.transform.parent = transform;
            _spawned = true;
        }
    }

}
