﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGenerator : MonoBehaviour
{
	public GameObject socialAgentPrefab;
	public int socialAgentAmount;
	public GameObject TravellerAgentPrefab;
	public int TravellerAgentAmount;
	
	// Use this for initialization
	void Start () {
		
		//Generate Traveller Agent
		for (int i = 0; i < TravellerAgentAmount; i++)
		{
			Instantiate(TravellerAgentPrefab);
		}
		
		//Generate Social Agent
		for (int i = 0; i < socialAgentAmount; i++)
		{
			float xCord = Random.Range(-30, 30);
			float zCord = Random.Range(-18, 18);
			var spawnPos = new Vector3(xCord, 0.5f, zCord);
			while(Physics.CheckSphere(spawnPos, 3))
			{
				xCord = Random.Range(-30, 30);
				zCord = Random.Range(-18, 18);
				spawnPos = new Vector3(xCord, 0.5f, zCord);
			}

			Instantiate(socialAgentPrefab, spawnPos, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}