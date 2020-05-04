using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTargetPractise : MonoBehaviour
{
    [SerializeField] Tutorial _tut;

    public void Hit()
    {
        _tut.room2Hits += 1;
    }
}
