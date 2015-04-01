using UnityEngine;
using System.Collections;

public class Building : ObjDestroyable {
	public GameObject rubbleObject;
	ObjDebris[] debrisList;
	public AudioClip[] debrisSounds;
	public int pointWorth;
	float lastHealth = 100;
	GameManager manager;
	[Range(0F,1F)]
	public float debrisPercent = 0.5F; // Percentage of the attached pieces that will fall off before full destruction
	public float breakForce = 10F;

	public void Start()
	{
		base.Start ();
		manager = GameObject.FindObjectOfType<GameManager>();
		lastHealth = 100;
		debrisList = this.GetComponentsInChildren<ObjDebris>(); // Get all marked debris pieces in child objects
		
		if(manager != null)
		{
			manager.RegisterCulling(this.gameObject);
		}
		
		for(int i = 0; i < debrisList.Length; i++)
		{
			ObjDebris debris = debrisList[i];
			debris.enabled = false;
			debris.GetComponent<Rigidbody>().useGravity = false;

			foreach(ParticleSystem ps in debris.GetComponentsInChildren<ParticleSystem>())
			{
				ps.Stop(true);
			}

			if(debris.GetComponent<Collider>() != null)
			{
				debris.GetComponent<Collider>().enabled = false;
			}
		}

		if(true && debrisList.Length > 1) // Sort pieces from top down
		{
			ArrayList sorted = new ArrayList();

			sorted.AddRange(debrisList);

			for(int i = 1; i < sorted.Count; i++)
			{
				ObjDebris obj = (ObjDebris)sorted[i];

				int j = i;
				ObjDebris prevObj = obj;

				while(j > 0 && prevObj.transform.position.y >= obj.transform.position.y)
				{
					j--;
					prevObj = (ObjDebris)sorted[j];
				}

				if(j >= 0 && j != i)
				{
					sorted.RemoveAt(i);
					sorted.Insert(j, obj);
				}
			}

			for(int i = 0; i < sorted.Count; i++)
			{
				debrisList[i] = (ObjDebris)sorted[i];
			}
		}
	}

	public void Update()
	{
		base.Update();
		if(lastHealth != health)
		{
			lastHealth = health;
			int i = Mathf.FloorToInt(debrisList.Length * (1.0F - debrisPercent) + (health/maxHealth * debrisList.Length * debrisPercent)); // Get the index of the current piece to break off the building

			for(int index = i; index < debrisList.Length; index++)
			{
				if(debrisList[index] != null)
				{
					ObjDebris debris = debrisList[index]; // Pull debris from valid pieces
					debrisList[index] = null; // Remove it from the list
					debris.creationTime = Time.timeSinceLevelLoad + Random.Range(-3F,3F); // Set the time the debris was broken off
					debris.enabled = true; // Enable the debris script
					debris.GetComponent<Rigidbody>().useGravity = true; // Enable gravity
					
					foreach(ParticleSystem ps in debris.GetComponentsInChildren<ParticleSystem>())
					{
						ps.Play(true);
					}

					if(debris.GetComponent<Collider>() != null)
					{
						debris.GetComponent<Collider>().enabled = true;
					}
					debris.transform.SetParent(null); // Detatch from main building
					debris.GetComponent<Rigidbody>().AddForce((debris.transform.position - this.transform.position).normalized * breakForce, ForceMode.Impulse); // Throw object relative to building origin
					if(debrisSounds != null && debrisSounds.Length > 0)
					{
						AudioSource.PlayClipAtPoint(debrisSounds[Random.Range(0, debrisSounds.Length - 1)], debris.transform.position);
					}
				}
			}
		}
	}

	public override void OnDeath()
	{
		base.OnDeath();

		for(int i = 0; i < debrisList.Length; i++)
		{
			ObjDebris debris = debrisList[i]; // Pull debris from valid pieces

			if(debris == null)
			{
				continue;
			}

			debrisList[i] = null; // Remove it from the list
			debris.creationTime = Time.timeSinceLevelLoad + Random.Range(-3F,3F); // Set the time the debris was broken off
			debris.enabled = true; // Enable the debris script
			debris.GetComponent<Rigidbody>().useGravity = true; // Enable gravity
			
			foreach(ParticleSystem ps in debris.GetComponentsInChildren<ParticleSystem>())
			{
				ps.Play(true);
			}

			if(debris.GetComponent<Collider>() != null)
			{
				debris.GetComponent<Collider>().enabled = true;
			}
			debris.transform.SetParent(null); // Detatch from main building
			debris.GetComponent<Rigidbody>().AddForce((debris.transform.position - this.transform.position).normalized * breakForce, ForceMode.Impulse); // Throw object relative to building origin
		}

		if(rubbleObject != null)
		{
			GameObject tmp = GameObject.Instantiate(rubbleObject, this.transform.position, this.transform.rotation) as GameObject;
		}
		
		if(manager != null)
		{
			manager.totalScore += pointWorth;
		}
		GameObject.Destroy(this.gameObject);
	}
}
