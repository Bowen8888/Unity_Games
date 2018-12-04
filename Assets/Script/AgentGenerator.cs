using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentGenerator : MonoBehaviour
{
	public GameObject SocialAgentPrefab;
	public int SocialAgentAmount;
	public GameObject TravellerAgentPrefab;
	public int TravellerAgentAmount;
	public GameObject WanderAgentPrefab;
	public int WanderAgentAmount;
	
	// Use this for initialization
	void Start () {
		
		//Generate Traveller Agent
		for (int i = 0; i < TravellerAgentAmount; i++)
		{
			Instantiate(TravellerAgentPrefab);
		}
		
		//Generate Social Agent
		SpawnAtRandomPosition(SocialAgentPrefab, SocialAgentAmount);
		
		//Generate Wander Agent
		SpawnAtRandomPosition(WanderAgentPrefab, WanderAgentAmount);
	}

	private void SpawnAtRandomPosition(GameObject prefab, int N)
	{
		for (int i = 0; i < N; i++)
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

			Instantiate(prefab, spawnPos, Quaternion.identity);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
