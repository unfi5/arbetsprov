using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Corshair : MonoBehaviour
{

    [SerializeField] Player _player;
    [SerializeField] Gun _pistol;
    [SerializeField] AssaultRifle _arifle;

    private RectTransform _rt;
    private Image _Image;
    private float __alpha;
    private float _Spread;
    private Vector3 _oScale;

    void Awake()
    {
        _rt = gameObject.GetComponent<RectTransform>();
        _Image = GetComponent<Image>();
        _oScale = _rt.localScale;
    }

    private void Start()
    {
        if(_player.weaponSelected == 0)
        {
            if (__alpha != 0)
            {
                __alpha = 0;
                _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, __alpha);
            }
        }
    }

    void Update()
    {
        //Pistol Spread
        if(_player.weaponSelected == 1)
        {
            if (__alpha != 1)
            {
                __alpha = 1;
                _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, __alpha);
            }

            _Spread = _pistol.Spread * 100;
            _rt.localScale = new Vector3(_oScale.x + _Spread, _oScale.y + _Spread, _oScale.z + _Spread);

        }

        //AssRifle Spread
        else if(_player.weaponSelected == 2)
        {
            if (__alpha != 1)
            {
                __alpha = 1;
                _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, __alpha);
            }

            _Spread = _arifle.aSpread * 100;
            _rt.localScale = new Vector3(_oScale.x + _Spread, _oScale.y + _Spread, _oScale.z + _Spread);

        }

        //if not wep 1 or 2 dont show corsair
        else
        {
            if(__alpha != 0)
            {
                __alpha = 0;
                _Image.color = new Vector4(_Image.color.r, _Image.color.g, _Image.color.b, __alpha);
            }
        }        
    }
}
