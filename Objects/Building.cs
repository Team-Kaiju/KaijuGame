using UnityEngine;
using System.Collections;

public class Building : ObjDestroyable {
	public GameObject rubbleObject;
	ObjDebris[] debrisList;
	public AudioClip[] debrisSounds;
	public int pointWorth;
	float lastHealth = 100;
	GameManager manager;

	public void Start()
	{
		manager = GameObject.FindObjectOfType<GameManager>();
		lastHealth = 100;
		debrisList = this.GetComponentsInChildren<ObjDebris>(); // Get all marked debris pieces in child objects
		for(int i = 0; i < debrisList.Length; i++)
		{
			ObjDebris debris = debrisList[i];
			debris.enabled = false;
			debris.rigidbody.useGravity = false;
			if(debris.collider != null)
			{
				debris.collider.enabled = false;
			}
		}
	}

	public void Update()
	{
		base.Update();
		if(lastHealth != health)
		{
			lastHealth = health;
			int index = Mathf.FloorToInt(health/maxHealth * debrisList.Length); // Get the index of the current piece to break off the building
			if(debrisList.Length > 0 && debrisList[index] != null)
			{
				ObjDebris debris = debrisList[index]; // Pull debris from valid pieces
				debrisList[index] = null; // Remove it from the list
				debris.creationTime = Time.timeSinceLevelLoad; // Set the time the debris was broken off
				debris.enabled = true; // Enable the debris script
				debris.rigidbody.useGravity = true; // Enable gravity
				if(debris.collider != null)
				{
					debris.collider.enabled = true;
				}
				debris.transform.SetParent(null); // Detatch from main building
				debris.rigidbody.AddForce((debris.transform.position - this.transform.position).normalized * 10F, ForceMode.Impulse); // Throw object relative to building origin
				if(debrisSounds != null && debrisSounds.Length > 0)
				{
					AudioSource.PlayClipAtPoint(debrisSounds[Random.Range(0, debrisSounds.Length - 1)], debris.transform.position);
				}
			}
		}
	}

	public override void OnDeath()
	{
		base.OnDeath();

		if(rubbleObject != null)
		{
			GameObject.Instantiate(rubbleObject, this.transform.position, this.transform.rotation);
		}
		manager.totalScore += pointWorth;
		GameObject.Destroy(this.gameObject);
	}
}
