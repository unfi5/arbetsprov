using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelDoor : MonoBehaviour
{
    private Text _text;
    private bool _pInside = false;
    bool EBottonDown;

    private void Awake()
    {
        _text = GameObject.Find("HelpSubtitles").GetComponent<Text>();
    }

    private void Update()
    {
        if (_pInside)
        {
            SetInputs();
        }
    }

    void SetInputs()
    {
        EBottonDown = Input.GetKeyDown(KeyCode.E);
        if (EBottonDown)
        {
            SpawnLevel.SpawnLevelGen();
            SceneManager.LoadScene(1);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            _text.text = "[E] to enter level";
            _pInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            _text.text = "";
            _pInside = false;
        }
    }
}
