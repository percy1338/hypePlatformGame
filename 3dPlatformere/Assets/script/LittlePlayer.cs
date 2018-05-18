using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittlePlayer : MonoBehaviour
{
    [Header("basic player Properties")]
    public float speed = 10.0f;
    public float maxSpeed = 10;
    public float groundDrag = 3;
    public float airDrag = 0;
    private Vector3 _movement;
    private Vector3 _jump;
    private Rigidbody _rb;
    private CapsuleCollider _cap;
    // private Vector3 _momentum;

    [Header("Jump Properties")]
    public float JumpForce = 400.0f;
    private bool _grounded = false;

    [Header("Slide Properties")]
    public float SlideForce;
    public float MaxSlideDistance = 120;
    private Vector3 _slideVec;
    private float _curSlideDistance;
    private bool Slideing;

    [Header("WallRun Properties")]
    public bool _wallRun = false;
    private bool isWallR = false;
    private bool isWallL = false;
    private bool isWallF = false;
    private bool isWallB = false;
    private RaycastHit hitR;
    private RaycastHit hitL;
    private RaycastHit hitF;
    private RaycastHit hitB;
    [HideInInspector]

    [Header("Sickest Lerps")]
    public float rotationSpeed = 4.0f;
    public float wallRunTime = 2.0f;
    public Quaternion targetAngle;
    private Vector3 currentAngle;
    private Vector3 temp;


    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _cap = gameObject.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        GroundedCheck();
        wallsCheck();

        Movement();
        Jumping();
        wallRunning();
        Sliding();

        //Debug.Log(_rb.velocity);
    }

    void FixedUpdate()
    {
        _rb.AddForce(_movement);
        if (_rb.velocity.magnitude > maxSpeed)
        {
            _rb.velocity = _rb.velocity.normalized * maxSpeed;
        }
    }

    private void Movement()
    {
        _movement = Vector3.zero;

        if (!Slideing)
        {
            _movement.z = Input.GetAxis("Vertical");
            _movement.x = Input.GetAxis("Horizontal");
        }

        if (_grounded)
        {
            _movement = transform.rotation * (_movement * speed);
            _rb.drag = groundDrag;
        }
        else
        {
            _movement = transform.rotation * (_movement * (speed * 0.1f));
            _rb.drag = airDrag;
        }
    }

    private void Jumping()
    {
        if ((Input.GetButtonDown("Jump")) && _grounded)
        {
            _rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
        }
    }

    private void WallJump() //Jumping from a wall. Different then a normal jump!
    {
        if (isWallR)
        {
            _rb.AddForce((-transform.right * JumpForce) + (transform.up * (JumpForce * 0.5f)), ForceMode.Impulse);
        }

        if (isWallL)
        {
            _rb.AddForce((transform.right * JumpForce) + (transform.up * (JumpForce * 0.5f)), ForceMode.Impulse);
        }

        if (isWallF)
        {
            _rb.AddForce((transform.up * JumpForce));
            Debug.Log("ding ding");
        }
    }

    private void wallRunning()
    {
        if (isWallR)
        {
            _rb.useGravity = false;

            temp = Vector3.Cross(transform.up, -hitR.normal);
            targetAngle = Quaternion.LookRotation(-temp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);
            WallRunTimer();

            if ((Input.GetButtonDown("Jump")))
            {
                WallJump();
            }
        }

        else if (isWallL)
        {
            _rb.useGravity = false;

            Vector3 temp = Vector3.Cross(transform.up, hitL.normal);
            targetAngle = Quaternion.LookRotation(-temp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);

            WallRunTimer();

            if ((Input.GetButtonDown("Jump")))
            {
                WallJump();
            }
        }

        else if (isWallF)
        {
            WallRunTimer();

            if ((Input.GetButtonDown("Jump")))
            {
                WallJump();
            }
        }

        else if (isWallB)
        {
        }
    }

    private void WallRunTimer()
    {
        wallRunTime -= Time.deltaTime;

        if (wallRunTime < 0)
        {
            isWallL = false;
            isWallR = false;
            isWallF = false;
            isWallB = false;
            _wallRun = false;
            _rb.useGravity = true;
        }
    }

    private void Sliding()
    {
        if (_wallRun != true)
        {
            if ((Input.GetKeyDown(KeyCode.LeftShift)))
            {
                _slideVec = transform.forward;
                Slideing = true;
            }
            if ((Input.GetKeyUp(KeyCode.LeftShift)))
            {
                Slideing = false;
            }
            if (Slideing)
            {
                _cap.height = 0.5f;
                _rb.AddForce(_slideVec * SlideForce);
            }
            else
            {
                _cap.height = 2;
            }
            if (_curSlideDistance >= MaxSlideDistance)
            {
                Slideing = false;
                _curSlideDistance = 0;
            }
        }
    }

    private void wallsCheck()
    {
        if (!_grounded)
        {
            if (Physics.Raycast(transform.position, transform.right, out hitR, 1))
            {
                if (hitR.transform.tag == "Wall")
                {
                    isWallR = true;
                    isWallL = false;
                    isWallF = false;
                    isWallB = false;
                    _wallRun = true;
                }
            }

            else if (Physics.Raycast(transform.position, -transform.right, out hitL, 1))
            {
                if (hitL.transform.tag == "Wall")
                {
                    isWallR = false;
                    isWallL = true;
                    isWallF = false;
                    isWallB = false;
                    _wallRun = true;
                }
            }

            else if (Physics.Raycast(transform.position, transform.forward, out hitF, 1))
            {
                if (hitF.transform.tag == "Wall" || hitF.transform.tag == "Floor")
                {
                    isWallR = false;
                    isWallL = false;
                    isWallF = true;
                    isWallB = false;
                    _wallRun = true;
                }
            }

            else if (Physics.Raycast(transform.position, -transform.forward, out hitB, 1))
            {
                if (hitB.transform.tag == "Wall" || hitB.transform.tag == "Floor")
                {
                    isWallR = false;
                    isWallL = false;
                    isWallF = false;
                    isWallB = true;
                    _wallRun = true;
                }
            }
            else
            {
                isWallR = false;
                isWallL = false;
                isWallF = false;
                isWallB = false;
                _wallRun = false;
                wallRunTime = 2.0f;
                _rb.useGravity = true;
                Debug.Log("happend");
            }
        }
        else
        {
            isWallR = false;
            isWallL = false;
            isWallF = false;
            isWallB = false;
            _wallRun = false;
        }
    }

    private void GroundedCheck()
    {
        RaycastHit hit;
        Ray jumpRay = new Ray(this.transform.position, Vector3.down);

        if (Physics.Raycast(jumpRay, out hit, 1.01f))
        {
            if (hit.transform.tag == "Wall")
            {
                _grounded = true;
                wallRunTime = 2.0f;
            }
        }
        else
        {
            _grounded = false;
        }
    }
}