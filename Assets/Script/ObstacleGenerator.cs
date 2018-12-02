using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
	public GameObject objectPrefab;
	public GameObject obstacleContainerPrefab;

	// Use this for initialization
	void Start ()
	{
		GenerateObstacle(Random.Range(-20,20), Random.Range(-20,20));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void GenerateObstacle(float containerPositionX, float containerPositionZ)
	{
		GameObject obstacleContainer = Instantiate(obstacleContainerPrefab, new Vector3(), Quaternion.identity);
		
		System.Random rnd = new System.Random();
		GameObject obstacle = Instantiate(objectPrefab,new Vector3(0,2,0), Quaternion.identity);
		float width = (float) (rnd.NextDouble() * 2 + 2);
		float height = (float) (rnd.NextDouble() * 2 + 2);
		obstacle.transform.localScale = new Vector3(width,4,height);
		float obstacleX = obstacle.transform.position.x;
		float obstacleZ = obstacle.transform.position.z;

		obstacle.transform.parent = obstacleContainer.transform;		
		//check for north side
		if (rnd.NextDouble() < 0.5)
		{
			GameObject side = Instantiate(objectPrefab,new Vector3(0,2,0), Quaternion.identity);
			float sideWidth = (float) (rnd.NextDouble() * 4 + 1);
			float sideHeight = (float) (rnd.NextDouble() * 4 + 1);
			side.transform.localScale = new Vector3(sideWidth,4,sideHeight);
			bool positive = rnd.NextDouble() < 0.5;
			float shiftOffset = (float) rnd.NextDouble();
			side.transform.position = new Vector3(obstacleX+ ((positive) ?shiftOffset :-shiftOffset),2,obstacleZ+height/2+sideHeight/2);

			side.transform.parent = obstacleContainer.transform;
		}
		//check for south side
		if (rnd.NextDouble() < 0.5)
		{
			GameObject side = Instantiate(objectPrefab,new Vector3(0,2,0), Quaternion.identity);
			float sideWidth = (float) (rnd.NextDouble() * 4 + 1);
			float sideHeight = (float) (rnd.NextDouble() * 4 + 1);
			side.transform.localScale = new Vector3(sideWidth, 4, sideHeight);
			bool positive = rnd.NextDouble() < 0.5;
			float shiftOffset = (float) rnd.NextDouble();
			side.transform.position = new Vector3(obstacleX+ ((positive) ?shiftOffset :-shiftOffset),2,obstacleZ-height/2-sideHeight/2);
			
			side.transform.parent = obstacleContainer.transform;
		}
		//check for east side
		if (rnd.NextDouble() < 0.5)
		{
			GameObject side = Instantiate(objectPrefab,new Vector3(0,2,0), Quaternion.identity);
			float sideWidth = (float) (rnd.NextDouble() * 4 + 1);
			float sideHeight = (float) (rnd.NextDouble() * 4 + 1);
			side.transform.localScale = new Vector3(sideWidth,4,sideHeight);
			bool positive = rnd.NextDouble() < 0.5;
			float shiftOffset = (float) rnd.NextDouble();
			side.transform.position = new Vector3(obstacleX+width/2+sideWidth/2,2,obstacleZ+ ((positive) ?shiftOffset :-shiftOffset));
			
			side.transform.parent = obstacleContainer.transform;
		}

		obstacleContainer.transform.rotation = Quaternion.Euler(0,Random.Range(0,360),0);
		obstacleContainer.transform.position = new Vector3(containerPositionX,2,containerPositionZ);
	}
}
