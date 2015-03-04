using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	[HideInInspector]
	public int totalScore = 0;
	public int[] targetScores = new int[]{2500, 10000, 50000};
	public float timeLimit = 60 * 5; // Time limit in seconds. Default 5 minutes

	// Use this for initialization
	void Start () {
		totalScore = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.timeSinceLevelLoad >= timeLimit)
		{
			GameOver();
		}
	}

	public void GameOver()
	{
	}
}
