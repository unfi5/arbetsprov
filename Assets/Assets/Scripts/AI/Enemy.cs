using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] SoundManager _sm;

    public float lookRadius = 30f;
    public int Health = 100;
    public bool Dead = false;
    public float DelayTime = 1;
    public GameObject target;
    public bool seeTarget = false;
    public GameObject[] LootTable;
    public bool BodyNotice = false;

    TextMesh notice;
    bool eNotice = false;
    bool waiting = false;

    NavMeshAgent agent;
    private Player _player;
    private EnemyHead _ehead;
    private EnemyWeapon _ewep;
    private Rigidbody _rb;
    private CapsuleCollider _cc;
    private BoxCollider _bc;
    private EnemyDeadBody _edb;
    private int _WeaponDmg = 15;
    private float _ShotDistance = 20.0f;
    private bool _canFire = true;
    private Vector3 _ogPos;
    private Vector3 _Body;
    private float _aggro = 0;
    private float _oglookRadius;
    private bool _goback;
    private bool _lookAtTarget;

    void Awake()
    {
        notice = GetComponentInChildren<TextMesh>();
        notice.color = new Vector4(notice.color.r, notice.color.g, notice.color.b, 0.0f);
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        _ogPos = transform.position;
        _ehead = GetComponentInChildren<EnemyHead>();
        _player = target.GetComponent<Player>();
        _ewep = GetComponentInChildren<EnemyWeapon>();
        _cc = GetComponent<CapsuleCollider>();
        _rb = GetComponent<Rigidbody>();
        _bc = GetComponentInChildren<BoxCollider>();
        _edb = GetComponentInChildren<EnemyDeadBody>();
        _oglookRadius = lookRadius;
    }

    void Update()
    {
        if (Dead || _player.Health <= 0) return;

        if (seeTarget == true && eNotice == false) EnemyNotice();

        if (_lookAtTarget) FaceTarget();

        float distance = Vector3.Distance(target.transform.position, transform.position);
        Vector3 targetDir = target.transform.position - transform.position;

        if (distance <= lookRadius && Vector3.Angle(targetDir, transform.forward) < 45.0f)
        {

            RaycastHit hit = new RaycastHit();
            if (Physics.Linecast(_ewep.transform.position, target.transform.position, out hit))
            {
                if (!seeTarget) _aggro += 1 * Time.deltaTime;

                if (hit.collider.gameObject.layer == 9 && _aggro >= 5) seeTarget = true;

                else seeTarget = false;
            }

            FaceTarget();

            if (seeTarget && distance <= _ShotDistance && _canFire)
            {
                agent.SetDestination(transform.position);
                _ewep.Shot(_WeaponDmg, target.transform);
                _canFire = false;
                StartCoroutine(DelaybtwShots());
                MakeSound(10f);
            }
            else
            {
                agent.SetDestination(target.transform.position);
                eNotice = false;
            }
        }
        else if (BodyNotice) StartCoroutine(SearchAround());

        else if(!seeTarget && !waiting)
        {
            waiting = true;
            StartCoroutine(wander());
        }
    }

    private IEnumerator SearchAround()
    {
        yield return new WaitForSeconds(2f);

        if (!seeTarget && !Dead)
        {
            if (!_goback)
            {
                agent.SetDestination(_Body + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
                _goback = true;
                waiting = false;
            }
            else
            {
                agent.SetDestination(_ogPos);
                _goback = false;
            }
        }
    }

    private IEnumerator wander()
    {
        yield return new WaitForSeconds(5f);

        if (!seeTarget && !Dead)
        {
            agent.SetDestination(_ogPos + new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5)));
            waiting = false;
        }
    } 

    private IEnumerator DelaybtwShots()
    {
        yield return new WaitForSeconds(DelayTime);

        _canFire = true;
    }

    private IEnumerator LookAtTarget()
    {
        _lookAtTarget = true;
        yield return new WaitForSeconds(3);

        _lookAtTarget = false;
    }

    private IEnumerator Despawn()
    {
        yield return new WaitForSeconds(10);

        Destroy(gameObject);
    }

    void FaceTarget()
    {
        _ehead.FaceTarget(target.transform);
        _ewep.FaceTarget(target.transform);
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 7f);
    }

    public void EnemyHearing(Transform Pos)
    {
        StartCoroutine(LookAtTarget());
        if(eNotice == false)
        {
            EnemySearch();
        }
        agent.SetDestination(Pos.position);
        _ehead.FaceTarget(Pos);
    }

    public void MakeSound(float SoundSize)
    {
        Vector3 Size = new Vector3(SoundSize, SoundSize, SoundSize);
        Collider[] enemyDetection = Physics.OverlapSphere(gameObject.transform.position, SoundSize);
        int i = 0;
        while (i < enemyDetection.Length)
        {

            if (enemyDetection[i].gameObject.layer == 12 && enemyDetection[i].GetComponent<Enemy>() != null)
            {
                enemyDetection[i].GetComponent<Enemy>().EnemyHearing(this.gameObject.transform);
            }

            i++;
        }
    }

    public void EnemyHealth(int Damage, Transform Pos)
    {
        Health -= Damage;
        if (!seeTarget)
        {
            seeTarget = true;
            _aggro += 4;

            if(lookRadius == _oglookRadius)
            {
                _ShotDistance += _ShotDistance;
                lookRadius += lookRadius;
            }

            StartCoroutine(LookAtTarget());
            agent.SetDestination(target.transform.position);
        }

        if(Damage > 0)
        {
            _sm.EnemyHurt.Post(gameObject);
        }

        if (Health <= 0)
        {
            Health = 0;

            if (!Dead)
            {
                Dead = true;
                agent.enabled = false;

                this.gameObject.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);

                _rb.isKinematic = true;
                _rb.constraints = RigidbodyConstraints.FreezeAll;
                _cc.enabled = false;
                _bc.enabled = false;
                _edb.enabled = true;

                int rand = Random.Range(0, LootTable.Length);
                GameObject instance = (GameObject)Instantiate(LootTable[rand], transform.position, Quaternion.identity);

                StartCoroutine(Despawn());
            }

        }
        if (Health > 100)
        {
            Health = 100;
        }
    }

    public void EnemyNotice()
    {
        if(Health > 0)
        {
            eNotice = true;
            notice.text = "!";
            notice.color = new Vector4(notice.color.r, notice.color.g, notice.color.b, 1.0f);
            _sm.EnemyNotice.Post(gameObject);
            StartCoroutine(NoticeDelay());
        }
    }

    public void EnemySearch()
    {
        if (Health > 0)
        {
            notice.text = "?";
            notice.color = new Vector4(notice.color.r, notice.color.g, notice.color.b, 1.0f);
            _sm.EnemySearch.Post(gameObject);
            StartCoroutine(NoticeDelay());
        }
    }

    private IEnumerator NoticeDelay()
    {
        yield return new WaitForSeconds(1f);
        if(Health > 0)
        {
            StartCoroutine(NoticeFadeTimer());
        }
        else
        {
            notice.color = new Vector4(notice.color.r, notice.color.g, notice.color.b, 0.0f);
        }
    }

    public void NoticeBody(Transform body)
    {
        _Body = body.position;

        RaycastHit hit = new RaycastHit();
        if (Physics.Linecast(transform.position, _Body, out hit))
        {
            if(hit.collider.gameObject.layer == 12 && Health > 0)
            {
                EnemySearch();
                BodyNotice = true;
                agent.SetDestination(_Body);
            }
        }
    }

    private IEnumerator NoticeFadeTimer()
    {
        float Fade = 1;
        while (Fade > 0)
        {
            yield return new WaitForSeconds(0.1f);
            Fade -= 0.1f;

            if(Health > 0)
            {
                notice.color = new Vector4(notice.color.r, notice.color.g, notice.color.b, Fade);
            }
            else
            {
                notice.color = new Vector4(notice.color.r, notice.color.g, notice.color.b, 0.0f);
            }
        }
    }
}
