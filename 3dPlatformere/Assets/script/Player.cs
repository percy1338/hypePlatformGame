using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;
    public float MaxSpeed = 20;
    public float JumpForce = 0f;

    private bool _grounded;
    private Rigidbody rb;
    Vector3 movement;

    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();     
	}
	
	void Update ()
    {
    }

    void FixedUpdate()
    {
        movement.z = Input.GetAxis("Vertical");
        movement.x = Input.GetAxis("Horizontal");

        movement = transform.rotation * movement;

        Vector3 gravFix = new Vector3(0, rb.velocity.y, 0);

        if(rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * MaxSpeed;
        }

        RaycastHit hit;
        Ray jumpRay = new Ray(this.transform.position, Vector3.down);
        if(Physics.Raycast(jumpRay,out hit, 1.0f))
        {
            if(hit.transform.tag == "Ground")
            {
                _grounded = true;
            }
        }

        if ((Input.GetKeyDown(KeyCode.Space)) && _grounded)
        {
            
            rb.AddForce(Vector3.up * JumpForce);
            _grounded = false;
        }      
        rb.velocity = movement * speed;
        rb.velocity += gravFix;   
    }
}
