using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
	public GameObject objectPrefab;
	public GameObject obstacleContainerPrefab;
	public int N;
	private System.Random rnd = new System.Random();
	
	// Use this for initialization
	void Awake ()
	{
		int count = 0;
		int t = 0;
		while (count < N)
		{
			float xCord = Random.Range(-23, 25);
			float zCord = Random.Range(-20, 20);
			bool canPlace = true;
			GameObject[] obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
			foreach (var obstacle in obstacles)
			{
				if (Vector3.Distance(obstacle.transform.position, new Vector3(xCord, 2, zCord)) < 11)
				{
					canPlace = false;
					break;
				}
			}

			if (canPlace)
			{
				GenerateObstacle(xCord, zCord);
				count++;
			}

			if (t++ > 1000)
			{
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void GenerateObstacle(float containerPositionX, float containerPositionZ)
	{
		GameObject obstacleContainer = Instantiate(obstacleContainerPrefab, new Vector3(), Quaternion.identity);
		
		GameObject obstacle = Instantiate(objectPrefab,new Vector3(0,2,0), Quaternion.identity);
		float width = (float) (rnd.NextDouble() * 3 + 3);
		float height = (float) (rnd.NextDouble() * 3 + 3);
		obstacle.transform.localScale = new Vector3(width,4,height);
		float obstacleX = obstacle.transform.position.x;
		float obstacleZ = obstacle.transform.position.z;
		

		
		obstacle.transform.parent = obstacleContainer.transform;	
		GenerateSideWings(obstacleContainer,obstacleX,obstacleZ,height,width,0);
		GenerateSideWings(obstacleContainer,obstacleX,obstacleZ,height,width,1);
		GenerateSideWings(obstacleContainer,obstacleX,obstacleZ,height,width,2);

		obstacleContainer.transform.rotation = Quaternion.Euler(0,Random.Range(0,360),0);
		obstacleContainer.transform.position = new Vector3(containerPositionX,0,containerPositionZ);
	}

	private void GenerateSideWings(GameObject obstacleContainer, float obstacleX, float obstacleZ, float height, float width,int sideIndex)
	{
		if (rnd.NextDouble() < 0.5)
		{
			GameObject side = Instantiate(objectPrefab,new Vector3(0,2,0), Quaternion.identity);
			float sideWidth = (float) (rnd.NextDouble() * 3 + 1);
			float sideHeight = (float) (rnd.NextDouble() * 3 + 1);
			side.transform.localScale = new Vector3(sideWidth, 4, sideHeight);
			bool positive = rnd.NextDouble() < 0.5;
			float shiftOffset = (float) rnd.NextDouble();
		
			if (sideIndex == 1) //south
			{
				side.transform.position = new Vector3(obstacleX+ ((positive) ?shiftOffset :-shiftOffset),2,obstacleZ-height/2-sideHeight/2);
			}
			else if (sideIndex == 0)//north
			{
				side.transform.position = new Vector3(obstacleX+ ((positive) ?shiftOffset :-shiftOffset),2,obstacleZ+height/2+sideHeight/2);
			}
			else if (sideIndex == 2)//east
			{
				side.transform.position = new Vector3(obstacleX+width/2+sideWidth/2,2,obstacleZ+ ((positive) ?shiftOffset :-shiftOffset));
			}
			
			side.transform.parent = obstacleContainer.transform;
		}
	}
}
