using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] SoundManager _sm;
    [SerializeField] ParticleSystem _weaponVFX;

    public GameObject Enemy;
    public GameObject BulletPrefab;
    public Transform BulletSpawn;

    private Enemy _enemy;
    private Player _player;
    private float _BulletSpeed = 100;
    private float _BulletLifeTime = 1;
    private int _layerMask = ~(1 << 12);

    float _WeaponSpread = 0.1f;
    float _randf;
    float _xrand;
    float _yrand;
    float _zrand;

    private void Awake()
    {
        _enemy = Enemy.GetComponent<Enemy>();
        _player = _enemy.target.GetComponent<Player>();
        _weaponVFX.Stop();
    }

    public void Shot(int WeaponDmg, Transform target)
    {

        RaycastHit hit = new RaycastHit();
        Setxyz();
        var fwd = transform.TransformDirection(Vector3.forward + new Vector3 (_xrand, _yrand, _zrand));
        _weaponVFX.Play();

        if (Physics.Raycast(transform.position, fwd, out hit, 60, _layerMask))
        {
            GameObject Bullet = Instantiate(BulletPrefab);
            Bullet.transform.position = BulletSpawn.position;
            Vector3 rotation = Bullet.transform.rotation.eulerAngles;
            Bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
            Bullet.GetComponent<Rigidbody>().AddForce(fwd * _BulletSpeed, ForceMode.Impulse);
            StartCoroutine(DestroyBullet(Bullet, _BulletLifeTime));

            _sm.EnemyShot.Post(gameObject);
            if (hit.collider.gameObject.layer == 9)
            {
                var angleY = Mathf.Asin(Vector3.Cross(transform.forward, hit.transform.forward).y) * Mathf.Rad2Deg;
                _player.DamageAngle(angleY);
                _player.PlayerHealth(WeaponDmg);
            }
        }
    }

    public void FaceTarget(Transform target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);
    }

    private IEnumerator DestroyBullet(GameObject Bullet, float Delay)
    {
        yield return new WaitForSeconds(Delay);

        Destroy(Bullet);
    }

    public void EnemyHealth(int Damage)
    {
        _enemy.EnemyHealth(Damage, transform);
    }

    void Setxyz()
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
}
