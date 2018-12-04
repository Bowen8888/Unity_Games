﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SocialAgent : MonoBehaviour {
	private Rigidbody _rigidbody;
	public float speed;
	public float rotateSpeed;  
	private Vector3 destination;
	public bool _talking = false;
	private Vector3 talkingPosition;
	
	// Use this for initialization
	void Start () {
		UpdateDestination();
		_rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
	}

	private void UpdateDestination()
	{
		float xCord = Random.Range(-30, 30);
		float zCord = Random.Range(-18, 18);
		destination = new Vector3(xCord, 0.5f, zCord);
	}
	
	private void FixedUpdate()
	{
		if (_talking)
		{
			transform.position = talkingPosition;
			return;
		}

		GameObject socialAgentAround = SocialAgentAround();
		
		if (socialAgentAround != null)
		{
			Vector3 desiredVelocity = socialAgentAround.transform.position - transform.position;
			var distance = desiredVelocity.magnitude;
			var slowingRange = 4;
		
			if (distance < 2)
			{
				_rigidbody.velocity = new Vector3();
				_talking = true;
				talkingPosition = transform.position;
			}
			else if(distance < slowingRange)
			{
				_rigidbody.velocity = Vector3.Normalize(desiredVelocity) * speed * distance/slowingRange ;
			}
			else
			{
				_rigidbody.velocity = Vector3.Normalize(desiredVelocity) * speed;
			}
		}
		else
		{
			var distance = Vector3.Distance(transform.position, destination);
			var slowingRange = 4;
		
			if (distance < 0.5 || Physics.CheckSphere(destination, 3))
			{
				UpdateDestination();
			}
			else if(distance < slowingRange)
			{
				_rigidbody.velocity = transform.forward * speed * distance/slowingRange ;
			}
			else
			{
				_rigidbody.velocity = transform.forward * speed;
			}
		}

		if (!ReactToObstacles())
		{
			var rotation = Quaternion.LookRotation(destination - transform.position);
			_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,rotateSpeed));
		}
	}

	private GameObject SocialAgentAround()
	{
		var viewRange = 5;
		GameObject[] socialAgents = GameObject.FindGameObjectsWithTag("SocialAgent");
		foreach (var socialAgent in socialAgents)
		{
			if (!socialAgent.Equals(gameObject))
			{
				var distance = Vector3.Distance(transform.position, socialAgent.transform.position);
				if (distance < viewRange)
				{
					return socialAgent;
				}
			}
		}

		return null;
	}

	private bool ReactToObstacles()
	{
		bool toggle = false;
		
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		foreach (var obstacle in obstacles)
		{
			float distance = Vector3.Distance(transform.position, obstacle.transform.position);
			if (distance < 5)
			{
				var rotation = Quaternion.LookRotation(transform.position - obstacle.transform.position);
				_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,(float) (rotateSpeed/Math.Pow(2,distance/4))));
				toggle = true;
			}
		}

		return toggle;
	}
}
