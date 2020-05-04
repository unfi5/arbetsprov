using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    void Awake()
    {
        transform.Rotate(new Vector3(90.0f, -90.0f, 0));
    }
}
