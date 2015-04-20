using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	[HideInInspector]
	public int totalScore = 0;
	public int[] targetScores = new int[]{2500, 5000, 10000};
	public float timeLimit = 60 * 5; // Time limit in seconds. Default 5 minutes
	BasicPlayer player;
	ArrayList cullingList;
	public Camera cullingCam;
	Plane[] camPlanes;
	
	IEnumerator routine;
	
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
			Application.LoadLevel("TitleScene");
			return;
		}
		
		if(Time.timeSinceLevelLoad >= timeLimit || player == null)
		{
			GameOver();
		}
		
		if(player != null && routine == null)
		{
			cullingList.Remove(null);
			routine = CullObjects(cullingList);
			StartCoroutine(routine);
		}
	}
	
	IEnumerator CullObjects(ArrayList list)
	{
		foreach(GameObject gameObj in list)
		{
			if(gameObj == null)
			{
				list.Remove(gameObj);
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
        
		routine = null;
		
		yield return null;
	}
	
	public void RegisterCulling(GameObject obj)
	{
		cullingList.Add(obj);
	}
    
    public void GameOver()
	{
		PlayerPrefs.SetInt("Score", totalScore);
		
		for(int i = 0; i < 10; i++)
		{
			if(!PlayerPrefs.HasKey("HS_SCORE_" + i) || PlayerPrefs.GetInt("HS_SCORE_" + i) < PlayerPrefs.GetInt("Score")) // Empty or smaller score found
			{
				Application.LoadLevel("NewHighscoreScreen");
				return;
			}
		}
		
		Application.LoadLevel("HighscoreScene");
	}
}
