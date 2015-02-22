﻿using UnityEngine;
using System.Collections;

public class ObjDebris : MonoBehaviour
{
	public float despawnTime = 10;
	float creationTime = 0F;

	// Use this for initialization
	void Start ()
	{
		creationTime = Time.timeSinceLevelLoad;
	}
	
	// Update is called once per frame
	void Update()
	{
		if(creationTime + despawnTime < Time.timeSinceLevelLoad)
		{
			GameObject.Destroy(this.gameObject);
		}
	}
}
