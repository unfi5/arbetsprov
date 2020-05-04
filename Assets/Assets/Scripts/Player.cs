using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Variabels
    [SerializeField] HUD_DamageRight _dmgRight;
    [SerializeField] HUD_DamageLeft _dmgLeft;
    [SerializeField] OverHeadChecker _ohChecker;
    [SerializeField] SoundManager _sm;
    [SerializeField] Text _helpSubs;

    public Rigidbody rb;
    public GameObject Camera;
    public GameObject Pf_BulletHole;

    public int Health = 100;
    public int KeyCards = 0;

    public float MovementSpeed;
    public float JumpForce;
    public float SprintSpeed;
    public float CrouchSpeed;

    public bool isGrounded;
    public bool isCrouching;
    public bool isSprinting;
    public bool isClimbing = false;

    private RaycastHit _hit;
    private float fallMultiplier = 1.5f;
    private float _storedMoveSpeed;
    private bool _StopCrouch;

    public struct InputStates
    {
        public bool SprintButtonDown;
        public bool SprintButtonUp;
        public bool JumpButtonDown;
        public bool CrouchButtonDown;
        public bool CrouchButtonUp;
        public bool OneButtonDown;
        public bool TwoButtonDown;
        public float ScrollWheel;
    }
    InputStates _input;
    #endregion

    #region Weapon Variables

    [SerializeField] Gun _pistolScipt;
    [SerializeField] AssaultRifle _arScript;

    public enum Weapons
    {
        Pistol,
        AssualtRifle,
    }

    public struct Weapon
    {
        public int MagazineBullets;
        public int Ammo;
        public int MaxAmmo;
        public int ClipSize;
        public bool Unlocked;
    }
    public Weapon Pistol;
    public Weapon AssaultRifle;

    public GameObject PistolObject;
    public GameObject AssaultRifleObject;

    public int ThrowAmmo = 2;

    public int weaponSelected = 0;
    #endregion

    #region Reverb Variables
    public static AK.Wwise.Event ReverbEvent;
    public static float ReverbDistance;
    public static float MaterialAbsorption;

    public string Material;
    public int Reflections;
    public float maxLength;

    private ObjectPool _objPool;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 direction;

    [System.Serializable]
    public struct MaterialAbsorptions
    {
        public string MaterialTag;
        public float AbsorptionValue;        
    }

    [SerializeField] MaterialAbsorptions[] _materialAbsorption;
    Dictionary<string, float> _materialAbsorptionLookUp = new Dictionary<string, float>();
    #endregion    

    private void Awake()
    {
        foreach (var matAborption in _materialAbsorption)
            _materialAbsorptionLookUp.Add(matAborption.MaterialTag, matAborption.AbsorptionValue);

        Pistol.ClipSize = _pistolScipt.ClipSize;
        Pistol.MagazineBullets = _pistolScipt.ClipSize;
        Pistol.MaxAmmo = _pistolScipt.MaxAmmo;
        Pistol.Unlocked = false;

        AssaultRifle.ClipSize = _arScript.ClipSize;
        AssaultRifle.MagazineBullets = _arScript.ClipSize;
        AssaultRifle.MaxAmmo = _arScript.MaxAmmo;
        AssaultRifle.Unlocked = false;

        _storedMoveSpeed = MovementSpeed;

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _objPool = ObjectPool.Instance;
    }

    void Update()
    {
        if (Health <= 0) return;
        SetMovementSpeed();
        SetInputs();
    }

    void SetMovementSpeed()
    {
        if (!isClimbing)
        {
            //Movement
            rb.MovePosition(transform.position + (transform.forward * Input.GetAxis("Vertical") * MovementSpeed) + (transform.right * Input.GetAxis("Horizontal") * MovementSpeed));
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.transform.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            //Ladder Climbing
            rb.MovePosition(transform.position + (transform.up * Input.GetAxis("Vertical") * MovementSpeed) + (transform.right * Input.GetAxis("Horizontal") * MovementSpeed / 2));
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, Camera.transform.eulerAngles.y, transform.rotation.eulerAngles.z);
        }

        //Limit movement speed
        if (MovementSpeed > _storedMoveSpeed + SprintSpeed && !Input.GetButton("Sprint"))
        {
            MovementSpeed = _storedMoveSpeed;
        }
        else if (MovementSpeed > _storedMoveSpeed + SprintSpeed && Input.GetButton("Sprint"))
        {
            MovementSpeed = _storedMoveSpeed + SprintSpeed;
        }
        else if (MovementSpeed < _storedMoveSpeed - CrouchSpeed && !Input.GetButton("Crouch"))
        {
            MovementSpeed = _storedMoveSpeed;
        }
        else if (MovementSpeed < _storedMoveSpeed - CrouchSpeed && Input.GetButton("Crouch"))
        {
            MovementSpeed = _storedMoveSpeed - CrouchSpeed;
        }

        //Falling Gravity
        if (rb.velocity.y < 0 && !isGrounded && !isClimbing)
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            if (transform.position.y < -20)
            {
                rb.velocity = new Vector3(0, 0, 0);
                transform.position = new Vector3(2.4f, 1.5f, -84f);
            }
        }
    }

    void SetInputs()
    {
        _input.SprintButtonDown = Input.GetButtonDown("Sprint");
        _input.SprintButtonUp = Input.GetButtonUp("Sprint");
        _input.CrouchButtonDown = Input.GetButtonDown("Crouch");
        _input.CrouchButtonUp = Input.GetButtonUp("Crouch");
        _input.JumpButtonDown = Input.GetButtonDown("Jump");
        _input.OneButtonDown = Input.GetKeyDown("1");
        _input.TwoButtonDown = Input.GetKeyDown("2");
        _input.ScrollWheel = Input.GetAxis("Mouse ScrollWheel");

        //Stop Sprint
        if (_input.SprintButtonUp && isCrouching == false)
        {
            isSprinting = false;
            MovementSpeed -= SprintSpeed;
        }

        //Sprint
        if (_input.SprintButtonDown && isCrouching == false)
        {
            isSprinting = true;
            MovementSpeed += SprintSpeed;
        }

        //Jump
        if (_input.JumpButtonDown && !isClimbing && !isCrouching)
        {
            if (isGrounded == true)
            {
                isGrounded = false;
                rb.AddForce(JumpForce * transform.up, ForceMode.Impulse);
            }
        }

        //Crouch
        if (_input.CrouchButtonDown && !isCrouching && _StopCrouch == false)
        {
            _StopCrouch = false;
            isCrouching = true;
            transform.localScale = new Vector3(1.0f, 0.8f, 1.0f);
            MovementSpeed *= CrouchSpeed;
        }

        //Stop Crouch
        if (_input.CrouchButtonUp && isSprinting == false && isCrouching)
        {
            _StopCrouch = true;
        }

        //Standup
        if (_StopCrouch == true && _ohChecker.standupCheck == true)
        {
            _StopCrouch = false;
            isCrouching = false;
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            MovementSpeed /= CrouchSpeed;
        }

        //Weapon Switching

        if (_input.ScrollWheel > 0f)
        {
            if (Pistol.Unlocked && AssaultRifle.Unlocked)
            {
                weaponSelected += 1;
                if (weaponSelected > 2)
                {
                    weaponSelected = 1;
                }
            }

        }
        else if (_input.ScrollWheel < 0f)
        {
            if (Pistol.Unlocked & AssaultRifle.Unlocked)
            {
                weaponSelected -= 1;
                if (weaponSelected == 0)
                {
                    weaponSelected = 2;
                }
            }

        }

        if (weaponSelected == 1 && Pistol.Unlocked)
        {
            if (_input.ScrollWheel != 0)
            {
                PistolObject.SetActive(true);
                AssaultRifleObject.SetActive(false);
            }
            AssaultRifleObject.transform.eulerAngles = Camera.transform.eulerAngles;
        }
        else if (weaponSelected == 2 && AssaultRifle.Unlocked)
        {
            if (_input.ScrollWheel != 0)
            {
                PistolObject.SetActive(false);
                AssaultRifleObject.SetActive(true);
            }
            PistolObject.transform.eulerAngles = Camera.transform.eulerAngles;
        }

        if (_input.OneButtonDown && Pistol.Unlocked)
        {
            PistolObject.SetActive(true);
            AssaultRifleObject.SetActive(false);
            weaponSelected = 1;
        }

        if (_input.TwoButtonDown && AssaultRifle.Unlocked)
        {
            PistolObject.SetActive(false);
            AssaultRifleObject.SetActive(true);
            weaponSelected = 2;
        }

    }

    #region GroundCheck
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.gameObject.layer == 11 || collision.transform.gameObject.layer == 16)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.gameObject.layer == 11 || collision.transform.gameObject.layer == 16)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.transform.gameObject.layer == 11 || collision.transform.gameObject.layer == 16)
        {
            isGrounded = false;
        }
    }
    #endregion

    //Health
    public void PlayerHealth(int Damage)
    {
        Health -= Damage;

        if(Damage > 0)
        {
            _sm.PlayerHurt.Post(gameObject);
        }

        if(Health <= 0)
        {
            Health = 0;
            
            this.gameObject.transform.eulerAngles = new Vector3(90.0f, 0.0f, 90.0f);
            weaponSelected = 0;
            PistolObject.SetActive(false);
            AssaultRifleObject.SetActive(false);
            Camera.GetComponent<CameraScript>().RunAnimation(false);
            StartCoroutine(Restart());
        }

        if(Health > 100)
        {
            Health = 100;
        }       
    }

    //HUD Damage angle
    public void DamageAngle(float angle)
    {
        if (angle < 3 && angle > -30)
        {
            //Forward and backwards
        }
        else if (angle > -30)
        {
            _dmgRight.DamageRight();
        }
        else if (angle < 30)
        {
            _dmgLeft.DamageLeft();
        }
    }

    //Restart
    private IEnumerator Restart()
    {

        yield return new WaitForSeconds(5);

        Health = 100;
        SceneManager.LoadScene(0);
    }

    public IEnumerator Win()
    {
        _helpSubs.text = "Mission succeeded";

        yield return new WaitForSeconds(5);

        Health = 100;
        SceneManager.LoadScene(0);
    }

    //Ammo pickup
    public void AmmoPickup(int amount, Weapons Weapon, GameObject obj)
    {
        switch (Weapon)
        {
            case Weapons.Pistol:
                Pistol.Ammo = Mathf.Min(Pistol.Ammo + amount, Pistol.MaxAmmo);
                break;
            case Weapons.AssualtRifle:
                AssaultRifle.Ammo = Mathf.Min(AssaultRifle.Ammo + amount, AssaultRifle.MaxAmmo);
                break;
        }
        obj.GetComponent<AmmoPickup>().Destroy();
    }

    //Footsteps
    public void Footsteps()
    {
        ReverbEvent = _sm.Footstep;
        ReverbEvent.Post(gameObject);
        Reverb();
        
        if(isCrouching == true)
        {
            MakeSound(0f);
        }
        else if (isSprinting == true)
        {
            MakeSound(7f);
        }
        else
        {
            MakeSound(3f);
        }
    }

    //Shot
    public void Shot(float SoundSize)
    {
        if(weaponSelected == 1)
        {
            ReverbEvent = _sm.PistolShot;
        }
        else if(weaponSelected == 2)
        {
            ReverbEvent = _sm.ARShot;
        }
        Reverb();
        MakeSound(SoundSize);
    }

    //Sound bubble
    public void MakeSound(float SoundSize)
    {
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

    //Reverb trigger
    public void Reverb()
    {
        ReverbRay(transform.forward);
        ReverbRay(-transform.forward);
        ReverbRay(-transform.right);
        ReverbRay(transform.right);
        ReverbRay(transform.up);
        ReverbRay(-transform.up);
    }

    //Reverb rays
    public void ReverbRay(Vector3 direction)
    {
        ray = new Ray(transform.position, direction);

        float remainingLenght = maxLength;
        MaterialAbsorption = 0.0f;

        for (int i = 0; i < Reflections; i++)
        {
            if (Physics.Raycast(ray.origin, ray.direction, out hit, remainingLenght))
            {
                Material = hit.collider.tag;
                if (Material != null && _materialAbsorptionLookUp.ContainsKey(Material))
                {
                    MaterialAbsorption += _materialAbsorptionLookUp[Material];
                }
                else
                {
                    MaterialAbsorption += 0.2f;
                }
                _objPool.SpawnFromPool("Reverb", hit.point, Vector3.Reflect(ray.direction, hit.normal));

                remainingLenght -= Vector3.Distance(ray.origin, hit.point);
                ReverbDistance = maxLength - remainingLenght;
                ray = new Ray(hit.point, Vector3.Reflect(ray.direction, hit.normal));
            }
        }
    }
}
