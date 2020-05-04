using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] SoundManager _sm;

    public bool Pistol;
    public bool AR;

    private Player _player;
    private Text _HelpText;
    private bool _pInside = false;
    private bool _EButtonDown;

    private void Update()
    {
        if (_pInside)
        {
            if(_HelpText != null)
            {
                _HelpText.text = "[E] to pick up assault rifle";
            }

            if (AR && _player.AssaultRifle.Unlocked == false)
            {
                SetInput();
            }
        }

    }

    void SetInput()
    {
        _EButtonDown = Input.GetKeyDown(KeyCode.E);
        if (_EButtonDown)
        {
            _player.AssaultRifle.Unlocked = true;
            if (_player.Pistol.Unlocked == false)
            {
                _player.weaponSelected = 2;
                _player.AssaultRifleObject.SetActive(true);
            }
            _HelpText.text = "";
            _sm.WeaponPickup.Post(gameObject);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            if(_player == null)
            {
                _player = other.transform.GetComponent<Player>();
                _HelpText = GameObject.Find("HelpSubtitles").GetComponent<Text>();
            }          

            if (Pistol && _player.Pistol.Unlocked == false)
            {
                _player.Pistol.Unlocked = true;
                if (_player.AssaultRifle.Unlocked == false)
                {
                    _player.weaponSelected = 1;
                    _player.PistolObject.SetActive(true);
                }
                _sm.WeaponPickup.Post(gameObject);
                Destroy(gameObject);
            }
            else if (AR && !_player.Pistol.Unlocked && !_player.AssaultRifle.Unlocked)
            {
                _player.AssaultRifle.Unlocked = true;
                if (_player.Pistol.Unlocked == false)
                {
                    _player.weaponSelected = 2;
                    _player.AssaultRifleObject.SetActive(true);
                }
                _sm.WeaponPickup.Post(gameObject);
                Destroy(gameObject);
            }
            else if (Pistol && _player.Pistol.Unlocked == true)
            {
                _player.Pistol.Ammo += 6;
                _sm.WeaponPickup.Post(gameObject);
                Destroy(gameObject);
            }
            else if (AR && _player.AssaultRifle.Unlocked == true)
            {
                _player.AssaultRifle.Ammo += 10;
                _sm.WeaponPickup.Post(gameObject);
                Destroy(gameObject);
            }

            _pInside = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            _pInside = false;
        }
    }
}
