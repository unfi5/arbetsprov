using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Throw : MonoBehaviour
{
    public Text AmmoText;
    private Player _player;
    private int _Ammo;

    private void Awake()
    {
        _player = GetComponentInParent<Player>();
    }

    void Update()
    {
        _Ammo = _player.ThrowAmmo;
        AmmoText.text = _Ammo.ToString();
    }
}
