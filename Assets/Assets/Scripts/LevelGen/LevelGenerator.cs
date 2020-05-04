using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public Transform[] startingPositions;
    public Transform[] BossPositions;
    public Transform PlayerSpawn;
    public GameObject[] Rooms; // index 0=LR, index 1=LRB, index 2=LRT, index 3=LRBT
    public GameObject BossRoom;
    public GameObject Boss;
    public float MoveAmount;
    public float StartTime = 0.25f;
    public float MinX;
    public float MaxX;
    public float MinZ;
    public bool StopGeneration = false;
    public LayerMask Room;
    public GameObject Player;
    public GameObject genCamera;
    public NavMeshSurface Surface;
    public Vector3 Center;
    public Vector3 Size;
    public int EnemySpawns;
    public GameObject Enemy;
    public bool randomspawncheck = false;
    public GameObject EnemySpawners;
    public bool SpawnedPlayer;

    private Vector3 _BossPos;
    private int _Direction;
    private float _TimeDelay;
    private GameObject _CurrentRoom;
    private int _RoomType;
    private int _DownCounter;
    private Vector3 _startingLoc;

    void Start()
    {
        int RandStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[RandStartingPos].position;
        Instantiate(Rooms[0], transform.position, Quaternion.identity);
        _startingLoc = transform.position;

        _Direction = Random.Range(1, 6);
    }

    void Update()
    {
        if (_TimeDelay <= 0 && StopGeneration == false)
        {
            Move();
            _TimeDelay = StartTime;
        }
        else
        {
            _TimeDelay -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (_Direction == 1 || _Direction == 2)
        {

            if (transform.position.x < MaxX)
            {

                _DownCounter = 0;

                Vector3 newPos = new Vector3(transform.position.x + MoveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, Rooms.Length);
                Instantiate(Rooms[rand], transform.position, Quaternion.identity);

                _Direction = Random.Range(1, 6);
                if (_Direction == 3)
                {
                    _Direction = 2;
                }
                else if (_Direction == 4)
                {
                    _Direction = 5;
                } 

            }
            else
            {
                _Direction = 5;
            }
        }
        else if (_Direction == 3 || _Direction == 4)
        {

            if (transform.position.x > MinX)
            {
                _DownCounter = 0;

                Vector3 newPos = new Vector3(transform.position.x - MoveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, Rooms.Length);
                Instantiate(Rooms[rand], transform.position, Quaternion.identity);

                _Direction = Random.Range(3, 6);
            }
            else
            {
                _Direction = 5;
            }
        }
        else if (_Direction == 5)
        {

            _DownCounter++;

            if (transform.position.z > MinZ)
            {

                Collider[] roomDetection = Physics.OverlapBox(gameObject.transform.position, transform.localScale, Quaternion.identity, Room);
                int i = 0;
                while (i < roomDetection.Length)
                {
                    Debug.Log("Hit : " + roomDetection[i].name + i);
                    _RoomType = roomDetection[i].GetComponent<RoomType>().Type;

                    if (_RoomType != 1 && _RoomType != 3)
                    {

                        if (_DownCounter >= 2)
                        {
                            roomDetection[i].GetComponent<RoomType>().DestroyRoom();
                            Instantiate(Rooms[3], transform.position, Quaternion.identity);                          
                        }

                        else
                        {
                            roomDetection[i].GetComponent<RoomType>().DestroyRoom();
                            Debug.Log("Destroy room");

                            int RandomBottomRoom = Random.Range(1, 4);
                            if (RandomBottomRoom == 2)
                            {
                                RandomBottomRoom = 1;
                            }
                            Instantiate(Rooms[RandomBottomRoom], transform.position, Quaternion.identity);
                        }
                    }

                    i++;
                }

                Vector3 newPos = new Vector3(transform.position.x, transform.position.y, transform.position.z - MoveAmount); // for horizontal z - moveamount
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(Rooms[rand], transform.position, Quaternion.identity);

                _Direction = Random.Range(1, 6);
            }
            else
            {
                //PlaceBoss Room
                int RandBossPos = Random.Range(0, BossPositions.Length);
                transform.position = BossPositions[RandBossPos].position;

                Collider[] roomDetection = Physics.OverlapBox(gameObject.transform.position, transform.localScale, Quaternion.identity, Room);
                int i = 0;
                while (i < roomDetection.Length)
                {
                    Debug.Log("Boss Room Hit : " + roomDetection[i].name + i);
                    _RoomType = roomDetection[i].GetComponent<RoomType>().Type;

                    if (_RoomType > 0)
                    {
                        roomDetection[i].GetComponent<RoomType>().DestroyRoom();
                        Instantiate(BossRoom, BossPositions[RandBossPos].position, Quaternion.identity);
                    }

                    i++;
                }
                if (i == 0)
                {
                    Instantiate(BossRoom, BossPositions[RandBossPos].position, Quaternion.identity);
                }

                //Position of Boss
                _BossPos = (BossPositions[RandBossPos].position + new Vector3(3, 4, 0));

                //Stop Level Generator!
                StopGeneration = true;
                StartCoroutine(DelayafterGen());
            }

        }

    }

    private IEnumerator DelayafterGen()
    {
        yield return new WaitForSeconds(2.0f);

        NavMesh();
    }

    void NavMesh()
    {
        Surface.BuildNavMesh();
        spawnPlayer();
        SpawnedPlayer = true;
        randomspawncheck = true;
        Instantiate(Boss, _BossPos, Quaternion.identity);
        Instantiate(EnemySpawners, new Vector3(0, 0, 0), Quaternion.identity);
    }

    void spawnPlayer()
    {
        genCamera.GetComponent<GeneratorCamera>().Destroy();
        Instantiate(Player, PlayerSpawn.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 5f);
        Gizmos.DrawCube(Center, Size);

    }

}
