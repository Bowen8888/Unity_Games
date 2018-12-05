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
		_rotateSpeed = 20;
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
			if (Vector3.Distance(transform.position, _target.transform.position) > 10)
			{
				_target = null;
			}
			else
			{
				Vector3 targetDestination = _target.GetComponent<Agent>().GetDestination();
				destination = Vector3.Normalize(targetDestination - _target.transform.position)*2 + _target.transform.position;
			}
		}
		else if (Vector3.Distance(transform.position, destination) < 0.5 || Physics.CheckSphere(destination, 3))
		{
			UpdateDestination();
			Debug.Log("new destination " + destination);
		}

		CheckTravelerAround();
		

		if (!ReactToObstacles())
		{
			var rotation = Quaternion.LookRotation(destination - transform.position);
			_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,_rotateSpeed));
		}
		
		var distance = Vector3.Distance(transform.position, destination);
		var slowingRange = 4;
		
		if (_target != null)
		{
			_rigidbody.velocity = transform.forward * _speed;
		}
		else if ((distance < 0.1 || Physics.CheckSphere(destination, 3)))
		{
			UpdateDestination();
		}
		else if(distance < slowingRange)
		{
			_rigidbody.velocity = transform.forward * _speed * distance/slowingRange;
		}
		else
		{
			_rigidbody.velocity = transform.forward * _speed;
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
		float minDist = avoidDist + 1;
		Vector3 resultingVector = new Vector3();
				
		foreach (var obstacle in obstacles)
		{
			if (obstacle == gameObject)
			{
				continue;
			}
			
			float distance = Vector3.Distance(transform.position, obstacle.transform.position);
			if (distance < avoidDist)
			{
				if (minDist > distance)
				{
					minDist = distance;
					resultingVector = transform.position - obstacle.transform.position;
					toggle = true;
				}
			}
		}
		
		if (transform.position.z < -17)
		{
			var wallPosition = new Vector3(transform.position.x, 0.5f, -19.9f);
			var distance = Vector3.Distance(transform.position, wallPosition);
			if (minDist > distance)
			{
				minDist = distance;
				resultingVector = transform.position - wallPosition;
				toggle = true;
			}
		}

		if (transform.position.z > 18)
		{
			
			var wallPosition = new Vector3(transform.position.x, 0.5f, 21.31657f);
			var distance = Vector3.Distance(transform.position, wallPosition);
			if (minDist > distance)
			{
				minDist = distance;
				resultingVector = transform.position - wallPosition;
				toggle = true;
			}
		}

		if (transform.position.x < -34)
		{
			var wallPosition = new Vector3(-37.58638f, 0.5f, transform.position.z);
			var distance = Vector3.Distance(transform.position, wallPosition);
			if (minDist > distance)
			{
				minDist = distance;
				resultingVector = transform.position - wallPosition;
				toggle = true;
			}
		}

		if (toggle)
		{
			var rotation = Quaternion.LookRotation(resultingVector);
			_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,_rotateSpeed*2/minDist));
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
