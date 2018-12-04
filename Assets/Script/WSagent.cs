using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WSagent : MonoBehaviour
{
	private Rigidbody _rigidbody;
	private float _speed;
	private float _rotateSpeed;  
	private Vector3 destination;
	private GameObject _target;

	// Use this for initialization
	void Start () {
		UpdateDestination();
		_speed = Random.Range(5, 10);
		_rotateSpeed = 10;
		_rigidbody = GetComponent<Rigidbody>();
		_target = null;
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
		if (_target != null)
		{
			Vector3 targetDestination = _target.GetComponent<Agent>().GetDestination();
			destination = Vector3.Normalize(targetDestination - _target.transform.position)*2 + _target.transform.position;
		}
		else if (Vector3.Distance(transform.position, destination) < 0.5 || Physics.CheckSphere(destination, 3))
		{
			UpdateDestination();
			Debug.Log("new destination " + destination);
		}

		CheckTravelerAround();
		
		_rigidbody.velocity = transform.forward * _speed;

		if (!ReactToObstacles())
		{
			var rotation = Quaternion.LookRotation(destination - transform.position);
			_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,_rotateSpeed));
		}
	}

	private bool ReactToObstacles()
	{		
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		GameObject[] socialAgents = GameObject.FindGameObjectsWithTag("SocialAgent");
		GameObject[] wanderAgents = GameObject.FindGameObjectsWithTag("WanderAgent");

		return ReactToObstacles(obstacles, 5) || ReactToObstacles(wanderAgents, 2) || ReactToObstacles(socialAgents, 2);
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
	
	private void CheckTravelerAround()
	{
		var viewRange = 5;
		GameObject[] travelers = GameObject.FindGameObjectsWithTag("Traveler");
		foreach (var traveler in travelers)
		{
			var distance = Vector3.Distance(transform.position, traveler.transform.position);
			if (distance < viewRange)
			{
				_target = traveler;
				return;
			}
		}
	}
}
