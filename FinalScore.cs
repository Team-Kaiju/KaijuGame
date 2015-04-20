using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FinalScore : MonoBehaviour
{
	public Text txt;
	
	// Use this for initialization
	void Start ()
	{
		txt.text = "Score: " + PlayerPrefs.GetInt("Score");
	}
}
