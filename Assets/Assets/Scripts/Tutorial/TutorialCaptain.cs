using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TutorialCaptain : MonoBehaviour
{
    public int Health = 100;
    public bool Dead = false;
    public GameObject[] LootTable;

    private Rigidbody _rb;
    private CapsuleCollider _cc;
    private BoxCollider _bc;
    TutorialEHead _head;
    NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _cc = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
        _bc = GetComponentInChildren<BoxCollider>();
        _head = GetComponentInChildren<TutorialEHead>();
    }

    public void EnemyHealth(int Damage, Transform Pos)
    {
        Health -= Damage;
        _head.FaceTarget(Pos);
        _agent.SetDestination(Pos.position);

        if (Health <= 0)
        {
            Health = 0;
            Dead = true;
            _agent.enabled = false;

            this.gameObject.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);

            _rb.isKinematic = true;
            _rb.constraints = RigidbodyConstraints.FreezeAll;
            _cc.enabled = false;
            _bc.enabled = false;

            int rand = Random.Range(0, LootTable.Length);
            GameObject instance = (GameObject)Instantiate(LootTable[rand], transform.position, Quaternion.identity);

            StartCoroutine(Despawn());
        }
        if (Health > 100)
        {
            Health = 100;
        }

    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }
}
