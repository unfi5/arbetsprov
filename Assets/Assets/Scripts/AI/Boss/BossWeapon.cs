using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    [SerializeField] SoundManager _sm;

    public GameObject BulletPrefab;
    public Transform BulletSpawn;

    private float _BulletSpeed = 100;
    private float _BulletLifeTime = 1;
    private int _layerMask = ~(1 << 12);

    float _WeaponSpread = 0.1f;
    float _randf;
    float _xrand;
    float _yrand;
    float _zrand;

    public void Shot(int WeaponDmg, Transform target)
    {
        RaycastHit hit = new RaycastHit();
        RandomizeVector3();
        var fwd = transform.TransformDirection(Vector3.forward + new Vector3(_xrand, _yrand, _zrand));

        if (Physics.Raycast(transform.position, fwd, out hit, 40, _layerMask))
        {
            GameObject Bullet = Instantiate(BulletPrefab);
            Bullet.transform.position = BulletSpawn.position;
            Vector3 rotation = Bullet.transform.rotation.eulerAngles;
            Bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            Bullet.GetComponent<Rigidbody>().AddForce(fwd * _BulletSpeed, ForceMode.Impulse);
            StartCoroutine(DestroyBullet(Bullet, _BulletLifeTime));

            _sm.EnemyShot.Post(gameObject);
            if (hit.transform.GetComponent<Player>() != null)
            {
                var angleY = Mathf.Asin(Vector3.Cross(transform.forward, hit.transform.forward).y) * Mathf.Rad2Deg;
                hit.transform.GetComponent<Player>().DamageAngle(angleY);
                hit.transform.GetComponent<Player>().PlayerHealth(WeaponDmg);
            }
        }
    }

    private IEnumerator DestroyBullet(GameObject Bullet, float Delay)
    {
        yield return new WaitForSeconds(Delay);

        Destroy(Bullet);
    }

    void RandomizeVector3()
    {
        randomizer();
        _xrand = _randf;
        randomizer();
        _yrand = _randf;
        randomizer();
        _zrand = _randf;
    }

    void randomizer()
    {
        float rand = Random.Range(-16, 16);
        if (rand > 5 || rand < -5)
        {
            rand = 0;
        }

        _randf = (_WeaponSpread * rand);
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);
    }
}
