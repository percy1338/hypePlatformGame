using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    public float speed = 10.0f;
    private bool _grounded;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked; //Locks the cursor in the middle so it can't go out of the screen. 
	}
	
	// Update is called once per frame
	void Update () {
        
        float translation = Input.GetAxis("Vertical") * speed; //Checks if A / D are pressed (happens in the GetAxis) and if so, multiplies it with the speed.
        float strafe = Input.GetAxis("Horizontal") * speed; //Checks if W / S are pressed (happens in GetAxis) and if so, multiplies it with the speed.
        translation *= Time.deltaTime; //Multplies the speed with the delta time, so it isn't frame depended.
        strafe *= Time.deltaTime; //Multplies the speed with the delta time, so it isn't frame depended.

        transform.Translate(strafe, 0, translation); //Makes the actual character move.

        RaycastHit hit; // used for grounded checking.

        Vector3 physicsCentre = this.transform.position + this.GetComponent<CapsuleCollider>().center; //Sets the center to the middle of the character.
        //Debug.DrawRay(physicsCentre, Vector3.down, Color.red, 1.2f); //Usefull for debugging.

        if (Physics.Raycast(physicsCentre, Vector3.down, out hit, 1.2f)) //Checks if the raycast hits the floor.
        {
            if (hit.transform.gameObject.tag != "Player") //Makes sure it doesn't check ffor itself, else it would always be grounded.
            {
                _grounded = true;
            }
        }
        else
        {
            _grounded = false;
        }



        if ((Input.GetKeyDown(KeyCode.Space)) && _grounded == true )
        {
            this.GetComponent<Rigidbody>().AddForce(Vector3.up * 200); //Lets you jump as long as grounded is true and spacebar is pressed.
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None; //Lets you control the cursor again.
        }
	}
}
