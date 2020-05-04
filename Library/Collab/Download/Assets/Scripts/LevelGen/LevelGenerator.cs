using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    //Generate rooms
    public Transform[] startingPositions;
    public GameObject[] Rooms; // index 0=LR, index 1=LRB, index 2=LRT, index 3=LRBT

    private int Direction;
    public float MoveAmount;

    private float TimeDelay;
    public float StartTime = 0.25f;

    public float MinX;
    public float MaxX;
    public float MinZ;
    public static bool StopGeneration = false;
    private GameObject CurrentRoom;
    private int RoomType;
    private int DownCounter;

    public LayerMask Room;
    public GameObject Player;
    private Vector3 startingLoc;

    public GameObject genCamera;

    //NavMesh
    public NavMeshSurface Surface;

    //Random Spawner
    public Vector3 Center;
    public Vector3 Size;
    public int EnemySpawns;

    public GameObject Enemy;
    public static bool randomspawncheck = false;
    public GameObject EnemySpawners;

    void Start()
    {
        int RandStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[RandStartingPos].position;
        Instantiate(Rooms[0], transform.position, Quaternion.identity);
        startingLoc = transform.position;

        Direction = Random.Range(1, 6);
    }

    void Update()
    {
        if (TimeDelay <= 0 && StopGeneration == false)
        {
            Move();
            TimeDelay = StartTime;
        }
        else
        {
            TimeDelay -= Time.deltaTime;
        }
    }

    private void Move()
    {
        if (Direction == 1 || Direction == 2)
        {

            if (transform.position.x < MaxX)
            {

                DownCounter = 0;

                Vector3 newPos = new Vector3(transform.position.x + MoveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, Rooms.Length);
                Instantiate(Rooms[rand], transform.position, Quaternion.identity);

                Direction = Random.Range(1, 6);
                if (Direction == 3)
                {
                    Direction = 2;
                }
                else if (Direction == 4)
                {
                    Direction = 5;
                } 

            }
            else
            {
                Direction = 5;
            }
        }
        else if (Direction == 3 || Direction == 4)
        {

            if (transform.position.x > MinX)
            {
                DownCounter = 0;

                Vector3 newPos = new Vector3(transform.position.x - MoveAmount, transform.position.y, transform.position.z);
                transform.position = newPos;

                int rand = Random.Range(0, Rooms.Length);
                Instantiate(Rooms[rand], transform.position, Quaternion.identity);

                Direction = Random.Range(3, 6);
            }
            else
            {
                Direction = 5;
            }
        }
        else if (Direction == 5)
        {

            DownCounter++;

            if (transform.position.z > MinZ)
            {

                Collider[] roomDetection = Physics.OverlapBox(gameObject.transform.position, transform.localScale, Quaternion.identity, Room);
                int i = 0;
                while (i < roomDetection.Length)
                {
                    Debug.Log("Hit : " + roomDetection[i].name + i);
                    RoomType = roomDetection[i].GetComponent<RoomType>().Type;

                    if (RoomType != 1 && RoomType != 3)
                    {

                        if (DownCounter >= 2)
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

                Direction = Random.Range(1, 6);
            }
            else
            {
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
        randomspawncheck = true;
        Instantiate(EnemySpawners, new Vector3(0, 0, 0), Quaternion.identity);
        Spawner();
        Spawner();
        Spawner();
        Spawner();
        Spawner();
    }

    void Spawner()
    {

            Vector3 pos = new Vector3(Random.Range(-Size.x / 2, Size.x / 2), 1.5f, Random.Range(-Size.z / 2, Size.z / 2));
            transform.position = pos;

            RaycastHit hitInfo;
            if (Physics.BoxCast(pos + new Vector3(0, 1.0f, 0), new Vector3(0.5f, 0.5f, 0.5f), Vector3.forward, out hitInfo, Quaternion.identity, 1.0f))
            {
                print("Couldnt spawn enemy " + hitInfo.collider.name);              
                if(Physics.BoxCast(pos + new Vector3(2.0f, 1.0f, 0), new Vector3(0.5f, 0.5f, 0.5f), Vector3.forward, out hitInfo, Quaternion.identity, 1.0f))
                {

                    print("Couldnt spawn enemy " + hitInfo.collider.name);
                    if (Physics.BoxCast(pos + new Vector3(2.0f, 1.0f, 2.0f), new Vector3(0.5f, 0.5f, 0.5f), Vector3.forward, out hitInfo, Quaternion.identity, 1.0f))
                    {
                    print("Couldnt spawn enemy " + hitInfo.collider.name);
                    }
                    else
                    {
                    Instantiate(Enemy, pos, Quaternion.identity);
                    }
                }
                else
                {
                    Instantiate(Enemy, pos, Quaternion.identity);
                }
                
            }
            else
            {
                Instantiate(Enemy, pos, Quaternion.identity);
            }

    }

    void spawnPlayer()
    {
        genCamera.GetComponent<GeneratorCamera>().Destroy();
        Instantiate(Player, startingLoc + new Vector3(0.0f, 1.0f, 0.0f), Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 5f);
        Gizmos.DrawCube(Center, Size);

    }

}
