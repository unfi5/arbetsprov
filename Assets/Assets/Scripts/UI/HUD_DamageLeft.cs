using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_DamageLeft : MonoBehaviour
{
    private Image _Image;
    private float _Fade;

    private void Awake()
    {
        _Image = GetComponent<Image>();
        _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, 0.0f);
    }

    public void DamageLeft()
    {
        _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, 1.0f);
        _Fade = 1;
        StartCoroutine(FadeTimer());
    }

    public void Fader()
    {
        _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, _Fade);
    }

    private IEnumerator FadeTimer()
    {
        while (_Fade > 0)
        {
            yield return new WaitForSeconds(0.05f);
            _Fade -= 0.05f;
            Fader();
        }
    }

}
