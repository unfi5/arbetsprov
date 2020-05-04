using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [SerializeField] SoundManager _sm;

    public int ammoAmount = 15;
    public bool Pistol = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if(Pistol) collision.transform.GetComponent<Player>().AmmoPickup(ammoAmount, Player.Weapons.Pistol, gameObject);
            else collision.transform.GetComponent<Player>().AmmoPickup(ammoAmount, Player.Weapons.AssualtRifle, gameObject);
            _sm.AmmoPickup.Post(gameObject);            
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
