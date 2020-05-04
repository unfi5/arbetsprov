using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofGen : MonoBehaviour
{
    private LevelGenerator _gen;
    private MeshRenderer _rend;
    private bool _stop = false;

    void Awake()
    {
        _rend = GetComponentInChildren<MeshRenderer>();
        _rend.enabled = false;
        _gen = GameObject.Find("LevelGeneration").GetComponent<LevelGenerator>();
    }

    void Update()
    {
        if (!_stop)
        {
            if (_gen.StopGeneration == true)
            {
                _rend.enabled = true;
                _stop = true;
            }
        }
    }
}
