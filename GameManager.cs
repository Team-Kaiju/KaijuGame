using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	[HideInInspector]
	public int totalScore = 0;
	public int[] targetScores = new int[]{2500, 10000, 50000};
	public float timeLimit = 60 * 5; // Time limit in seconds. Default 5 minutes
	BasicPlayer player;
	ArrayList cullingList;
	public Camera cullingCam;
	Plane[] camPlanes;
	
	// Use this for initialization
	void Start ()
	{
		cullingList = new ArrayList();
		totalScore = 0;
		player = GameObject.FindObjectOfType<BasicPlayer>();
		camPlanes = GeometryUtility.CalculateFrustumPlanes(cullingCam);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
		
		if(Time.timeSinceLevelLoad >= timeLimit || player == null)
		{
			GameOver();
		}
		
		if(player != null)
		{
			foreach(GameObject gameObj in cullingList)
			{
				if(gameObj == null)
				{
					cullingList.Remove(gameObj);
					continue;
				}
				camPlanes = GeometryUtility.CalculateFrustumPlanes(cullingCam);
				if(!GeometryUtility.TestPlanesAABB(camPlanes, gameObj.GetComponent<Collider>().bounds))
				{
					gameObj.SetActive(false);
				} else
				{
					gameObj.SetActive(true);
				}
			}
		}
	}
	
	public void RegisterCulling(GameObject obj)
	{
		cullingList.Add(obj);
	}
	
	public void GameOver()
	{
		Application.LoadLevel(Application.loadedLevelName);
	}
}
