using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour {

    Vector2 mouselook;
    Vector2 smoothV;

    public float Sensitiviy = 5.0f;
    public float Smoothing = 2.0f;

    GameObject Player;

	// Use this for initialization
	void Start () {
        Player = this.transform.parent.gameObject; //Gets the player, which is the parent of the camera (since the camera is nested in there)
	}

    // Update is called once per frame
    void Update() {

        var mouseDirection = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); //Gets the raw input from the mouse and puts this in a vec2 consisting of x and y.

        mouseDirection = Vector2.Scale(mouseDirection, new Vector2(Sensitiviy * Smoothing, Sensitiviy * Smoothing)); //Scale the input from the mouse and multiplies it with the sensitivity.
        smoothV.x = Mathf.Lerp(smoothV.x, mouseDirection.x, 1f / Smoothing); // smooths the x-axis.
        smoothV.y = Mathf.Lerp(smoothV.y, mouseDirection.y, 1f / Smoothing); // smooths the y-axis.
        mouselook += smoothV; //Adds the smoothed amounts to the mouselook

        mouselook.y = Mathf.Clamp(mouselook.y, -90f, 90f); //Makes it so that you can't rotate more then 90 degrees on the y-axis, so you can't look flip over

        transform.localRotation = Quaternion.AngleAxis(-mouselook.y, Vector3.right); //Rotates the camera.
        Player.transform.localRotation = Quaternion.AngleAxis(mouselook.x, Player.transform.up); //Rotates the player.

       // if (!Player.GetComponent<LittlePlayer>().UnlockCamera)
      //  {

      //  }
      //  else
      //  {
      //      transform.localRotation = Quaternion.AngleAxis(-mouselook.y, Vector3.right); //Rotates the camera.
            //Player.transform.localRotation = Player.GetComponent<LittlePlayer>().currentWall.rotation; // sets player to rotation of the wall
            //transform.rotation = Quaternion.FromToRotation(Vector3.right, hit.normal);
     //   }



       // if (!Player.GetComponent<LittlePlayer>().UnlockCamera) // when not wallrunning
       //{
       //     transform.localRotation = Quaternion.AngleAxis(-mouselook.y, Vector3.right); //Rotates the camera.
       //     Player.transform.localRotation = Quaternion.AngleAxis(mouselook.x, Player.transform.up); //Rotates the player.
       //}
       //else //When wallrunning
       //{
       //    transform.localRotation = Quaternion.AngleAxis(-mouselook.y, Vector3.right); //Rotates the camera.
       //     transform.localRotation = Quaternion.AngleAxis(mouselook.x, Vector3.up);
       //    transform.localRotation = Quaternion.AngleAxis(mouselook.x, transform.up); // rotates some other part of the camera

       //     Player.transform.localRotation = Player.GetComponent<LittlePlayer>().currentWall.rotation; // sets player to rotation of the wall!
       //}
    }
}
