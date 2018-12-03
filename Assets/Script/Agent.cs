using System;
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

		if (!ReactToObstacles())
		{
			var rotation = Quaternion.LookRotation(goal2.transform.position - transform.position);
			_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,rotateSpeed));
		}
	}

	private bool ReactToObstacles()
	{
		bool toggle = false;
		
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach (var obstacle in obstacles)
		{
			float distance = Vector3.Distance(transform.position, obstacle.transform.position);
			if (distance < 8)
			{
				var rotation = Quaternion.LookRotation(transform.position - obstacle.transform.position);
				_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,(float) (rotateSpeed/Math.Pow(2,distance/4))));
				toggle = true;
			}
		}

		return toggle;
	}
	
	private void Move(Vector3 vector3)
	{
		_rigidbody.AddForce(vector3);
	}
}
