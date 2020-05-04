using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{

    public float lookRadius = 20f;
    public int Health = 100;
    public bool Dead = false;
    public int WeaponDmg = 7;
    public float ShotDistance = 5.0f;
    private bool canFire = true;
    public float DelayTime = 2;
    public GameObject Weapon;
    public GameObject target;
    public bool seeTarget = false;
    NavMeshAgent agent;
    public GameObject[] LootTable;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Dead != true && target.GetComponent<Player>().Health != 0)
        {
                float distance = Vector3.Distance(target.transform.position, transform.position);
                Vector3 targetDir = target.transform.position - transform.position;

                if (distance <= lookRadius && Vector3.Angle(targetDir, transform.forward) < 80.0f)
                {

                    RaycastHit hit = new RaycastHit();
                    if (Physics.Linecast(transform.position, target.transform.position, out hit))
                    {
                        if (hit.transform.tag == "Player")
                        {
                            seeTarget = true;
                        }
                        else
                        {
                            seeTarget = false;
                        }
                    }

                FaceTarget();

                    if(seeTarget == true)
                    {
                        if (distance <= ShotDistance)
                        {

                            if (canFire == true)
                            {
                                agent.SetDestination(transform.position);
                                Weapon.GetComponent<EnemyWeapon>().Shot(WeaponDmg, target.transform);
                                canFire = false;
                                StartCoroutine(DelaybtwShots());
                            }
                        }
                    }

                    else
                    {
                        agent.SetDestination(target.transform.position);
                    }
                }

        }
    }

    private IEnumerator DelaybtwShots()
    {
        yield return new WaitForSeconds(DelayTime);

        canFire = true;
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }

    void FaceTarget()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);
    }

    public void EnemyHearing(Transform Pos)
    {
        FaceTarget();
        agent.SetDestination(Pos.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }

    public void EnemyHealth(int Damage)
    {
        Health -= Damage;

        if (Health <= 0)
        {
            Health = 0;
            Dead = true;
            agent.enabled = false;

            this.gameObject.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;

            int rand = Random.Range(0, LootTable.Length);
            GameObject instance = (GameObject)Instantiate(LootTable[rand], transform.position, Quaternion.identity);

            StartCoroutine(Despawn());
        }
        if (Health > 100)
        {
            Health = 100;
        }

    }

}
