using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("basic player Properties")]
    public float speed = 10.0f;

    [Header("Jump Properties")]
    public float JumpForce = 0.0f;
    private bool _grounded = false;

    [Header("Dash Properties")]
    public float DashForce = 0.0f;
    private int _dashCount = 0;

    [Header("Slide Properties")]
    public float SlideForce;
    public float MaxSlideDistance = 120;
    private Vector3 _slideVec;
    private float _curSlideDistance;
    private bool Slideing;

    [Header("WallRun Properties")]
    public float _wallRunForce = 0.0f;
    private Vector3 _wallRunVec;
    private bool _wallRun = false;
    //private Transform LastWall;
    public Transform currentWall;

    public bool UnlockCamera = false;


    private bool leftSide;
    private bool rightSide;
    private bool frontSide;
    // Raycasts.
    public RaycastHit Hitleft;
    public RaycastHit HitRight;

    private Rigidbody _rb;
    private Vector3 _movement;
    private CapsuleCollider _cap;
   // private Vector3 _test;


    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _cap = gameObject.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        GroundedCheck();
        Debug.Log(_grounded);
        Movement();
        Jumping();
        Wallrunning();
        Sliding();
        if(_grounded)
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
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Wall")
            {
                _grounded = true;
            }
        }
        else
        {
            _grounded = false;
        }
    }

    private void Movement()
    {
        _movement = Vector3.zero;

        if (!Slideing)
        {
            if (WallRunCheck(transform.forward * Input.GetAxis("Vertical")))
            {
                _movement.z = Input.GetAxis("Vertical");
            }

            if (WallRunCheck(transform.right * Input.GetAxis("Horizontal")))
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

    private void Wallrunning()
    {
        //if (_wallRun)
        //{
        //    UnlockCamera = true;
        //}
        //else
        //{
        //    UnlockCamera = false;
        //}
    }

    private void Sliding()
    {
        //if (_wallrun != true)
        //{
        //    if ((input.getkeydown(keycode.leftshift)))
        //    {
        //        _slidevec = transform.forward;
        //        slideing = true;
        //    }
        //    if ((input.getkeyup(keycode.leftshift)))
        //    {
        //        slideing = false;
        //    }
        //    if (slideing)
        //    {
        //        _cap.height = 0.5f;
        //        _rb.addforce(_slidevec * slideforce);
        //    }
        //    else
        //    {
        //        _cap.height = 2;
        //    }
        //    if (_curslidedistance >= maxslidedistance)
        //    {
        //        slideing = false;
        //        _curslidedistance = 0;
        //    }
        //}
    }


    private bool WallRunCheck(Vector3 direction)
    {
        float distanceToPoints = _cap.height / 2 - _cap.radius;

        Vector3 point1 = transform.position + _cap.center + Vector3.up * distanceToPoints;
        Vector3 point2 = transform.position + _cap.center - Vector3.up * distanceToPoints;
        float radius = _cap.radius * 0.95f;
        float castDistance = 0.5f;

        RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, radius, direction, castDistance);

        foreach (RaycastHit objectHit in hits)
        {
            if (objectHit.transform.tag == "Wall" || objectHit.transform.tag == "Ground")
            {
                _wallRun = true;
                currentWall = objectHit.transform;

                checkSide();
                wallrunCamera(objectHit);

                if ((Input.GetKeyDown(KeyCode.Space))) // && LastWall != objectHit.transform
                {
                   // LastWall = objectHit.transform;
                    _wallRun = false;
                    WallJump();
                }
                return false;
            }
        }
        _wallRun = false;

        return true;
    }

    private void checkSide()
    {
        RaycastHit hit;

        Ray LeftRay = new Ray(this.transform.position, transform.right);
        Ray RightRay = new Ray(this.transform.position, -transform.right);
        Ray FrontRay = new Ray(this.transform.position, transform.forward);

        if (Physics.Raycast(LeftRay, out hit, 1.01f))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Wall")
            {
                rightSide = false;
                leftSide = true;
                frontSide = false;
            }
        }

        if (Physics.Raycast(RightRay, out hit, 1.01f))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Wall")
            {
                rightSide = true;
                leftSide = false;
                frontSide = false;
            }
        }

        if (Physics.Raycast(FrontRay, out hit, 1.01f))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Wall")
            {
                rightSide = false;
                leftSide = false;
                frontSide = true;
            }
        }
    }

    private void wallrunCamera(RaycastHit objectHit)
    {
        //if (!_grounded)
        //{
        //    if (rightSide == true)
        //    {
        //       // transform.rotation = Quaternion.FromToRotation(Vector3.right, objectHit.normal);
        //    }

        //    if (leftSide == true)
        //    {
        //        //transform.rotation = Quaternion.FromToRotation(-Vector3.right, objectHit.normal);
        //    }
        //}

    }

    private void WallJump() //Jumping from a wall. Different then a normal jump!
    {
        if(leftSide)
        {
            _rb.AddForce((-transform.right * (JumpForce *0.5f)) + (transform.up * JumpForce));
            _rb.AddForce(_movement * speed);// += _movement * speed;
        }
        if(rightSide)
        {
            _rb.AddForce((transform.right * (JumpForce * 0.5f)) + (transform.up * JumpForce));
            //_rb.velocity += _movement * speed;
            _rb.AddForce(_movement * speed);
        }
        if(frontSide)
        {
            _rb.AddForce(transform.up * JumpForce * 0.75f);
        }   
    }
}