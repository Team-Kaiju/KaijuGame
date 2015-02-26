using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BasicUI : MonoBehaviour {
	public GUISkin uiSkin; // The base skin for the GUI
	public Text scoreTxt;
	public Text timeTxt;
	GameManager manager;

	// Use this for initialization
	void Start () {
		manager = GameObject.FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		scoreTxt.text = "Score: " + manager.totalScore;
		string time = string.Format("{0}:{1}", Mathf.FloorToInt((manager.timeLimit - Time.timeSinceLevelLoad)/60).ToString("00"), Mathf.FloorToInt((manager.timeLimit - Time.timeSinceLevelLoad)%60).ToString("00"));
		timeTxt.text = "Time: " + time;
	}
}
