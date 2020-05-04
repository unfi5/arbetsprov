using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialAR : MonoBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] Text _HelpText;
    [SerializeField] Tutorial _tut;

    private bool pInside = false;

    void Update()
    {
        if (pInside)
        {
            if (_HelpText != null)
            {
                _HelpText.text = "[E] to pick up assault rifle";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                _tut.ARpickup = true;
                _player.AssaultRifle.Unlocked = true;
                _HelpText.text = "";
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            pInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            pInside = false;
        }
    }
}
