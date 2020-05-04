using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField] TutorialDoor _room2Door;
    [SerializeField] TutorialDoor _room3Door;

    public int room2Hits = 0;
    public int room3Lure = 0;
    public bool ARpickup = false;

    private Text _txt;
    private Player _player;

    private void Awake()
    {
        _txt = GetComponentInChildren<Text>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        if(room2Hits > 2)
        {
            _room2Door.openDoor();
        }

        if(room3Lure > 1)
        {
            _room3Door.openDoor();
        }

        if (ARpickup)
        {
            _txt.text = "[Scrollwheel] or [2] to switch to assualtrifle";

            float d = Input.GetAxis("Mouse ScrollWheel");
            if (d < 0 || d > 0 || Input.GetKeyDown("1") || Input.GetKeyDown("2"))
            {
                ARpickup = false;
                _txt.text = "";
            }
        }
    }
}
