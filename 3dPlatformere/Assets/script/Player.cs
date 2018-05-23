using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("basic player Properties")]
    public float acceleration = 10.0f;
    public float maxSpeed = 10;
    public float groundDrag = 3;
    public float airDrag = 0;
    private Vector3 _movement;
    private Vector3 _jump;
    private Rigidbody _rb;
    private CapsuleCollider _cap;
    private Camera _cam;

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
    public float wallrunAcceleration = 5;
    private bool isWallR = false;
    private bool isWallL = false;
    private bool isWallF = false;
    private bool isWallB = false;
    private float _velocityFloat;
    public float _wallRunTime = 2;
    private RaycastHit hitR;
    private RaycastHit hitL;
    private RaycastHit hitF;
    private RaycastHit hitB;

    [Header("Sickest Lerps")]
    public float rotationSpeed = 4.0f;
    public Quaternion targetAngle;
    private Vector3 currentAngle;
    private Vector3 temp;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _cap = gameObject.GetComponent<CapsuleCollider>();
        _cam = gameObject.GetComponentInChildren<Camera>();

        Cursor.visible = false;
    }

    void Update()
    {
        if (_rb.velocity.y > 0)
        {
            _velocityFloat = _rb.velocity.magnitude - _rb.velocity.y;
        }
        else
        {
            _velocityFloat = _rb.velocity.magnitude + _rb.velocity.y;
        }
        
        GroundedCheck();
        wallsCheck();

        Movement();
        Jumping();
        wallRunning();
        Sliding();

       // Debug.Log(_velocityFloat);
        Debug.Log(_wallRunTime);
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
 
        if(!Slideing)
        {
            _movement.z = Input.GetAxis("Vertical");
            _movement.x = Input.GetAxis("Horizontal");
        }   

        if (_grounded)
        {
            _movement = transform.rotation * (_movement * acceleration);
            _rb.drag = groundDrag;
        }
        else if (_wallRun)
        {
            _movement = transform.rotation * (_movement * wallrunAcceleration);
            _rb.drag = groundDrag;
        }
        else
        {
            _movement = transform.rotation * (_movement * (acceleration * 0.1f));
            _rb.drag = airDrag;
        }

    }

    private void Jumping()
    {
        if ((Input.GetButtonDown("Jump")) && _grounded)
        {
            _rb.AddForce((Vector3.up * JumpForce) + (_rb.velocity * 0.1f), ForceMode.Impulse);

        }
    }

    private void wallRunning()
    {

        if (_velocityFloat > 1.0f)
        {
            if (isWallR)
            {
                _rb.useGravity = false;

                temp = Vector3.Cross(transform.up, -hitR.normal);
                targetAngle = Quaternion.LookRotation(-temp);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);

                WallRunTimer();
                _wallRun = true;
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
                _wallRun = true;

                if ((Input.GetButtonDown("Jump")))
                {
                    WallJump();
                }
            }

            else if (isWallF)
            {
                if ((Input.GetButtonDown("Jump")))
                {
                    WallJump();
                }
            }

            else if (isWallB)
            {
                if ((Input.GetButtonDown("Jump")))
                {
                    WallJump();
                }
            }
            else
            {
                _rb.useGravity = true;
                _wallRun = false;
            }
        }
        else
        {
            if ((Input.GetButtonDown("Jump")))
            {
                WallJump();
            }
            _rb.useGravity = true;
            _wallRun = false;
        }
    }

    private void WallJump() //Jumping from a wall. Different then a normal jump!
    {
        _wallRunTime = 2f;
        if (isWallR)
        {
            _rb.AddForce((-transform.right * (JumpForce * 0.7f)) + (transform.up * (JumpForce * 0.75f)), ForceMode.Impulse);
        }
        if (isWallL)
        {
            _rb.AddForce((transform.right * (JumpForce * 0.7f)) + (transform.up * (JumpForce * 0.75f)), ForceMode.Impulse);
        }
        if (isWallF)
        {
            _rb.AddForce((-transform.forward * (JumpForce * 0.7f)) + (transform.up * JumpForce * 0.75f), ForceMode.Impulse);
        }
        if (isWallB)
        {
            _rb.AddForce((transform.forward * (JumpForce * 0.7f)) + (transform.up * JumpForce * 0.75f), ForceMode.Impulse);
        }
    }

    private void WallRunTimer()
    {
        _wallRunTime -= Time.deltaTime;
        if (_wallRunTime < 0)
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
        if(_velocityFloat < 2)
        {
            Slideing = false;
        }
        if (_wallRun != true)
        {
            if ((Input.GetKeyDown(KeyCode.LeftShift)) && _grounded && _velocityFloat > 2)
            {
                groundDrag = 0;
                _cap.height = 0.5f;
                _rb.velocity += _rb.velocity;
                Slideing = true;
            }
            else if ((Input.GetKeyDown(KeyCode.LeftShift)) && _grounded && _velocityFloat < 2)
            {
                _cap.height = 0.5f;
                
            }
            if ((Input.GetKeyUp(KeyCode.LeftShift)))
            {
                Slideing = false;
                groundDrag = 4;
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
                }
            }
            else
            {
                isWallR = false;
                isWallL = false;
                isWallF = false;
                isWallB = false;
                _rb.useGravity = true;
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
        _wallRunTime = 2.0f;
        if (Physics.Raycast(jumpRay, out hit, 1.01f))
        {
            if (hit.transform.tag == "Wall")
            {
                _grounded = true;
            }
        }
        else
        {
            _grounded = false;
        }
    }
}