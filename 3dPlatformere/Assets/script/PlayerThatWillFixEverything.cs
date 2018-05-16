using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThatWillFixEverything : MonoBehaviour
{
    [Header ("general properties")]

    public float Gravity = 9.8f;

    [Header("player properties")]

    public float speed = 5;
    private float verticalVelocity;
    private Vector3 _movement;
    private CharacterController _pc;
    private CapsuleCollider _cap;

    [Header("jumping properties")]

    public float JumpForce = 5;
    private bool _grounded;

    [Header("Slide Properties")]

    public float SlideForce = 5;
    public float MaxSlideDistance = 120;
    private Vector3 _slideVec;
    private float _curSlideDistance;
    private bool Slideing;

    [Header("Wallrun Properties")]

    private bool _wallRun = false;
    private bool isWallR = false;
    private bool isWallL = false;
    private bool isWallF = false;
    private bool isWallB = false;
    private RaycastHit hitR;
    private RaycastHit hitL;
    private RaycastHit hitF;
    private RaycastHit hitB;
    [HideInInspector]
    public bool UnlockCamera = false;



    // Use this for initialization
    void Start ()
    {
        _pc = gameObject.GetComponent<CharacterController>();
        _cap = gameObject.GetComponent<CapsuleCollider>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        wallsCheck();       
        Movement();
        wallRunning();
        //Sliding();

      //  Debug.Log(_movement);
    }

    private void Movement()
    {
        _movement = Vector3.zero;
        _movement.z = Input.GetAxis("Vertical");
        _movement.x = Input.GetAxis("Horizontal");

        if (_pc.isGrounded)
        {
            _movement = transform.rotation * (_movement * speed);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = JumpForce;
            }
        }
        else
        {
            _movement = transform.rotation * (_movement * (speed * 0.25f));
            verticalVelocity -= Gravity * Time.deltaTime;
        }
        _movement.y = verticalVelocity;

        _pc.Move(_movement);
    }

    private void wallRunning()
    {
        if (isWallR)
        {
            //_rb.useGravity = false;
            StartCoroutine(afterRun(0.5f));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }

        else if (isWallL)
        {
            //_rb.useGravity = false;
            StartCoroutine(afterRun(0.5f));

            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJump();
            }
        }

        else if (isWallF)
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
        //_rb.useGravity = true;
    }

    private void WallJump() //Jumping from a wall. Different then a normal jump!
    {
        if (isWallR)
        {
            verticalVelocity = JumpForce;
            _movement = -transform.right * JumpForce;
            Debug.Log(-transform.right * JumpForce);
        }

        if (isWallL)
        {
            verticalVelocity = JumpForce;
            _movement = transform.right * JumpForce;
            Debug.Log(transform.right * JumpForce);
        }

        if (isWallF)
        {
            verticalVelocity = JumpForce;
            _movement = -transform.forward * JumpForce;
            Debug.Log(transform.forward * JumpForce);
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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if(!_pc.isGrounded && hit.normal.y < 0.15)
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //    {
        //         Debug.DrawRay(hit.point, hit.normal, Color.red, 1.25f);

        //          verticalVelocity = JumpForce;
        //          _movement = hit.moveDirection * speed;
        //    }
        //} 
    }
}
