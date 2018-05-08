using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 10.0f;

    public float JumpForce = 0.0f;
    private int jumpCount = 0;

    public float DashForce = 0.0f;
    private int dashCount = 0;

    private Rigidbody rb;
    Vector3 movement;

    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();     
	}
	
	void Update ()
    {
        RaycastHit hit;
        Ray jumpRay = new Ray(this.transform.position, Vector3.down);
        if (Physics.Raycast(jumpRay, out hit, 1.0f))
        {
            if (hit.transform.tag == "Ground")
            {
                jumpCount = 0;
            }
        }
        if ((Input.GetKeyDown(KeyCode.Space)) && jumpCount <= 1)
        {
            Jump();
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift)) && dashCount > 1)
        {
            Airdash();
        }
    }

        void FixedUpdate()
    {
        movement.z = Input.GetAxis("Vertical");
        movement.x = Input.GetAxis("Horizontal");

        movement = transform.rotation * movement;

        Vector3 gravFix = new Vector3(0, rb.velocity.y, 0);

        rb.velocity = movement * speed;
        rb.velocity += gravFix;
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * JumpForce);
        jumpCount++;
    }

    void Airdash()
    {
        rb.AddForce(Vector3.forward * DashForce);
        dashCount++;
    }
}
