using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ListScores : MonoBehaviour
{
	public Text txt;
	// Use this for initialization
	void Start () {
		string scoreList = "";
		
		for(int i = 0; i < 10; i++)
		{
			int score = 0;
			string username = "NONE";
			if(PlayerPrefs.HasKey("HS_USER_" + i))
			{
				score = PlayerPrefs.GetInt("HS_SCORE_" + i);
				username = PlayerPrefs.GetString("HS_USER_" + i);
			}
			
			scoreList += "#" + (i + 1) + " " + username + " - " + score + "\n";
		}
		
		txt.text = scoreList;
	}
}
