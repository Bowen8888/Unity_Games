using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
	public GameObject goal1;
	public GameObject goal2;
	public float speed;
	public float rotateSpeed;

	private Rigidbody _rigidbody;
	// Use this for initialization
	void Start ()
	{
		_rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		GetInput();
	}

	private void GetInput()
	{		
		if(Input.GetKey(KeyCode.W))
		{
			Move(Vector3.forward*Time.deltaTime*speed);
		}
		if(Input.GetKey(KeyCode.S))
		{
			Move(Vector3.back*Time.deltaTime*speed);
		}
		if(Input.GetKey(KeyCode.A))
		{
			Move(Vector3.left*Time.deltaTime*speed);
		}
		if(Input.GetKey(KeyCode.D))
		{
			Move(Vector3.right*Time.deltaTime*speed);
		}

	}

	private void FixedUpdate()
	{
		_rigidbody.velocity = transform.forward * speed;

		var rotation = Quaternion.LookRotation(goal2.transform.position - transform.position);
		_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,rotateSpeed));
	}

	private void Move(Vector3 vector3)
	{
		_rigidbody.AddForce(vector3);
	}
}
