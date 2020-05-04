using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YZFlipper : MonoBehaviour
{
    void Awake()
    {
        Vector3 Pos = transform.position;
        transform.position.Set(Pos.x, Pos.z, Pos.y);
    }
}
