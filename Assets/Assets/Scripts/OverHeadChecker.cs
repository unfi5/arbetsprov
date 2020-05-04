using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverHeadChecker : MonoBehaviour
{
    public bool standupCheck = true;
    public bool inDelay = false;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 9)
        {
            standupCheck = false;
            inDelay = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        inDelay = true;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        if(inDelay == true)
        {
            standupCheck = true;
        }
    }

}
