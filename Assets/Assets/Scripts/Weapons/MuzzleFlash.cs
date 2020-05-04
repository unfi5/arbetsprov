using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    public float LightTimer;

    public void Start()
    {
        StartCoroutine(TimerLight());
    }

    private IEnumerator TimerLight()
    {
        yield return new WaitForSeconds(LightTimer);
        Destroy(gameObject);
    }
}
