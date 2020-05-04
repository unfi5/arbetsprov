using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDespawn : MonoBehaviour
{
    private void OnLevelWasLoaded(int level)
    {
        if (level == 0)
        {
            Destroy(gameObject);
        }
    }
}
