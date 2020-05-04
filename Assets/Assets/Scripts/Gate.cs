using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gate : MonoBehaviour
{
    private HelpSubtitles _HelpSubs;
    private bool _canOpen = false;
    private bool _isOpen = false;
    private bool _EButtonDown;

    private void Update()
    {
        if (_canOpen == true && !_isOpen)
        {
            SetInputs();
        }
    }

    void SetInputs()
    {
        _EButtonDown = Input.GetKeyDown(KeyCode.E);
        if (_EButtonDown)
        {
            StartCoroutine(Open());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && !_isOpen)
        {
            if (_HelpSubs == null)
            {
                _HelpSubs = GameObject.Find("HelpSubtitles").GetComponent<HelpSubtitles>();
            }

            if(other.GetComponent<Player>().KeyCards >= 3)
            {
                _HelpSubs.HelpText("[E] to open", 0);
                _canOpen = true;
            }
            else if (other.GetComponent<Player>().KeyCards == 2)
            {
                _HelpSubs.HelpText("You need one more Keycard.", 0);
            }
            else if (other.GetComponent<Player>().KeyCards == 1)
            {
                _HelpSubs.HelpText("You need two more Keycards.", 0);
            }
            else
            {
                _HelpSubs.HelpText("You need 3 keycards.", 0);
            }
        }

    }


    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            _HelpSubs.HelpText("", 0);
            _canOpen = false;
        }
    }

    IEnumerator Open()
    {
        _isOpen = true;
        float y = 0;
        while (y < 2)
        {
            yield return new WaitForSeconds(0.01f);
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.02f, transform.position.z);
            y += 0.02f;
        }
    }

}
