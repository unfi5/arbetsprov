using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDoor : MonoBehaviour
{
    public GameObject Handle;
    bool _isOpen = false;
    private Light _light;

    private void Awake()
    {
        _light = GetComponentInChildren<Light>();
    }

    public void openDoor()
    {
        if (!_isOpen)
        {
            _isOpen = true;
            StartCoroutine(HandleRotate());
            StartCoroutine(RotateOpen());
        }
    }

    IEnumerator HandleRotate()
    {
        float hRot = 0;
        while (hRot < 35)
        {
            yield return new WaitForSeconds(0.01f);
            Handle.transform.eulerAngles = new Vector3(Handle.transform.eulerAngles.x + 2f, Handle.transform.eulerAngles.y, Handle.transform.eulerAngles.z);
            hRot += 2f;
        }
        while (hRot > 0)
        {
            yield return new WaitForSeconds(0.01f);
            Handle.transform.eulerAngles = new Vector3(Handle.transform.eulerAngles.x - 2f, Handle.transform.eulerAngles.y, Handle.transform.eulerAngles.z);
            hRot -= 2f;
        }

    }

    IEnumerator RotateOpen()
    {
        float Rot = 0;
        while (Rot < 90)
        {
            yield return new WaitForSeconds(0.01f);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + 2f, transform.eulerAngles.z);
            Rot += 2f;
        }
        _light.color = Color.green;
    }
}
