using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    public GameObject Handle;

    bool isOpen = false;
    bool isRotating = false;
    bool canOpen = false;
    bool EButtonDown;

    private Text _HelpText;
    private float _startRot;

    private void Awake()
    {
        _startRot = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (canOpen == true)
        {
            Setinputs();
        }
    }

    void Setinputs()
    {
        EButtonDown = Input.GetKeyDown(KeyCode.E);
        if (EButtonDown)
        {
            openDoor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            if(_HelpText == null)
            {
                _HelpText = GameObject.Find("HelpSubtitles").GetComponent<Text>();
            }

            canOpen = true;
        }
        else if (other.gameObject.layer == 12 && !isOpen)
        {
            openDoor();
        }
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 9)
        {

            if (isOpen == false)
            {
                _HelpText.text = "[E] to open";
            }
            else
            {
                _HelpText.text = "[E] to close";
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            _HelpText.text = "";
            canOpen = false;
        }
        else if(other.gameObject.layer == 12 && isOpen)
        {
            openDoor();
        }
    }

    private void openDoor()
    {
        if(isRotating == false)
        {
            isRotating = true;

            if (isOpen == false)
            {
                StartCoroutine(HandleRotate());
                StartCoroutine(RotateOpen());
            }
            else
            {
                StartCoroutine(RotateClose());
            }
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
        isOpen = true;
        isRotating = false;
    }

    IEnumerator RotateClose()
    {
        float Rot = 0;
        while (Rot < 90)
        {
            yield return new WaitForSeconds(0.01f);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + -2f, transform.eulerAngles.z);
            Rot += 2f;
        }
        isOpen = false;
        isRotating = false;
    }
}
