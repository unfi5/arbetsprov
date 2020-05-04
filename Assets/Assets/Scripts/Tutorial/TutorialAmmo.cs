using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAmmo : MonoBehaviour
{
    public bool Pistol = false;
    public bool AR = false;
    public bool Throwable = false;
    public bool Health = false;

    private bool _spawned = true;
    private bool _PlayerCheck = false;
    private Player _player;
    private MeshRenderer _rend;
    private LevelGenerator _levelgen;

    private void Awake()
    {
        _rend = GetComponent<MeshRenderer>();
        _spawned = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && _spawned)
        {
            if (!_PlayerCheck)
            {
                _player = other.GetComponent<Player>();
            }

            if (Pistol)
            {
                _player.Pistol.Ammo += 12;
                StartCoroutine(Respawn());
            }
            else if (AR)
            {
                _player.AssaultRifle.Ammo += 30;
                StartCoroutine(Respawn());
            }
            else if (Throwable)
            {
                _player.ThrowAmmo += 2;
                StartCoroutine(Respawn());
            }
            else if (Health)
            {
                _player.PlayerHealth(-25);
                StartCoroutine(Respawn());
            }
        }
    }

    IEnumerator Respawn()
    {
        _rend.enabled = false;
        _spawned = false;
        yield return new WaitForSeconds(5);
        _rend.enabled = true;
        _spawned = true;
    }
}
