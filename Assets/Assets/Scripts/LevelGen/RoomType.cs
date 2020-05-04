using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType : MonoBehaviour
{
    public int Type;

    public void DestroyRoom()
    {
        Destroy(gameObject);
    }
}
