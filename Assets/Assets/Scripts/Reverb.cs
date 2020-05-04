using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reverb : MonoBehaviour, IpooledObject
{
    public float DelayTime;
    public float Distance;

    private float _VolumeLoss = 1.8f;
    private float _VolumeDecrease;
    private float _Absorption;

    public void OnObjectSpawn()
    {
        Distance = Player.ReverbDistance;
        _Absorption = Player.MaterialAbsorption;
        DelayTime = Distance / 343;
        _VolumeDecrease = (_VolumeLoss + Distance)/3;

        if (DelayTime < 4.0f)
        {
            StartCoroutine(Timer());
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(DelayTime);

        PlaySound();
    }

    private void PlaySound()
    {
        AkSoundEngine.SetRTPCValue("Absorption", _Absorption, gameObject);
        AkSoundEngine.SetRTPCValue("Reflection", -1 * _VolumeDecrease, gameObject);
        Player.ReverbEvent.Post(gameObject);
    }
}
