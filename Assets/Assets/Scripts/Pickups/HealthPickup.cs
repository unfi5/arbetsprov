using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] SoundManager _sm;
    private Player _player;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (_player == null)
            {
                _player = collision.transform.GetComponent<Player>();
            }

            if (_player.Health != 100)
            {
                _player.PlayerHealth(-20);
                _sm.HealthPickup.Post(gameObject);
                Destroy(gameObject);
            }
        }
    }
}
