using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private float _Rot;
    private float _time;

    void Update()
    {
        if(_Rot == 360)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z);
        }

        _time = Time.deltaTime * 100;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + _time, transform.eulerAngles.z);
        _Rot += _time;
    }
}
