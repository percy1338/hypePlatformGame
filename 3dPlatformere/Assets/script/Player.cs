﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("basic player Properties")]
    public float speed = 10.0f;

    [Header("Jump Properties")]
    public float JumpForce = 0.0f;
    private int _jumpCount = 0;

    [Header("dash Properties")]
    public float DashForce = 0.0f;
    private int _dashCount = 0;

    [Header ("Slide Properties")]
    public float SlideForce;
    public float MaxSlideDistance = 120;  
    private Vector3 _slideVec;
    private float _curSlideDistance;    
    private bool Slideing;

    private Rigidbody _rb;
    private Vector3 _movement;
    private CapsuleCollider _cap;

    void Start ()
    {
        _rb = gameObject.GetComponent<Rigidbody>();
        _cap = gameObject.GetComponent<CapsuleCollider>();
    }
	
	void Update ()
    {
        RaycastHit hit;
        Ray jumpRay = new Ray(this.transform.position, Vector3.down);
        if (Physics.Raycast(jumpRay, out hit, 1.0f))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Wall")
            {
                _jumpCount = 0;
            }
        }
        if ((Input.GetKeyDown(KeyCode.Space)) && _jumpCount <= 1)
        {
            Jump();
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift)))
        {
            _slideVec = transform.forward;
        }
        if ((Input.GetKey(KeyCode.LeftShift)))
        {          
            Slideing = true;
        }
        if((Input.GetKeyUp(KeyCode.LeftShift)))
        {
            Slideing = false;          
        }
        if(Slideing)
        {
            _curSlideDistance += 1f;
        }
        if(_curSlideDistance >= MaxSlideDistance)
        {
            Slideing = false;
            _curSlideDistance = 0;
        }
        if(Slideing)
        {
            _cap.height = 0.5f;
            _rb.AddForce(_slideVec * SlideForce);
        }
        else
        {
            _cap.height = 2;
        }
    }
    
    void FixedUpdate()
    {
        _movement.x = 0;
        _movement.z = 0;
        if(!Slideing)
        {
            if (CanMove(transform.forward * Input.GetAxis("Vertical")))
            {
                _movement.z = Input.GetAxis("Vertical");
            }

            if (CanMove(transform.right * Input.GetAxis("Horizontal")))
            {
                _movement.x = Input.GetAxis("Horizontal");
            }
        }

        _movement = transform.rotation * _movement;

        Vector3 gravFix = new Vector3(0, _rb.velocity.y, 0);

        _rb.velocity = _movement * speed;
        _rb.velocity += gravFix;

        Debug.Log(Slideing);
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * JumpForce);
        _jumpCount++;
    }

    void Slide()
    {
        
    }

    void Airdash()
    {
        _rb.AddForce(Vector3.forward * DashForce);
        _dashCount++;
    }

    bool CanMove(Vector3 direction)
    {
        float distanceToPoints = _cap.height / 2 - _cap.radius;

        Vector3 point1 = transform.position + _cap.center + Vector3.up * distanceToPoints;
        Vector3 point2 = transform.position + _cap.center - Vector3.up * distanceToPoints;
        float radius = _cap.radius * 0.95f;
        float castDistance = 0.5f;

        RaycastHit[] hits = Physics.CapsuleCastAll(point1, point2, radius, direction, castDistance);

        foreach (RaycastHit objectHit in hits)
        {
            if (objectHit.transform.tag == "Wall")
            {
                Debug.Log("Hit");
                return false;
            }
        }
        return true;
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
