using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittlePlayer : MonoBehaviour
{
    public float speed = 10.0f;
    public float MaxSpeed = 20;
    public float JumpForce = 0f;

    private bool _grounded;
    private Rigidbody rb;
    private CapsuleCollider cap;
    Vector3 movement;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        cap = gameObject.GetComponent<CapsuleCollider>();
    }

    void Update()
    {
    }

    void FixedUpdate()
    {
        movement.x = 0;
        movement.z = 0;


        if (CanMove(transform.forward * Input.GetAxis("Vertical")))
        {
            movement.z = Input.GetAxis("Vertical");
        }

        if (CanMove(transform.right * Input.GetAxis("Horizontal")))
        {
            movement.x = Input.GetAxis("Horizontal");
        }

        movement = transform.rotation * movement;

        Vector3 gravFix = new Vector3(0, rb.velocity.y, 0);

        if (rb.velocity.magnitude > MaxSpeed)
        {
            rb.velocity = rb.velocity.normalized * MaxSpeed;
        }

        RaycastHit hit;
        Ray jumpRay = new Ray(this.transform.position, Vector3.down);
        if (Physics.Raycast(jumpRay, out hit, 1.0f))
        {
            if (hit.transform.tag == "Ground")
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
}