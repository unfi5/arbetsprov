using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{
    [SerializeField] string _text;
    [SerializeField] string _Objective;
    [SerializeField] Text _ObjectiveText;

    public bool IncludeObjective = false;

    private Text _txt;

    private void Awake()
    {
        _txt = GetComponentInParent<Text>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _txt.text = _text;

            if (IncludeObjective)
            {
                _ObjectiveText.text = _Objective;
            }
        }
    }
}
