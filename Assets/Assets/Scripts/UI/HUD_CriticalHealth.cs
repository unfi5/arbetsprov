using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_CriticalHealth : MonoBehaviour
{
    [SerializeField] Player _player;

    private float _Fade;
    private float _Alpha;
    private Image _Image;

    private void Awake()
    {
        _Image = GetComponent<Image>();
        _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, 0.0f);
        
        RectTransform rt = GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

        _Alpha = 0;
    }

    private void Update()
    {
        if(_player.Health < 41)
        {
            _Fade = _player.Health;
            _Alpha =  (_Fade / -40 + 1f) / 2;

            _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, _Alpha);
        }
        else if(_player.Health > 40 && _Alpha != 0)
        {
            _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, 0);
        }
    }
}
