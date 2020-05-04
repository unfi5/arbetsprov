using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyCards : MonoBehaviour
{
    [SerializeField] SoundManager _sm;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            other.transform.GetComponent<Player>().KeyCards += 1;
            _sm.KeycardPickup.Post(gameObject);
            other.transform.GetComponentInChildren<HelpSubtitles>().HelpText("You've gained a Keycard!", 3f);
            Destroy(gameObject);
        }
    }
}
