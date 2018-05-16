using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittlePlayer : MonoBehaviour
{

    [Header("basic player Properties")]
    public float speed = 10.0f;
    private Vector3 _movement;
    private Rigidbody _rb;
    private CapsuleCollider _cap;

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
    // [HideInInspector]
    public bool WallRun = false;
    private bool isWallR = false;
    private bool isWallL = false;
    private bool isWallF = false;
    private bool isWallB = false;
    private RaycastHit hitR;
    private RaycastHit hitL;
    private RaycastHit hitF;
    private RaycastHit hitB;

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

        if (_grounded)
        {
            _movement = transform.rotation * _movement;
        }
        else
        {
            _movement = transform.rotation * (_movement * 0.5f);
        }
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + _movement * speed * Time.fixedDeltaTime);
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
                    WallRun = true;
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
                    WallRun = true;
                }
            }

            else if (Physics.Raycast(transform.position, transform.forward, out hitF, 1))
            {
                if (hitF.transform.tag == "Wall")
                {
                    isWallR = false;
                    isWallL = false;
                    isWallF = true;
                    isWallB = false;
                    WallRun = true;
                }
            }



            else if(Physics.Raycast(transform.position, -transform.forward, out hitB, 1))
            {
                if (hitB.transform.tag == "Wall")
                {
                    isWallR = false;
                    isWallL = false;
                    isWallF = false;
                    isWallB = true;
                    WallRun = true;
                }
            }
            else
            {
                isWallR = false;
                isWallL = false;
                isWallF = false;
                isWallB = false;
                WallRun = false;
                wallRunTime = 2.0f;
                _rb.useGravity = true;
            }

        }
        else
        {
            isWallR = false;
            isWallL = false;
            isWallF = false;
            isWallB = false;
            WallRun = false;
            _rb.useGravity = true;
        }

    }

    private void Movement()
    {
        _movement = Vector3.zero;

        if (!Slideing)
        {
            _movement.z = Input.GetAxis("Vertical");

            if (!WallRun)
            {
                _movement.x = Input.GetAxis("Horizontal");
            }
        }
    }

    private void Jumping()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && _grounded)
        {
            _rb.AddForce(Vector3.up * JumpForce);
            _rb.velocity += transform.rotation * _movement * speed;
        }
    }

    private void wallRunning()
    {

        if (isWallR)
        {
            _rb.useGravity = false;

            //Fixing camera angle.
            temp = Vector3.Cross(transform.up, -hitR.normal);
            targetAngle = Quaternion.LookRotation(-temp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);

            WallRunTimer();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }

        else if (isWallL)
        {
            _rb.useGravity = false;

            //Fixing camera angle.
            Vector3 temp = Vector3.Cross(transform.up, hitL.normal);
            targetAngle = Quaternion.LookRotation(-temp);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetAngle, rotationSpeed * Time.deltaTime);

            WallRunTimer();

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }

        else if (isWallF)
        {
            WallRunTimer();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }

        else if (isWallB)
        {
        }



    }

    public void WallRunTimer()
    {
        wallRunTime -= Time.deltaTime;

        if (wallRunTime < 0)
        {
            isWallL = false;
            isWallR = false;
            isWallF = false;
            isWallB = false;
            WallRun = false;
           _rb.useGravity = true;
        }
    }

    private void Sliding()
    {
        if (WallRun != true)
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

    private void WallJump() //Jumping from a wall. Different then a normal jump!
    {
        if (isWallR)

        {
            _rb.AddForce((-transform.right * (JumpForce * 0.5f)) + (transform.up * JumpForce));
            _rb.AddForce(_movement * speed);// += _movement * speed;
        }

        if (isWallL)
        {
            _rb.AddForce((transform.right * (JumpForce * 0.5f)) + (transform.up * JumpForce));
            //_rb.velocity += _movement * speed;
            _rb.AddForce(_movement * speed);
        }

        if (isWallF)
        {
            _rb.AddForce(transform.up * JumpForce * 0.75f);
        }
    }
}
