using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject boss;
    public GameObject LockDoorCollider;
    public Transform DoorMesh;
    public GameObject BossSpawnLoc;

    private bool _spawned = false;

    // y0.66 open, 0 closed

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 9)
        {
            if (!_spawned)
            {
                _spawned = true;
                LockDoorCollider.SetActive(true);
                StartCoroutine(DoorClosing());
                Instantiate(boss, BossSpawnLoc.transform);
            }
        }
    }

    IEnumerator DoorClosing()
    {
        float y = 3.2f;
        while(y > 0)
        {
            DoorMesh.position = new Vector3(DoorMesh.position.x, DoorMesh.position.y - 0.03f, DoorMesh.position.z);
            y -= 0.03f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
