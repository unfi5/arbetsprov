using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AK.Wwise.Event Footstep;

    public AK.Wwise.Event PistolShot;
    public AK.Wwise.Event PistolReload;

    public AK.Wwise.Event ARShot;
    public AK.Wwise.Event ARReload1;
    public AK.Wwise.Event ARReload2;

    public AK.Wwise.Event PlayerHurt;
    public AK.Wwise.Event EnemyHurt;

    public AK.Wwise.Event EnemyShot;
    public AK.Wwise.Event EnemyNotice;
    public AK.Wwise.Event EnemySearch;

    public AK.Wwise.Event Throw;
    public AK.Wwise.Event ThrowCollision;

    public AK.Wwise.Event BulletImpacts;
    public AK.Wwise.Event BulletHit;

    public AK.Wwise.Event DoorOpen;
    public AK.Wwise.Event DoorClose;
    public AK.Wwise.Event GateOpen;

    public AK.Wwise.Event AmmoPickup;
    public AK.Wwise.Event HealthPickup;
    public AK.Wwise.Event KeycardPickup;
    public AK.Wwise.Event Throwpickup;
    public AK.Wwise.Event WeaponPickup;

    public AK.Wwise.Event Ambience;
    public AK.Wwise.Event Music;

    private void Start()
    {
        Ambience.Post(gameObject);
        Music.Post(gameObject);
    }
}
