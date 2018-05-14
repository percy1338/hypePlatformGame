using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("basic player Properties")]
    public float speed = 10.0f;

    [Header("Jump Properties")]
    public float JumpForce = 0.0f;
    private Transform LastWall;
    private bool _grounded = false;

    [Header("dash Properties")]
    public float DashForce = 0.0f;
    private int _dashCount = 0;

    [Header("Slide Properties")]
    public float SlideForce;
    public float MaxSlideDistance = 120;
    private Vector3 _slideVec;
    private float _curSlideDistance;
    private bool Slideing;

    private bool _wallRun = false;

    private Vector3 Momentum;

    private Rigidbody _rb;
    private Vector3 _movement;
    private CapsuleCollider _cap;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _cap = gameObject.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
        Movement();
        Momentum = _rb.velocity;
        Jumping();
        //Sliding();
        GroundedCheck();
        //Debug.Log(_wallRun);
       // Debug.Log(_grounded);
        Debug.Log(Momentum);
        
    }

    void FixedUpdate()
    {
        Vector3 gravFix = new Vector3(0, _rb.velocity.y, 0);

        _rb.velocity = _movement * speed;
        _rb.velocity += gravFix;
    }

    private void Movement()
    {
       //_movement.x = 0;
       // _movement.z = 0;
        if (!Slideing)// && _grounded)
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

        _movement = transform.rotation * _movement;
    }

    private void Jumping()
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && _grounded == true)
        {         
            _grounded = false;
            Slideing = false;
            _rb.velocity = (Vector3.up * JumpForce);// + Momentum;
            Debug.Log(Momentum);
        }
    }

    private void Sliding()
    {
        if ((Input.GetKeyDown(KeyCode.LeftShift)) && _grounded)
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



   /* private void Airdash()
    {
        _rb.AddForce(Vector3.forward * DashForce);
        _dashCount++;
    }*/

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

                if ((Input.GetKeyDown(KeyCode.Space) && LastWall != objectHit.transform))
                {
                    LastWall = objectHit.transform;
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

        Vector3 launch = _rb.position;
        launch.x -= 10;

        _rb.AddExplosionForce(1000, launch, 100);
        _rb.AddForce(Vector3.up * JumpForce);
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
