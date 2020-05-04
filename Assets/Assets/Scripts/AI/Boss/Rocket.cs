using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] GameObject Boss;
    [SerializeField] GameObject Explosion;

    public float ExplosionSize = 3;
    public int ExplosionDamage = 20;
    public float ExplosionForce = 5;

    private GameObject _target;
    private Rigidbody _rb;
    private Player _player;
    private float _RocketSpeed = 5f;

    private void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _rb = GetComponent<Rigidbody>();
        _player = _target.GetComponent<Player>();
    }

    void Update()
    {
        FaceTarget(_target.transform);
        _rb.MovePosition(transform.position + (transform.forward * _RocketSpeed * Time.deltaTime));
    }


    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag != "Boss")
        {
            Explode();
        }
    }

    public void Explode()
    {
        Collider[] ExplosionDetection = Physics.OverlapSphere(gameObject.transform.position, ExplosionSize);
        int i = 0;
        while (i < ExplosionDetection.Length)
        {

            if (ExplosionDetection[i].gameObject.layer == 9)
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Linecast(transform.position, _target.transform.position, out hit))
                {
                    if (hit.collider.gameObject.layer == 9)
                    {
                        _player.PlayerHealth(ExplosionDamage);
                    }
                }
            }
            else if(ExplosionDetection[i].transform.gameObject.layer == 16 && ExplosionDetection[i].GetComponent<Rigidbody>() != null)
            {
                ExplosionDetection[i].GetComponent<Rigidbody>().AddExplosionForce(ExplosionForce, transform.position, ExplosionSize);
            }

            i++;
        }
        Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
