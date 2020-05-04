using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    #region Varibles
    [SerializeField] SoundManager _sm;

    public int ClipSize = 30;
    public int MaxAmmo = 300;
    public float ShotDelay = 0.2f;
    public GameObject Player;
    public GameObject BulletHole;
    public GameObject Bloodeffect;
    public Transform BulletExitPoint;
    public AK.Wwise.Event GunShot;
    public GameObject MuzzleFlashing;
    public float SpreadAmount = 0.05f;
    public float aSpread = 0;

    public struct InputStates
    {
        public bool FireBotton;
        public bool ReloadButtonDown;
    }

    InputStates _input;

    private float _DelayTime = 0.05f;
    private bool _isReloading;
    private float _ReloadTime;
    private bool _canFire = true;
    private int _WeaponDmg = 30;
    private Vector3 _wSpread;
    private Player _player;
    private Animator _Anim;
    #endregion

    private void Awake()
    {
        _Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        UpdateAnimClipTimes();
        _player = Player.GetComponent<Player>();
    }

    void Update()
    {
        if (_player.Health <= 0) return;

        SetInput();

        if(aSpread > 0.1f)
        {
            aSpread = 0.1f;
        }

        if (aSpread > 0)
        {
            aSpread -= 0.120f * Time.deltaTime;
        }
        else
        {
            aSpread = 0;
            _wSpread = new Vector3(0, 0, 0);
        }

    }

    void SetInput()
    {
        _input.FireBotton = Input.GetButton("Fire1");
        _input.ReloadButtonDown = Input.GetKeyDown(KeyCode.R);

        //Fire input
        if (_input.FireBotton)
        {
            if (_isReloading == true)
            {
                _isReloading = false;
                _Anim.Play("Idle");
            }

            //Fire weapon
            if (_player.AssaultRifle.MagazineBullets > 0 && _canFire == true)
            {
                _Anim.Play("Idle");
                _player.AssaultRifle.MagazineBullets -= 1;
                GunShot.Stop(gameObject);
                GunShot.Post(gameObject);
                _Anim.Play("AssaultRifle_Fire");
                _player.Shot(50f);
                _canFire = false;
                StartCoroutine(DelaybtwShots());

                var fwd = transform.TransformDirection(Vector3.forward);
                RaycastHit hit = new RaycastHit();

                MuzzleFlash();

                //Raycast
                if (Physics.Raycast(transform.position, fwd + _wSpread, out hit, 70))
                {
                    if (hit.collider.gameObject.layer == 12)
                    {
                        if (hit.collider.GetComponent<EnemyHead>() != null)
                        {
                            hit.transform.GetComponentInChildren<EnemyHead>().EnemyHealth(_WeaponDmg, transform);
                        }
                        else if (hit.transform.GetComponent<Enemy>() != null)
                        {
                            hit.transform.GetComponent<Enemy>().EnemyHealth(_WeaponDmg, transform);
                        }

                        Instantiate(Bloodeffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    }
                    else if (hit.collider.gameObject.layer == 18)
                    {
                        if (hit.collider.GetComponent<BossHead>() != null)
                        {
                            hit.transform.GetComponentInChildren<BossHead>().EnemyHealth(_WeaponDmg, transform);
                        }
                        else if (hit.transform.GetComponent<Boss>() != null)
                        {
                            hit.transform.GetComponent<Boss>().EnemyHealth(_WeaponDmg);
                        }

                        Instantiate(Bloodeffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    }
                    else if (hit.collider.transform.gameObject.layer == 16 && hit.transform.GetComponent<Rigidbody>() != null)
                    {
                        var curBullethole = Instantiate(BulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                        curBullethole.transform.parent = hit.transform;
                        hit.rigidbody.AddForceAtPosition(transform.forward * 10f, hit.point, ForceMode.Impulse);
                    }
                    else if (hit.collider.tag == "Rocket")
                    {
                        hit.transform.GetComponent<Rocket>().Explode();
                    }
                    else if (hit.collider.tag == "Tutorial")
                    {
                        if (hit.collider.GetComponent<TutorialTargetPractise>() != null)
                        {
                            hit.collider.GetComponent<TutorialTargetPractise>().Hit();
                            Instantiate(BulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                        }
                        else if (hit.collider.GetComponent<TutorialEHead>() != null)
                        {
                            hit.collider.GetComponent<TutorialEHead>().EnemyHealth(_WeaponDmg, transform);
                            Instantiate(Bloodeffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                        }
                        else
                        {
                            hit.transform.GetComponent<TutorialCaptain>().EnemyHealth(_WeaponDmg, transform);
                            Instantiate(Bloodeffect, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                        }
                    }
                    else
                    {
                        Instantiate(BulletHole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
                    }

                    //add bullet spread
                    aSpread += SpreadAmount;
                    _wSpread = new Vector3(Random.Range(-aSpread, aSpread), Random.Range(-aSpread, aSpread), Random.Range(-aSpread, aSpread));
                }
            }
        }

        if (_input.ReloadButtonDown && _player.AssaultRifle.MagazineBullets < ClipSize && !_isReloading && _player.AssaultRifle.Ammo > 0)
        {
            _isReloading = true;
            _Anim.Play("Idle");
            _Anim.Play("AssaultRifle_Reload");
            StartCoroutine(Timer());
        }

    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = _Anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "AssaultRifle_Reload":
                    _ReloadTime = clip.length;
                    break;
            }
        }
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(_ReloadTime);

        if (_isReloading == true)
        {
            Reload();
        }
    }

    private IEnumerator DelaybtwShots()
    {
        yield return new WaitForSeconds(_DelayTime);

        _canFire = true;
    }

    private void Reload()
    {
        int AmmoDiff;
        AmmoDiff = ClipSize - _player.AssaultRifle.MagazineBullets;

        if (AmmoDiff < _player.AssaultRifle.Ammo)
        {
            _player.AssaultRifle.Ammo -= AmmoDiff;
            _player.AssaultRifle.MagazineBullets = ClipSize;
            _isReloading = false;
        }
        else
        {
            _player.AssaultRifle.MagazineBullets += _player.AssaultRifle.Ammo;
            _player.AssaultRifle.Ammo -= _player.AssaultRifle.Ammo;
            _isReloading = false;
        }
    }

    public void ReloadSound1()
    {
        _sm.ARReload1.Post(gameObject);
    }

    public void ReloadSound2()
    {
        _sm.ARReload2.Post(gameObject);
    }

    public void MuzzleFlash()
    {
        Instantiate(MuzzleFlashing, BulletExitPoint.position, Quaternion.identity);
    }
}
