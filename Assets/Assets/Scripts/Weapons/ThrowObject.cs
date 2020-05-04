using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    [SerializeField] SoundManager _sm;

    private Rigidbody _rb;
    private float _velocity;
    private int _PredictionSteps;
    private Vector3 _objVelocity;
    private bool _stop = false;
    private bool _MadeSound = false;
    private float _soundSize = 10f;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _velocity = 13f;
        _PredictionSteps = 6;
        _objVelocity = transform.forward * 13f;
        _sm.Throw.Post(gameObject);
        StartCoroutine(DestroyTimer());
    }

    void Update()
    {
        if (!_stop)
        {
            Vector3 point1 = transform.position;
            float stepSize = 1.0f / _PredictionSteps;
            for (float step = 0; step < 2 && !_stop; step += stepSize)
            {
                _objVelocity += (Physics.gravity + transform.forward) * stepSize * Time.deltaTime;
                Vector3 point2 = point1 + _objVelocity * stepSize * Time.deltaTime;

                Ray ray = new Ray(point1, (point2 - point1));
                if (Physics.Raycast(ray, (point2 - point1).magnitude))
                {
                    if (!_MadeSound)
                    {
                        _rb.mass = 0.3f;
                        _rb.angularDrag = 0.05f;
                        _stop = true;
                        MakeSound(_soundSize);
                        _sm.ThrowCollision.Post(gameObject);
                        _rb.AddForce(transform.forward * 10, ForceMode.Impulse);
                    }
                }

                point1 = point2;
            }
            transform.position = point1;
        }
    }

    private void OnDrawGizmos()
    {
        if (!_stop)
        {
            Gizmos.color = Color.red;
            Vector3 point1 = this.transform.position;
            float stepSize = 0.01f;
            Vector3 tempObjVelocity = _objVelocity;
            for (float step = 0; step < 2; step += stepSize)
            {
                tempObjVelocity += Physics.gravity * stepSize;
                Vector3 point2 = point1 + tempObjVelocity * stepSize;
                Gizmos.DrawLine(point1, point2);
                point1 = point2;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_MadeSound)
        {
            _rb.mass = 0.3f;
            _rb.angularDrag = 0.05f;
            _stop = true;
            _sm.ThrowCollision.Post(gameObject);
            MakeSound(_soundSize);
            _rb.AddForce(transform.forward * 10, ForceMode.Impulse);
        }

        if(collision.gameObject.layer == 9)
        {
            collision.transform.GetComponent<Player>().ThrowAmmo += 1;
            _sm.Throwpickup.Post(gameObject);
            Destroy(gameObject);
        }

    }

    public void MakeSound(float SoundSize)
    {
        Vector3 Size = new Vector3(SoundSize, SoundSize, SoundSize);
        Collider[] enemyDetection = Physics.OverlapSphere(gameObject.transform.position, SoundSize);
        int i = 0;
        while (i < enemyDetection.Length)
        {

            if (enemyDetection[i].tag == "Enemy" && enemyDetection[i].GetComponent<Enemy>() != null)
            {
                enemyDetection[i].GetComponent<Enemy>().EnemyHearing(this.gameObject.transform);
            }
            else if (enemyDetection[i].tag == "Enemy" && enemyDetection[i].GetComponent<TutorialEnemy>() != null)
            {
                enemyDetection[i].GetComponent<TutorialEnemy>().Throwable(transform);
            }

            i++;
        }
        _MadeSound = true;
    }

    IEnumerator DestroyTimer()
    {
        yield return new WaitForSeconds(20);

        Destroy(gameObject);
    }
}
