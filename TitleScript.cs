using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class TitleScript : MonoBehaviour
{
    public GameObject[] pages;
    [HideInInspector]
    public static int currentPage = -1;
    public float flipSpeed;

	bool isLoading = true;
	public Text progressTxt;
	public GameObject loadingScreen;
	public InputField nameInput;
    public GameObject splashParent;
    public Image splashCover;
    public float splashTime = 10F;

    AudioSource audioSrc;
    public AudioClip titleMusic;

    // Script reference for the order of the pages. Change if pages are add/removed/moved in the array
    public enum PageNum
    {
        TITLE = 0,
        HIGHSCORE = 1,
        OPTIONS = 2,
        NEW_HIGHSCORE = 3,
        CREDITS = 4
    }

    public void Start()
    {
        audioSrc = this.GetComponent<AudioSource>();
        isLoading = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Update()
    {
        if (currentPage == (int)PageNum.NEW_HIGHSCORE && nameInput != null && Input.GetKeyDown(KeyCode.Return))
        {
            SubmitScore();
            return;
        }

        if(currentPage == -1)
        {
            if(Time.timeSinceLevelLoad < splashTime)
            {
                isLoading = true;
                splashParent.SetActive(true);
                splashCover.enabled = true;

                splashCover.color = new Color(0F, 0F, 0F, Mathf.Sin((Time.timeSinceLevelLoad / splashTime * 360 + 90) * Mathf.Deg2Rad) * 0.5F + 0.5F);
            } else
            {
                splashParent.SetActive(false);
                splashCover.enabled = false;
                currentPage = 0;
                isLoading = false;
            }
        } else
        {
            if(audioSrc != null && !audioSrc.isPlaying && !isLoading)
            {
                audioSrc.loop = true;
                audioSrc.clip = titleMusic;
                audioSrc.volume = 0.25F;
                audioSrc.Play();
            }

            splashParent.SetActive(false);
            splashCover.enabled = false;

            for (int i = 0; i < pages.Length; i++ )
            {
                GameObject pgObj = pages[i];

                if(pgObj == null)
                {
                    continue;
                }

                RectTransform pgTrans = pgObj.GetComponent<RectTransform>();

                float screenHeight = Screen.height;
                float screenFactor = screenHeight / 600F;

                if(i != currentPage)
                {
                    if ((pgTrans.anchoredPosition.y > -screenHeight * 2F) && pgObj.activeSelf)
                    {
                        pgTrans.Translate(Vector3.up * -Time.deltaTime * flipSpeed * screenFactor);
                    } else
                    {
                        pgObj.SetActive(false);
                        pgTrans.anchoredPosition = Vector2.up * -screenHeight * 2F;
                    }
                } else
                {
                    pgObj.SetActive(true);
                    if (pgTrans.anchoredPosition.y < 0F)
                    {
                        pgTrans.Translate(Vector3.up * Time.deltaTime * flipSpeed * screenFactor);
                    }
                    else
                    {
                        if (pgTrans.anchoredPosition != Vector2.zero)
                        {
                            isLoading = false;
                            pgTrans.anchoredPosition = Vector2.zero;
                        }
                    }
                }
            }
        }
    }

    public void FlipTo(int page)
    {
        if (currentPage != page && !isLoading)
        {
            isLoading = true;
            currentPage = Mathf.Clamp(page, 0, pages.Length - 1);
        }
    }
	
	public void Play()
	{
		if(isLoading)
		{
			return;
		}

		isLoading = true;
		StartCoroutine(LoadLevel("NewKaiju"));
        audioSrc.Stop();
	}

    public void Quit()
    {
        Application.Quit();
    }
	
	// Load level coroutine with black screen and progress 
	IEnumerator LoadLevel(string levelName)
	{
		AsyncOperation gameLoad = Application.LoadLevelAsync(levelName);
		loadingScreen.SetActive(true);
        
        while(!gameLoad.isDone)
		{
			int roundedTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
			string spin = roundedTime%4 == 0? "  " : (roundedTime%4 == 1? ".  " : (roundedTime%4 == 2? ".. " : "..."));
			progressTxt.text = "Loading" + spin + " " + Mathf.FloorToInt(gameLoad.progress*100F) + "% ";
			
			yield return null;
        }
        
        progressTxt.text = "";
		isLoading = false;
		loadingScreen.SetActive(false);
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
					if(PlayerPrefs.HasKey("HS_SCORE_" + (j - 1))) // Ensure we don't create actual blank entries. Moves only valid ones
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
		
		FlipTo((int)PageNum.HIGHSCORE); // Show highscores
	}
}
