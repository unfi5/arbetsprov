using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFiller : MonoBehaviour
{
    public LayerMask WhatRoom;

    private GameObject _LevelGen;
    private LevelGenerator _levelgenerator;

    private void Awake()
    {
        _LevelGen = GameObject.Find("LevelGeneration");
        _levelgenerator = _LevelGen.GetComponent<LevelGenerator>();
    }

    void Update()
    {
        Collider[] roomDetection = Physics.OverlapBox(gameObject.transform.position, transform.localScale, Quaternion.identity, WhatRoom);

        if (roomDetection.Length == 0 && _levelgenerator.StopGeneration == true)
        {
            int rand = Random.Range(0, _levelgenerator.Rooms.Length);
            Instantiate(_levelgenerator.Rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        else if (roomDetection.Length > 0 && _levelgenerator.StopGeneration == true)
        {
        Destroy(gameObject);
        }
    }
}
