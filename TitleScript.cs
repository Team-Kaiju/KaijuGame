using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TitleScript : MonoBehaviour
{
	bool isLoading = false;
	public Text progressTxt;
	public Image loadingScreen;
	public InputField nameInput;
	
	public void Play()
	{
		if(isLoading)
		{
			return;
		}
		
		isLoading = true;
		StartCoroutine(LoadLevel("PreAlphaDemo"));
	}
	
	// Load level coroutine with black screen and progress 
	IEnumerator LoadLevel(string levelName)
	{
		AsyncOperation gameLoad = Application.LoadLevelAsync(levelName);
		loadingScreen.enabled = true;
        
        while(!gameLoad.isDone)
		{
			int roundedTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
			string spin = roundedTime%4 == 0? "  " : (roundedTime%4 == 1? ".  " : (roundedTime%4 == 2? ".. " : "..."));
			progressTxt.text = "Loading" + spin + " " + Mathf.FloorToInt(gameLoad.progress*100F) + "% ";
			
			yield return null;
        }
        
        progressTxt.text = "";
		isLoading = false;
		loadingScreen.enabled = false;
	}
    
    public void Options()
	{
		if(isLoading)
		{
			return;
		}
		
		Application.LoadLevel("Options_Screen");
	}
	
	public void Credits()
	{
		if(isLoading)
		{
			return;
		}
		
		Application.LoadLevel("Credits_Screen");
	}
	
	public void Quit()
	{
		if(isLoading)
		{
			return;
		}
		
		Application.Quit();
	}
	
	public void BackToTitle()
	{
		if(isLoading)
		{
			return;
		}
		
		Application.LoadLevel("TitleScene");
	}
	
	public void Highscores()
	{
		if(isLoading)
		{
			return;
		}
		
		Application.LoadLevel("HighscoreScene");
	}
	
	public void Update()
	{
		if(nameInput != null && Input.GetKeyDown(KeyCode.Return))
		{
			SubmitScore();
		}
	}
	
	public void SubmitScore()
	{
		if(isLoading)
		{
			return;
		}
		
		string name = "Unknown";
		
		if(nameInput != null && nameInput.text.Length >= 0)
		{
			name = nameInput.text;
		}
		
		for(int i = 0; i < 10; i++)
		{
			if(!PlayerPrefs.HasKey("HS_SCORE_" + i) || PlayerPrefs.GetInt("HS_SCORE_" + i) < PlayerPrefs.GetInt("Score")) // Empty or smaller score found
			{
				for(int j = 9; j > i; j--) // Start shifting all scores down to make space
				{
					if(PlayerPrefs.HasKey("HS_SCORE_" + j)) // Ensure we don't create actual blank entries. Moves only valid ones
					{
						PlayerPrefs.SetInt("HS_SCORE_" + j, PlayerPrefs.GetInt("HS_SCORE_" + (j - 1)));
						PlayerPrefs.SetString("HS_USER_" + j, PlayerPrefs.GetString("HS_USER_" + (j - 1)));
					}
				}
				
				// Place current score in newly opened space
				PlayerPrefs.SetInt("HS_SCORE_" + i, PlayerPrefs.GetInt("Score"));
				PlayerPrefs.SetString("HS_USER_" + i, name);
				
				break; // Break loop, score has been written
			}
		}
		
		Highscores(); // Show highscores
	}
}
