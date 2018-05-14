using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backupPlayer : MonoBehaviour
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
    public float _wallRunForce  = 0.0f;
    private Vector3 _wallRunVec;
    private bool _wallRun = false;
    private Transform LastWall;
    public Transform currentWall;

    public bool UnlockCamera = false;


    private Rigidbody _rb;
    private Vector3 _movement;
    private CapsuleCollider _cap;
    private Vector3 _test;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _cap = gameObject.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        Movement();
        Jumping();
        Wallrunning();
        Sliding();
        
        GroundedCheck();
        //Debug.Log(_wallRun);
    }

    void FixedUpdate()
    {
        Vector3 gravFix = new Vector3(0, _rb.velocity.y, 0);

        _rb.velocity = _movement * speed;
        _rb.velocity += gravFix;
    }

    private void Movement()
    {
        _test.x = 0;
        _test.y = 0;
        _test.z = 0;

        _movement = _test;

        if (!Slideing)
        {
            if (WallRunCheck(transform.forward * Input.GetAxis("Vertical")))
            {
                _test.z = Input.GetAxis("Vertical");
            }

            if (WallRunCheck(transform.right * Input.GetAxis("Horizontal")))
            {
                _test.x = Input.GetAxis("Horizontal");
            }
        }

        _movement += _test;
        _movement = transform.rotation * _movement;
    }

    private void Jumping()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && _grounded == true)
        {
            _grounded = false;
            _rb.AddForce(Vector3.up * JumpForce);
        }
    }

    private void Wallrunning()
    {
        if (_wallRun)
        {
            UnlockCamera = true;
        }
        else
        {
            UnlockCamera = false;
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

                if ((Input.GetKeyDown(KeyCode.Space) && LastWall != objectHit.transform))
                {
                    LastWall = objectHit.transform;
                    _wallRun = false;
                    WallJump();
                }
                return false;
            }
        }
        _wallRun = false;

        return true;
    }

    private void WallJump() //Jumping from a wall. Different then a normal jump!
    {
        Vector3 test = new Vector3(_rb.velocity.x, 0, _rb.velocity.z);
        _rb.velocity = test;
        _rb.AddForce(Vector3.up * JumpForce);
    }

    private void Airdash()
    {
        _rb.AddForce(Vector3.forward * DashForce);
        _dashCount++;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.name == "CrawlSpace")
        //    {
        //       Slideing = true;
        //   }
        //  else
        //  {
        //       Slideing = false;
        //  }
        //  Debug.Log("crawling");
    }
}
