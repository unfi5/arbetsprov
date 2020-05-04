using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    #region Varibles
    [SerializeField] SoundManager _sm;

    public int ClipSize = 6;
    public int MaxAmmo = 300;
    public float ShotDelay = 2.0f;
    public GameObject BulletHole;
    public GameObject Bloodeffect;
    public GameObject MuzzleFlash;
    public Transform BulletExitPoint;
    public AK.Wwise.Event GunShot;
    public float SpreadAmount = 0.05f;
    public float Spread = 0;

    public struct InputStates
    {
        public bool FireBottonDown;
        public bool ReloadButtonDown;
    }

    InputStates _input;

    private GameObject Player;
    private float _DelayTime = 0.2f;
    private bool _isReloading;
    private float _ReloadTime = 3.0f;
    private bool _canFire = true;
    private Vector3 _wSpread;
    private int _WeaponDmg = 40;
    private Player _player;
    private Animator _Anim;
    #endregion

    private void Awake()
    {
        _Anim = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        _player = Player.GetComponent<Player>();
        UpdateAnimClipTimes();
    }

    void Update()
    {
        if (_player.Health <= 0) return;

        GetInput();

        if (Spread > 0.1f)
        {
            Spread = 0.1f;
        }

        if (Spread > 0)
        {
            Spread -= 0.120f * Time.deltaTime;
        }
        else
        {
            Spread = 0;
            _wSpread = new Vector3(0, 0, 0);
        }

    }

    void GetInput()
    {
        _input.FireBottonDown = Input.GetButtonDown("Fire1");
        _input.ReloadButtonDown = Input.GetKeyDown(KeyCode.R);

        if (_input.FireBottonDown)
        {
            if (_isReloading == true)
            {
                _isReloading = false;
                _Anim.Play("Idle");
            }

            if (_player.Pistol.MagazineBullets > 0 && _canFire == true)
            {
                _Anim.Play("Idle");
                _player.Pistol.MagazineBullets -= 1;
                GunShot.Stop(gameObject);
                GunShot.Post(gameObject);
                _Anim.Play("HandGun_Shot");
                _player.Shot(0f);
                _canFire = false;
                StartCoroutine(DelaybtwShots());

                var fwd = transform.TransformDirection(Vector3.forward);
                RaycastHit hit = new RaycastHit();

                _muzzleFlash();

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
                    else if (hit.collider.gameObject.layer == 16 && hit.transform.GetComponent<Rigidbody>() != null)
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
                }
                //add bullet spread
                Spread += SpreadAmount;
                _wSpread = new Vector3(Random.Range(-Spread, Spread), Random.Range(-Spread, Spread), Random.Range(-Spread, Spread));
            }
        }
        if (_input.ReloadButtonDown && _player.Pistol.MagazineBullets < ClipSize && _isReloading == false && _player.Pistol.Ammo > 0)
        {
            _isReloading = true;
            _Anim.Play("Idle");
            _Anim.Play("Reload");
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
                case "Reload":
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
        AmmoDiff = ClipSize - _player.Pistol.MagazineBullets;

        if(AmmoDiff < _player.Pistol.Ammo)
        {
            _player.Pistol.Ammo -= AmmoDiff;
            _player.Pistol.MagazineBullets = ClipSize;
            _isReloading = false;
        }
        else
        {
            _player.Pistol.MagazineBullets += _player.Pistol.Ammo;
            _player.Pistol.Ammo -= _player.Pistol.Ammo;
            _isReloading = false;
        }

    }

    public void ReloadSound()
    {
        _sm.PistolReload.Post(gameObject);
    }

    private void _muzzleFlash()
    {
        Instantiate(MuzzleFlash, BulletExitPoint.position, Quaternion.identity);
    }
}
