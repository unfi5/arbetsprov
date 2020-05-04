using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public bool randomRotation = false;
    public GameObject[] objects;

    private void Awake()
    {
        int rand = Random.Range(0, objects.Length);
        GameObject instance = (GameObject)Instantiate(objects[rand], transform.position, Quaternion.identity);
        instance.transform.parent = transform;
        if (randomRotation)
        {
            instance.transform.eulerAngles = new Vector3(0, Random.Range(-1, 360), 0); 
        }
    }
}
