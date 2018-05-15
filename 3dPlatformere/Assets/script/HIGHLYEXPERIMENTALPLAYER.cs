using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HIGHLYEXPERIMENTALPLAYER : MonoBehaviour {

    [Header("basic player Properties")]
    public float speed = 10.0f;
    private float savedSpeed;

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
    private bool _wallRun = false;
    private bool isWallR = false;
    private bool isWallL = false;
    private bool isWallF = false;
    private bool isWallB = false;

    private RaycastHit hitR;
    private RaycastHit hitL;
    private RaycastHit hitF;
    private RaycastHit hitB;

    public bool UnlockCamera = false; 

    private Rigidbody _rb;
    private Vector3 _movement;
    private CapsuleCollider _cap;
    private Vector3 _test;


    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _cap = gameObject.GetComponent<CapsuleCollider>();
        savedSpeed = speed;
    }

    void Update()
    {
        GroundedCheck();
        wallsCheck();

        Movement();

        Jumping(_test);
        wallRunning();
        Sliding();

        _movement = transform.rotation * _movement;
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
                speed = savedSpeed;
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
                    _wallRun = true;
                   // Debug.Log("Hit Right!");
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
                   // Debug.Log("Hit Left!");
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
                    //Debug.Log("Hit Forward!");
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
                  //  Debug.Log("Hit Backwards!");
                }
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

    private void Movement()
    {
        _test = Vector3.zero;
        _movement = _test;

        if (!Slideing)
        {
            _test.z = Input.GetAxis("Vertical");

            _test.x = Input.GetAxis("Horizontal");
        }

        _movement += _test;
    }

    private void Jumping(Vector3 momentum)
    {
        if ((Input.GetKeyDown(KeyCode.Space)) && _grounded)
        {
            _grounded = false;
            speed *= 0.5f;
            _rb.AddForce(Vector3.up * JumpForce);
            _rb.velocity += transform.rotation * momentum * speed;
        }
    }

    private void wallRunning()
    {
        if (isWallR)
        {
            _rb.useGravity = false;
            StartCoroutine(afterRun(0.5f));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }

        else if (isWallL)
        {
            _rb.useGravity = false;
            StartCoroutine(afterRun(0.5f));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }

        else  if (isWallF)
        {
             StartCoroutine(afterRun(0.5f));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }

        else if (isWallB)
        {
             StartCoroutine(afterRun(0.5f));
        }


    }

    IEnumerator afterRun(float CD)
    {
        yield return new WaitForSeconds(CD);
        
        isWallL = false;
        isWallR = false;
        isWallF = false;

        isWallB = false;
        _rb.useGravity = true;
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


    private void WallJump() //Jumping from a wall. Different then a normal jump!
    {
        if (isWallR)

        {
            _rb.AddForce((-transform.right * (JumpForce * 0.5f)) + (transform.up * JumpForce));
            _rb.velocity += _test * speed;
        }

        if (isWallL)
        {
            _rb.AddForce((transform.right * (JumpForce * 0.5f)) + (transform.up * JumpForce));
            _rb.velocity += _test * speed;
        }

        if (isWallF)
        {
            _rb.AddForce(transform.up * JumpForce * 0.75f);
        }
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