using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawns : MonoBehaviour
{
    public GameObject Enemy;
    public LevelGenerator LevelGen;
    public Transform Door;
    public Transform SpawnOne;
    public Transform SpawnTwo;
    public bool spawned = false;

    private bool _ready = false;

    private void Update()
    {
        if (!_ready && LevelGen.randomspawncheck)
        {
            _ready = true;
            Door.position = new Vector3(Door.position.x, 3.5f, Door.position.z);
        }
    }

    public void OpenDoor()
    {
        spawned = true;
        Instantiate(Enemy, SpawnOne.position, Quaternion.identity);
        Instantiate(Enemy, SpawnTwo.position, Quaternion.identity);
        StartCoroutine(DoorOpening());
    }

    IEnumerator DoorOpening()
    {
        float y = 3.5f;
        while (y < 7.9)
        {
            yield return new WaitForSeconds(0.03f);
            Door.position = new Vector3(Door.position.x, Door.position.y + 0.05f, Door.position.z);
            y += 0.05f;
        }
    }
}
