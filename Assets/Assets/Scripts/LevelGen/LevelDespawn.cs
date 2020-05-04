using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDespawn : MonoBehaviour
{
    private void Update()
    {
        if (SpawnLevel.IsDone == false)
        {
            Destroy(gameObject);
        }
    }
}
