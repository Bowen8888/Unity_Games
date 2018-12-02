using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
	public GameObject goal1;
	public GameObject goal2;
	private NavMeshAgent _navMeshAgent;
	public float speed;
	
	// Use this for initialization
	void Start ()
	{
		_navMeshAgent = GetComponent<NavMeshAgent>();
		_navMeshAgent.SetDestination(goal1.transform.position);
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
			_navMeshAgent.Move(Vector3.forward*Time.deltaTime*speed);
		}
		if(Input.GetKey(KeyCode.S))
		{
			_navMeshAgent.Move(Vector3.back*Time.deltaTime*speed);
		}
		if(Input.GetKey(KeyCode.A))
		{
			_navMeshAgent.Move(Vector3.left*Time.deltaTime*speed);
		}
		if(Input.GetKey(KeyCode.D))
		{
			_navMeshAgent.Move(Vector3.right*Time.deltaTime*speed);
		}

	}
}
