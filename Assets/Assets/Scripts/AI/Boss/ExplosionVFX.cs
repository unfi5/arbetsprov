using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
