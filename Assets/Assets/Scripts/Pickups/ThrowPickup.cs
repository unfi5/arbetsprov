using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPickup : MonoBehaviour
{
    [SerializeField] SoundManager _sm;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.transform.GetComponent<Player>().ThrowAmmo += 2;
            _sm.Throwpickup.Post(gameObject);
            Destroy(gameObject);
        }
    }
}
