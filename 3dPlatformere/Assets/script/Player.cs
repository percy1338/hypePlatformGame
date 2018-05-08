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

    private bool Slideing;
    public float SlideForce;

    private Rigidbody rb;
    Vector3 movement;
    private CapsuleCollider cap;
    private CapsuleCollider capsmal;

    void Start ()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cap = gameObject.GetComponent<CapsuleCollider>();
        capsmal = gameObject.GetComponent<CapsuleCollider>();
    }
	
	void Update ()
    {
        RaycastHit hit;
        Ray jumpRay = new Ray(this.transform.position, Vector3.down);
        if (Physics.Raycast(jumpRay, out hit, 1.0f))
        {
            if (hit.transform.tag == "Ground" || hit.transform.tag == "Wall")
            {
                jumpCount = 0;
            }
        }
        if ((Input.GetKeyDown(KeyCode.Space)) && jumpCount <= 1)
        {
            Jump();
        }   

        if ((Input.GetKey(KeyCode.LeftShift)))
        {
            Slideing = true;
            cap.height = 0.5f;
            rb.AddForce(transform.forward * SlideForce);
        }
        if((Input.GetKeyUp(KeyCode.LeftShift)))
        {
            Slideing = false;
            cap.height = 2;
        }
    }

    void FixedUpdate()
    {
        movement.x = 0;
        movement.z = 0;
        if(!Slideing)
        {
            if (CanMove(transform.forward * Input.GetAxis("Vertical")))
            {
                movement.z = Input.GetAxis("Vertical");
            }

            if (CanMove(transform.right * Input.GetAxis("Horizontal")))
            {
                movement.x = Input.GetAxis("Horizontal");
            }
        }

        movement = transform.rotation * movement;

        Vector3 gravFix = new Vector3(0, rb.velocity.y, 0);

        rb.velocity = movement * speed;
        rb.velocity += gravFix;

        Debug.Log(Slideing);
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * JumpForce);
        jumpCount++;
    }

    void Slide()
    {
        
    }

    void Airdash()
    {
        rb.AddForce(Vector3.forward * DashForce);
        dashCount++;
    }

    bool CanMove(Vector3 direction)
    {
        float distanceToPoints = cap.height / 2 - cap.radius;

        Vector3 point1 = transform.position + cap.center + Vector3.up * distanceToPoints;
        Vector3 point2 = transform.position + cap.center - Vector3.up * distanceToPoints;
        float radius = cap.radius * 0.95f;
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
