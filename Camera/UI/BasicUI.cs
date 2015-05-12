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

    public Image redTint;
    float lastHealth = 100;
    float lastHitTime = -999;


    public Image loadingScreen;
    public Text loadingText;
    public float tranistionSpeed = 250F;

	string[] scoreLvls = new string[]{"Bronze", "Silver", "Gold"};
	int scoreIdx = 0;

	// Use this for initialization
	void Start()
	{
		manager = GameObject.FindObjectOfType<GameManager>();
		player = GameObject.FindObjectOfType<BasicPlayer>();
		cam = GameObject.FindObjectOfType<Camera>();

        lastHealth = player.GetHealth();
	}
	
	// Update is called once per frame
	void Update()
	{
		if(scoreIdx < manager.targetScores.Length && manager.totalScore >= manager.targetScores[scoreIdx])
		{
			scoreIdx++;
		}

        if(manager.isLoading)
        {
            loadingScreen.enabled = true;
            int roundedTime = Mathf.FloorToInt(Time.timeSinceLevelLoad);
            string spin = roundedTime % 4 == 0 ? "  " : (roundedTime % 4 == 1 ? ".  " : (roundedTime % 4 == 2 ? ".. " : "..."));

            loadingText.text = "Generating" + spin;


        } else if(manager.isPlaying)
        {
            if (loadingScreen.color.a >= 0.01F)
            {
                loadingScreen.color = new Color(0F, 0F, 0F, loadingScreen.color.a - tranistionSpeed);
            } else
            {
                loadingScreen.enabled = false;
            }

            if(loadingText.enabled)
            {
                loadingText.enabled = false;
            }
        } else
        {
            loadingText.text = "[Press Esc]";
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
		
		if(player.GetHealth() != lastHealth)
        {
            if(player.GetHealth() < lastHealth)
            {
                lastHitTime = Time.timeSinceLevelLoad;
            }

            lastHealth = player.GetHealth();
        }

        if(Time.timeSinceLevelLoad - lastHitTime < 1F)
        {
            redTint.enabled = true;
            redTint.color = new Color(1F, 0F, 0F, 0.25F - (Time.timeSinceLevelLoad - lastHitTime) * 0.25F);
        } else
        {
            redTint.enabled = false;
        }
		
		string time = string.Format("{0}:{1}", Mathf.FloorToInt((manager.timeLimit - (Time.timeSinceLevelLoad - manager.startTime))/60).ToString("00"), Mathf.FloorToInt((manager.timeLimit - (Time.timeSinceLevelLoad - manager.startTime))%60).ToString("00"));
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
