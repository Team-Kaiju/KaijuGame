using UnityEngine;
using System.Collections;

public class Lightning : MonoBehaviour
{
	public float minInterval = 3F;
	public float maxInterval = 10F;
	public Light light;
	public AudioClip[] lightningSounds;
	float timer;
	float origInt = 1F;
	
	// Use this for initialization
	void Start()
	{
		timer = Random.Range(minInterval, maxInterval);
		
		if(light != null)
		{
			origInt = light.intensity;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		timer -= Time.deltaTime;
		
		if(timer <= 0F)
		{
			timer = Random.Range(minInterval, maxInterval);
			
			if(light != null)
			{
				light.intensity = 1F;
				if(lightningSounds != null && lightningSounds.Length > 0)
				{
					AudioSource.PlayClipAtPoint(lightningSounds[Random.Range(0, lightningSounds.Length - 1)], this.transform.position);
				}
			}
		} else
		{
			if(light != null)
			{
				light.intensity = origInt;
			}
		}
	}
}
