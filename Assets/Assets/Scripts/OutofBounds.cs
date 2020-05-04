using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutofBounds : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 9)
        {
            collision.transform.position = new Vector3(2.4f, 0.1f, -84f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            collision.transform.position = new Vector3(2.4f, 0.1f, -84f);
        }
    }
}
