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
	private float nextActionTime;
	private float freeUntil = 1f;
	private SocialCircle _socialCircle;
	
	// Use this for initialization
	void Start () {
		UpdateDestination();
		_speed = Random.Range(5, 10);
		_rotateSpeed = 20;
		_rigidbody = GetComponent<Rigidbody>();
		_socialCircle = null;
		nextActionTime = Random.Range(3, 10);
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
		if (_socialCircle != null)
		{
			if (Time.time > nextActionTime ) {
				_socialCircle.Leave();
				_socialCircle = null;
				freeUntil = Time.time + Random.Range(3,6);
			}
		}

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

			nextActionTime = Time.time + Random.Range(3,10);
			_socialCircle.Join();
		}

		if (_socialCircle != null)
		{
			destination = _socialCircle.Center;
		}
		
		Vector3 desiredVelocity = destination - transform.position;
		var distance = desiredVelocity.magnitude;
		var slowingRange = 4;
		var facingCenter = false;
		
		if (_socialCircle!=null && Math.Abs(distance - _socialCircle.Radius) < 0.2)
		{
			_rigidbody.velocity = new Vector3();
			facingCenter = true;
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
		

		if (facingCenter || !ReactToObstacles())
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

		return ((_socialCircle!=null)?ReactToObstacles(socialAgents,1):ReactToObstacles(socialAgents,2)) || ReactToObstacles(obstacles, 5) || ReactToObstacles(wanderAgents, 2) || ReactToObstacles(travelerAgents, 2) ;
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

}
