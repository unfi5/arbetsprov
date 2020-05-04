using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    #region "Variables"

    public GameObject Player;
    public GameObject ThrowingObject;
    public GameObject ThrowPos;
    public float MouseSensitivity;
    public float objVelocity = 13;

    private Player _Player;
    private Animator _Anim;
    private Rigidbody _rb;
    private LineRenderer _lr;
    private float _yaw = 0.0f;
    private float _pitch = 0.0f;
    private bool _RunAnim = false;
    private bool _canThrow = true;

    public struct InputStates
    {
        public bool HorizontalButton;
        public bool VerticalButton;
        public bool QButtonDown;
        public bool Qbutton;
        public bool QbuttonUp;
    }

    InputStates _input;

    #endregion

    void Awake()
    {
        _Anim = GetComponent<Animator>();
        _rb = Player.GetComponent<Rigidbody>();
        _lr = GetComponent<LineRenderer>();
        _Player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (_Player.Health <= 0) return;
        SetInputs();
    }

    void SetInputs()
    {
        _input.HorizontalButton = Input.GetButton("Horizontal");
        _input.VerticalButton = Input.GetButton("Vertical");
        _input.QButtonDown = Input.GetKeyDown(KeyCode.Q);
        _input.Qbutton = Input.GetKey(KeyCode.Q);
        _input.QbuttonUp = Input.GetKeyUp(KeyCode.Q);

        if (_Player.Health > 0)
        {
            if (Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
            {
                RunAnimation(true);
            }
            else
            {
                RunAnimation(false);
            }

            if (_Player.isCrouching == true)
            {
                _Anim.SetFloat("Speed", 0.6f);
            }
            if (_Player.isSprinting == true)
            {
                _Anim.SetFloat("Speed", 2.0f);
            }
            else
            {
                _Anim.SetFloat("Speed", 1.0f);
            }
        }
        else if (_Player.Health <= 0)
        {
            _Anim.SetBool("Running", false);
            _Anim.enabled = false;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_Player.ThrowAmmo > 0 && _canThrow)
            {
                _lr.enabled = true;
            }

        }

        if (Input.GetKey(KeyCode.Q))
        {
            if (_Player.ThrowAmmo > 0)
            {
                ThrowArc();
            }

        }

        if (Input.GetKeyUp(KeyCode.Q))
        {
            _lr.enabled = false;
            if (_Player.ThrowAmmo > 0 && _canThrow)
            {
                Throw();
            }
        }
    }

    private void LateUpdate()
    {
        _yaw += MouseSensitivity * Input.GetAxis("Mouse X");
        _pitch -= MouseSensitivity * Input.GetAxis("Mouse Y");
        transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
    }

    public void RunAnimation(bool ToRun)
    {

        if (_Player.isGrounded && ToRun)
        {
            if (_RunAnim == false)
            {
                _Anim.SetBool("Running", true);
            }
        }

        else
        {
            _Anim.SetBool("Running", false);
        }
    }

    public void Foots()
    {
        _Player.Footsteps();
    }

    private void ThrowArc()
    {
        _lr.positionCount = 1;
        Vector3 point1 = ThrowPos.transform.position;
        _lr.SetPosition(0, point1);
        float stepSize = 0.01f;
        Vector3 tempObjVelocity = transform.forward * objVelocity;

        for (float step = 0; step < 2; step += stepSize)
        {
            tempObjVelocity += Physics.gravity * stepSize;
            Vector3 point2 = point1 + tempObjVelocity * stepSize;

            Ray ray = new Ray(point1, (point2 - point1));
            if (Physics.Raycast(ray, (point2 - point1).magnitude))
            {
                //hit
            }
            else
            {
                _lr.positionCount += 1;
                _lr.SetPosition(_lr.positionCount - 1, point1 + (point2 - point1));
                point1 = point2;
            }
        }
    }

    private void Throw()
    {
        var tobj = Instantiate(ThrowingObject, ThrowPos.transform.position, transform.rotation) as GameObject;
        tobj.transform.eulerAngles = transform.eulerAngles;
        Player.GetComponent<Player>().ThrowAmmo -= 1;
        _canThrow = false;
        StartCoroutine(ThrowTimer());
    }

    IEnumerator ThrowTimer()
    {
        yield return new WaitForSeconds(1);
        _canThrow = true;
    }
}
