﻿using System;
using UnityEngine;
using Random = System.Random;

public class Agent : MonoBehaviour
{
	private GameObject[] _doorways;
	public float _speed;
	private float _rotateSpeed;
	private bool _chosenGoal1;
	private float _nextActionTime = 0.0f;
	private float _period = 5;

	private Rigidbody _rigidbody;
	// Use this for initialization
	void Start ()
	{
		var rnd = new Random();
		
		_rigidbody = GetComponent<Rigidbody>();
		_speed = UnityEngine.Random.Range(5, 10);
		_rotateSpeed = 10;
		_chosenGoal1 = rnd.NextDouble() < 0.5;
		_doorways = GameObject.FindGameObjectsWithTag("Doorway");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Time.time > _nextActionTime)
		{
			_nextActionTime += _period;
			_chosenGoal1 = !_chosenGoal1;
		}
	}

	private void FixedUpdate()
	{
		_rigidbody.velocity = transform.forward * _speed;

		if (!ReactToObstacles())
		{
			var rotation = Quaternion.LookRotation((_chosenGoal1?_doorways[0].transform.position : _doorways[1].transform.position) - transform.position);
			_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,_rotateSpeed));
		}
	}

	private bool ReactToObstacles()
	{		
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		GameObject[] socialAgents = GameObject.FindGameObjectsWithTag("SocialAgent");
		GameObject[] wanderAgents = GameObject.FindGameObjectsWithTag("WanderAgent");
		GameObject[] travelers = GameObject.FindGameObjectsWithTag("Traveler");

		return ReactToObstacles(obstacles, 5) || ReactToObstacles(socialAgents, 2) || ReactToObstacles(wanderAgents, 2) || ReactToObstacles(travelers, 2);
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

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Doorway"))
		{
			Destroy(gameObject);
		}
	}
}
