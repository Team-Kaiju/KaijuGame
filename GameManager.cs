using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour {
	[HideInInspector]
	public int totalScore = 0;
	public int[] targetScores = new int[]{2500, 5000, 10000};
	public float timeLimit = 60 * 5; // Time limit in seconds. Default 5 minutes
    [HideInInspector]
    public float startTime = 0F;
	BasicPlayer player;
	ArrayList cullingList = new ArrayList();
	public Camera cullingCam;

    AudioSource audioSrc;
    public AudioClip ambianceSound;
	
	IEnumerator routine;

    [HideInInspector]
    public bool isLoading = true;
    [HideInInspector]
    public bool isPlaying = false;
    public int maxGeneratorThreads = 5;

    ArrayList generators = new ArrayList();
	
	// Use this for initialization
	void Start ()
	{
		totalScore = 0;
        player = GameObject.FindObjectOfType<BasicPlayer>();
        PlayerPrefs.SetInt("Score", 0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

		if(Input.GetKeyDown(KeyCode.Escape) && !isLoading)
		{
            if (isPlaying)
            {
                TitleScript.currentPage = (int)TitleScript.PageNum.TITLE;
                Application.LoadLevel("TitleScene");
                return;
            } else
            {
                isPlaying = true;
                audioSrc = this.GetComponent<AudioSource>();
                audioSrc.clip = this.ambianceSound;
                audioSrc.Play();
            }
		}
		
        if(!isPlaying)
        {
            startTime = Time.timeSinceLevelLoad;
        } else if(Time.timeSinceLevelLoad >= timeLimit + startTime || player == null)
		{
			GameOver();
		}
		
		if(player != null && routine == null && isPlaying)
		{
			cullingList.Remove(null);
			routine = CullObjects(cullingList.Clone() as ArrayList);
			StartCoroutine(routine);
		}

        if(generators.Count > 0 || Time.timeSinceLevelLoad < 1F) // Must have waited a minimum of 1 second before allowing the loading screen to turn off
        {
            StepGenerators();
            isLoading = true;
        } else
        {
            isLoading = false;
        }
	}

    public void StepGenerators()
    {
        for (int i = Mathf.Min(generators.Count - 1, maxGeneratorThreads - 1); i >= 0; i--)
        {
            IEnumerator genRoutine = generators[i] as IEnumerator;
            if (!genRoutine.MoveNext())
            {
                generators.Remove(genRoutine);
            }
        }
    }

    public void RegisterGenerator(IEnumerator genRoutine)
    {
        generators.Add(genRoutine);
    }
	
	IEnumerator CullObjects(ArrayList list)
	{
        Plane[] camPlanes = GeometryUtility.CalculateFrustumPlanes(cullingCam);

		foreach(GameObject gameObj in list)
		{
			if(gameObj == null)
			{
				continue;
			}

            if(Vector3.Distance(cullingCam.transform.position, gameObj.transform.position) > cullingCam.farClipPlane)
            {
                gameObj.SetActive(false);
                continue;
            }
			
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
        if(obj == null)
        {
            return;
        }

		cullingList.Add(obj);
	}
    
    public void GameOver()
	{
        PlayerPrefs.SetInt("Score", totalScore);

        TitleScript.currentPage = (int)TitleScript.PageNum.HIGHSCORE;
		
		for(int i = 0; i < 10; i++)
		{
			if(!PlayerPrefs.HasKey("HS_SCORE_" + i) || PlayerPrefs.GetInt("HS_SCORE_" + i) < PlayerPrefs.GetInt("Score")) // Empty or smaller score found
			{
                TitleScript.currentPage = (int)TitleScript.PageNum.NEW_HIGHSCORE;
                break;
			}
		}

        Application.LoadLevel("TitleScene");
	}
}
