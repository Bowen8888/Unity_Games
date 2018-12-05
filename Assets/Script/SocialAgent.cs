﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SocialAgent : MonoBehaviour {
	private Rigidbody _rigidbody;
	private float _speed;
	private float _rotateSpeed;  
	private Vector3 destination;
	private bool _talking;
	private Vector3 talkingPosition; 
	private float nextActionTime = 0.0f;
	private float freeUntil = 2f;
	
	// Use this for initialization
	void Start () {
		UpdateDestination();
		_speed = Random.Range(5, 10);
		_rotateSpeed = 20;
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
			if (Time.time > nextActionTime ) {
				_talking = false;
				freeUntil = Time.time + Random.Range(3,6);
			}
			return;
		}

		GameObject socialAgentAround = TalkingAvailableSocialAgentAround();

		if (socialAgentAround != null && Time.time > freeUntil)
		{
			Vector3 desiredVelocity = socialAgentAround.transform.position - transform.position;
			var distance = desiredVelocity.magnitude;
			var slowingRange = 4;

			if (distance < 2)
			{
				_rigidbody.velocity = new Vector3();
				_talking = true;
				nextActionTime = Time.time + Random.Range(3,6);
				talkingPosition = transform.position;
			}
			else if(distance < slowingRange)
			{
				_rigidbody.velocity = Vector3.Normalize(desiredVelocity) * _speed * distance/slowingRange ;
			}
			else
			{
				_rigidbody.velocity = Vector3.Normalize(desiredVelocity) * _speed;
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
				_rigidbody.velocity = transform.forward * _speed * distance/slowingRange ;
			}
			else
			{
				_rigidbody.velocity = transform.forward * _speed;
			}
		}

		if (!ReactToObstacles())
		{
			var rotation = Quaternion.LookRotation(destination - transform.position);
			_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,_rotateSpeed));
		}
	}

	private GameObject TalkingAvailableSocialAgentAround()
	{
		var viewRange = 5;
		GameObject[] socialAgents = GameObject.FindGameObjectsWithTag("SocialAgent");
		foreach (var socialAgent in socialAgents)
		{
			if (!socialAgent.Equals(gameObject))
			{
				var distance = Vector3.Distance(transform.position, socialAgent.transform.position);
				if (distance < viewRange && Time.time > socialAgent.GetComponent<SocialAgent>().freeUntil)
				{
					return socialAgent;
				}
			}
		}

		return null;
	}

	private bool ReactToObstacles()
	{		
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		GameObject[] travelerAgents = GameObject.FindGameObjectsWithTag("Traveler");
		GameObject[] wanderAgents = GameObject.FindGameObjectsWithTag("WanderAgent");
		GameObject[] socialAgents = GameObject.FindGameObjectsWithTag("SocialAgent");

		return ReactToObstacles(obstacles, 5) || ReactToObstacles(wanderAgents, 2) || ReactToObstacles(travelerAgents, 2) || ReactToObstacles(socialAgents,2);
	}

	private bool ReactToObstacles(GameObject[] obstacles, float avoidDist)
	{
		bool toggle = false;
				
		foreach (var obstacle in obstacles)
		{
			if (obstacle == gameObject)
			{
				continue;
			}
			
			float distance = Vector3.Distance(transform.position, obstacle.transform.position);
			if (distance < avoidDist)
			{
				var rotation = Quaternion.LookRotation(transform.position - obstacle.transform.position);
				_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,_rotateSpeed/distance));
				toggle = true;
			}
		}

		return toggle;
	}
}
