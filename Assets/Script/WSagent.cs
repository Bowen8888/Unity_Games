using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WSagent : MonoBehaviour
{
	private Rigidbody _rigidbody;
	public float speed;
	public float rotateSpeed;  
	private Vector3 destination;
	
	// Use this for initialization
	void Start () {
		UpdateDestination();
//		destination = new Vector3(-30,0.5f,-10);
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
		var distance = Vector3.Distance(transform.position, destination);
		var slowingRange = 4;
		if (distance < 0.5 || Physics.CheckSphere(destination, 3))
		{
			UpdateDestination();
			Debug.Log("new destination " + destination);
		}
		else if(distance < slowingRange)
		{
			_rigidbody.velocity = transform.forward * speed * distance/slowingRange ;
		}
		else
		{
			_rigidbody.velocity = transform.forward * speed;
		}

		if (!ReactToObstacles())
		{
			var rotation = Quaternion.LookRotation(destination - transform.position);
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
			if (distance < 5)
			{
				var rotation = Quaternion.LookRotation(transform.position - obstacle.transform.position);
				_rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation,rotation,(float) (rotateSpeed/distance)));
				toggle = true;
			}
		}

		return toggle;
	}
	
	private GameObject TravelerAround()
	{
		var viewRange = 5;
		GameObject[] socialAgents = GameObject.FindGameObjectsWithTag("Traveler");
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
}
