using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SocialAgent : MonoBehaviour {
	private Rigidbody _rigidbody;
	private float _speed;
	private float _rotateSpeed;  
	private Vector3 destination;
	private float nextActionTime = 0.0f;
	private float freeUntil = 2f;
	private SocialCircle _socialCircle;
	
	// Use this for initialization
	void Start () {
		UpdateDestination();
		_speed = Random.Range(5, 10);
		_rotateSpeed = 20;
		_rigidbody = GetComponent<Rigidbody>();
		_socialCircle = null;
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
//		if (_socialCircle != null)
//		{
//			if (Time.time > nextActionTime ) {
//				_socialCircle.Leave();
//				_socialCircle = null;
//				freeUntil = Time.time + Random.Range(3,6);
//			}
//		}

		GameObject socialAgentAround = TalkingAvailableSocialAgentAround();

		if (socialAgentAround != null && _socialCircle == null && Time.time > freeUntil)
		{
			SocialAgent socialAgent = socialAgentAround.GetComponent<SocialAgent>();
			if (socialAgent._socialCircle == null)
			{
				_socialCircle = new SocialCircle(transform.position + Vector3.Normalize(transform.forward));
			}
			else
			{
				_socialCircle = socialAgent._socialCircle;
			}

			_socialCircle.Join();
//			Vector3 desiredVelocity = socialAgentAround.transform.position - transform.position;
//			var distance = desiredVelocity.magnitude;
//			var slowingRange = 4;
//
//			if (distance < 2)
//			{
//				_rigidbody.velocity = new Vector3();
//				_talking = true;
//				nextActionTime = Time.time + Random.Range(3,6);
//				talkingPosition = transform.position;
//			}
//			else if(distance < slowingRange)
//			{
//				_rigidbody.velocity = Vector3.Normalize(desiredVelocity) * _speed * distance/slowingRange ;
//			}
//			else
//			{
//				_rigidbody.velocity = Vector3.Normalize(desiredVelocity) * _speed;
//			}
		}

		if (_socialCircle != null)
		{
			destination = _socialCircle.Center;
		}
		
		Vector3 desiredVelocity = destination - transform.position;
		var distance = desiredVelocity.magnitude;
		var slowingRange = 4;

		if (_socialCircle!=null && Math.Abs(distance - _socialCircle.Radius) < 0.2)
		{
			_rigidbody.velocity = new Vector3();
			nextActionTime = Time.time + Random.Range(3,6);
			if (_socialCircle.MemberCount == 1)
			{
				_socialCircle.Leave();
				_socialCircle = null;
			}
		}
		else if (distance < 0.5 || Physics.CheckSphere(destination, 3) && _socialCircle == null)
		{
			UpdateDestination();
		}
		else if(distance < slowingRange)
		{
			if (_socialCircle != null && distance < _socialCircle.Radius)
			{
				_rigidbody.velocity = -transform.forward * _speed ;
			}
			else
			{
				_rigidbody.velocity = transform.forward * _speed * distance/slowingRange;	
			}
		}
		else
		{
			_rigidbody.velocity = transform.forward * _speed;
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

		return ReactToObstacles(socialAgents,1) || ReactToObstacles(obstacles, 5) || ReactToObstacles(wanderAgents, 2) || ReactToObstacles(travelerAgents, 2) ;
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
