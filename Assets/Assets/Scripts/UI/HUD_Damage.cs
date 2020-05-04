using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Damage : MonoBehaviour
{
    public float Fade;
    private Player _player;
    private int _CurrentHealth;
    private float _DelayTime = 0.2f;
    private Image _Image;
    private Color _tempcolor;
    private float _FadeTime = 0.5f;

    private void Awake()
    {
        _Image = GetComponent<Image>();
        _Image.color = new Vector4 (_Image.color.r, _Image.color.g, _Image.color.b, 0.0f);
        _player = GetComponentInParent<Player>();
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);
        _CurrentHealth = _player.Health;
    }

    void Update()
    {
        if(_CurrentHealth != 0)
        {

            if (_CurrentHealth > _player.Health)
            {
                _CurrentHealth = _player.Health;
                Fade = 1;
                _Image = GetComponent<Image>();
                _tempcolor = _Image.color;
                _tempcolor.a = 1;
                StartCoroutine(Delay());
            }
            _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, Fade);
        }
        else
        {
            _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, 1.0f);
        }
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(_DelayTime);

        StartCoroutine(FadeTimer());
    }

    private IEnumerator FadeTimer()
    {
        while (Fade > 0)
        {
            yield return new WaitForSeconds(_FadeTime);
            Fade -= _FadeTime;
        }
    }

}
