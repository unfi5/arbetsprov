using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulletimpact : MonoBehaviour
{
    [SerializeField] SoundManager _sound;

    public bool Blood = false;
    public float DespawnTimer = 3;

    void Awake()
    {
        if(Blood == false)
        {
            _sound.BulletImpacts.Post(gameObject);
        }
        else
        {
            _sound.BulletHit.Post(gameObject);
        }
    }

    private void Update()
    {
        DespawnTimer -= Time.deltaTime;
        if (DespawnTimer < 0)
        {
            Destroy(gameObject);
        }
    }
}
