using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Ammo : MonoBehaviour
{
    [SerializeField] Player _Player;

    public Text AmmoText;

    private int _AmmoClip;
    private int _MaxAmmoClip;
    private int _Ammo;

    void Update()
    {
        if (_Player.weaponSelected == 1)
        {
            _AmmoClip = _Player.Pistol.MagazineBullets;
            _MaxAmmoClip = _Player.Pistol.ClipSize;
            _Ammo = _Player.Pistol.Ammo;
            AmmoText.text = (_AmmoClip.ToString() + " / " + _MaxAmmoClip + "  " + _Ammo);
        }
        else if (_Player.weaponSelected == 2)
        {
            _AmmoClip = _Player.AssaultRifle.MagazineBullets;
            _MaxAmmoClip = _Player.AssaultRifle.ClipSize;
            _Ammo = _Player.AssaultRifle.Ammo;
            AmmoText.text = (_AmmoClip.ToString() + " / " + _MaxAmmoClip + "  " + _Ammo);
        }
        else
        {
            AmmoText.text = ("");
        }
    }
}
