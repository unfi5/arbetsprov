using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resetlvl : MonoBehaviour
{
    public GameObject Level;

    private void Awake()
    {
        Instantiate(Level, transform.position, Quaternion.identity);
    }
}
