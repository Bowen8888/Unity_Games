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
		
		_rigidbody.velocity = transform.forward * _speed;
	}

	private bool ReactToObstacles()
	{		
		GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
		GameObject[] socialAgents = GameObject.FindGameObjectsWithTag("SocialAgent");
		GameObject[] wanderAgents = GameObject.FindGameObjectsWithTag("WanderAgent");
		ReactToWall();

		return ReactToObstacles(obstacles, 7) || ReactToObstacles(wanderAgents, 2) || ReactToObstacles(socialAgents, 2);
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

	private void ReactToWall()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
		{
			if (hit.collider.CompareTag("Wall"))
			{
				var rotation = Quaternion.LookRotation(transform.position - hit.point);
				_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,_rotateSpeed/hit.distance));
				Debug.DrawLine(transform.position, hit.point, Color.red);
			}
		}
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
