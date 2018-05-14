using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterControlerTest : MonoBehaviour
{
    public float Speed = 5f;
    public float Gravity = 9.8f;
    public float JumpForce = 5;
    private Vector3 _moveVec;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        _moveVec.x = Input.GetAxis("Horizontal");
        _moveVec.z = Input.GetAxis("Vertical");

	}
}
