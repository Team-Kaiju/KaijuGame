using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicUI : MonoBehaviour {
	public GUISkin uiSkin; // The base skin for the GUI
	public Text scoreTxt;
	public Text timeTxt;
	public Text healthTxt;
	public Text destroyableTxt;
	GameManager manager;
	BasicPlayer player;
	Camera cam;
	string[] scoreLvls = new string[]{"Bronze", "Silver", "Gold"};
	int scoreIdx = 0;

	// Use this for initialization
	void Start()
	{
		manager = GameObject.FindObjectOfType<GameManager>();
		player = GameObject.FindObjectOfType<BasicPlayer>();
		cam = GameObject.FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update()
	{
		if(scoreIdx < manager.targetScores.Length && manager.totalScore >= manager.targetScores[scoreIdx])
		{
			scoreIdx++;
		}

        int targetScore = 0;
        string scoreName = "Highscore";

        if(scoreIdx < manager.targetScores.Length)
        {
            targetScore = manager.targetScores[scoreIdx];
            scoreName = scoreLvls[scoreIdx];
        } else
        {
            targetScore = PlayerPrefs.GetInt("HS_SCORE_" + 0);
        }

        scoreTxt.text = "Score: " + manager.totalScore + " / " + targetScore + " (" + scoreName + ")";
		
		
		
		string time = string.Format("{0}:{1}", Mathf.FloorToInt((manager.timeLimit - Time.timeSinceLevelLoad)/60).ToString("00"), Mathf.FloorToInt((manager.timeLimit - Time.timeSinceLevelLoad)%60).ToString("00"));
		timeTxt.text = "Time: " + time;
		healthTxt.text = player.GetHealth() + " / " + player.GetMaxHealth() + " HP";
		RaycastHit hitInfo;
		if(Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hitInfo, 500F))
		{
			ObjDestroyable obj = hitInfo.transform.gameObject.GetComponent<ObjDestroyable>();
			
			if(obj != null && obj.transform.gameObject != this.gameObject)
			{
				destroyableTxt.text = obj.gameObject.name + ": " + obj.GetHealth() + " / " + obj.GetMaxHealth();
			} else
			{
				destroyableTxt.text = "";
			}
		} else
		{
			destroyableTxt.text = "";
		}
	}
}
