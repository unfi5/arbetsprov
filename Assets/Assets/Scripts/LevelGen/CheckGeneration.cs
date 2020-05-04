using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckGeneration : MonoBehaviour
{
    private LevelGenerator Generator;
    private BoxCollider _col;
    private bool _Stop = false;

    private void Awake()
    {
        Generator = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>();
        _col = GetComponent<BoxCollider>();
    }

    void Update()
    {
        if(_Stop == false)
        {
            if(Generator != null)
            {
                if (Generator.StopGeneration == true)
                {
                    StartCoroutine(Delay());
                }
            }
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(10);

        DisableCol();
    }

    void DisableCol()
    {
        _col.enabled = false;
        _Stop = true;
    }
}
