using UnityEngine;
using System.Collections;

public class ObjRubble : MonoBehaviour
{
	public float despawnTime = 60;
	float creationTime = 0;

	// Use this for initialization
	void Start ()
	{
		despawnTime += Random.Range (-3F,3F);
		creationTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Time.timeSinceLevelLoad - creationTime >= despawnTime)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
