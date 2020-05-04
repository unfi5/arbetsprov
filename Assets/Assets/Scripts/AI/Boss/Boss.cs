using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] BossHead _bossHead;
    [SerializeField] BossWeapon _bossWeapon;

    public GameObject RocketPf;
    public GameObject LeftRocketLauncher;
    public GameObject RightRocketLauncher;
    public int Health = 1000;
    public int WeaponDmg;
    public bool fireMode = false;
    public bool seeTarget = false;
    public bool Dead = false;
    public bool waiting = false;

    NavMeshAgent _agent;
    private BossSpawns _rightDoor;
    private BossSpawns _leftDoor;
    private GameObject _target;
    private Player _player;
    private int _maxHealth;
    private TextMesh _Vurnable;

    void Awake()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _player = _target.GetComponent<Player>();
        _agent = GetComponent<NavMeshAgent>();
        _rightDoor = GameObject.Find("RightEnemySpawnDoor").GetComponent<BossSpawns>();
        _leftDoor = GameObject.Find("LeftEnemySpawnDoor").GetComponent<BossSpawns>();
        _maxHealth = Health;
        _Vurnable = GetComponentInChildren<TextMesh>();
        _Vurnable.color = new Vector4(_Vurnable.color.r, _Vurnable.color.g, _Vurnable.color.b, 0);
        Health = 1000;
    }

    void Update()
    {
        if (Dead) return;
        FaceTarget();

        RaycastHit hit = new RaycastHit();
        if (Physics.Linecast(transform.position, _target.transform.position, out hit))
        {
            if (hit.collider.gameObject.layer == 9)
            {
                seeTarget = true;
            }
            else
            {
                seeTarget = false;
            }
        }

        if (!seeTarget && !waiting)
        {
            _agent.SetDestination(_target.transform.position);
        }
        else if (!fireMode)
        {
            StartCoroutine(Shot());
        }       
    }

    public void EnemyHealth(int Damage)
    {
        if (waiting)
        {
            Health -= Damage;
        }
        else
        {
            Health -= Damage / 10;
        }

        if (Health <= 0)
        {
            Health = 0;

            if (!Dead)
            {
                Dead = true;
                _agent.enabled = false;

                this.gameObject.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);
                StartCoroutine(_player.Win());
            }

        }

        if(Health <= 333 && !_leftDoor.spawned)
        {
            _leftDoor.OpenDoor();
        }
        else if(Health <= 666 && !_rightDoor.spawned)
        {
            _rightDoor.OpenDoor();
        }

        if (Health > _maxHealth)
        {
            Health = _maxHealth;
        }
    }

    IEnumerator Shot()
    {
        fireMode = true;
        _bossWeapon.Shot(WeaponDmg, _target.transform);
        yield return new WaitForSeconds(0.1f);
        _bossWeapon.Shot(WeaponDmg, _target.transform);
        yield return new WaitForSeconds(0.1f);
        _bossWeapon.Shot(WeaponDmg, _target.transform);
        yield return new WaitForSeconds(3.0f);
        ShotLeftRocket();
        yield return new WaitForSeconds(0.5f);
        ShotRightRocket();
        yield return new WaitForSeconds(0.1f);
        _bossWeapon.Shot(WeaponDmg, _target.transform);
        yield return new WaitForSeconds(0.1f);
        _bossWeapon.Shot(WeaponDmg, _target.transform);
        yield return new WaitForSeconds(0.1f);
        _bossWeapon.Shot(WeaponDmg, _target.transform);
        yield return new WaitForSeconds(3.0f);
        ShotLeftRocket();
        yield return new WaitForSeconds(0.5f);
        ShotRightRocket();

        waiting = true;
        _Vurnable.color = new Vector4(_Vurnable.color.r, _Vurnable.color.g, _Vurnable.color.b, 1);
        yield return new WaitForSeconds(5.0f);
        waiting = false;
        _Vurnable.color = new Vector4(_Vurnable.color.r, _Vurnable.color.g, _Vurnable.color.b, 0);

        fireMode = false;
    }   

    void ShotLeftRocket()
    {
        GameObject Rocket = Instantiate(RocketPf, LeftRocketLauncher.transform.position, Quaternion.identity);
    }

    void ShotRightRocket()
    {
        GameObject Rocket = Instantiate(RocketPf, RightRocketLauncher.transform.position, Quaternion.identity);
    }

    void FaceTarget()
    {
        _bossHead.FaceTarget(_target.transform);
        _bossWeapon.FaceTarget(_target.transform);
        Vector3 direction = (_target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);
    }
}
