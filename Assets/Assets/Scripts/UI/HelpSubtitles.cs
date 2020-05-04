using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpSubtitles : MonoBehaviour
{
    private Text _hSubs;

    void Awake()
    {
        _hSubs = GetComponent<Text>();
    }

    public void HelpText(string text, float time)
    {
        _hSubs.text = text;
        if (time != 0)
        {
            StartCoroutine(Delay(time));
        }
    }

    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        _hSubs.text = "";
    }
}
