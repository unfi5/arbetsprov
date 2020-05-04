using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToLevelGen : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            SpawnLevel.SpawnLevelGen();
            SceneManager.LoadScene(1);
        }
    }
}
