using UnityEngine;
using System.Collections;

public class ArmySpawnNode : MonoBehaviour
{
	public GameObject[] armyVehicles;
	public float spawnInterval = 30F;
	public int spawn = -1; // A value of less than 0 will result in infinite spawns
	float spawnTime = 0F;

    GameManager manager;
	
	// Use this for initialization
	void Start()
	{
        manager = GameObject.FindObjectOfType<GameManager>();
	}
	
	// Update is called once per frame
	void Update()
    {
        if (!manager.isPlaying)
        {
            return;
        }

		if(spawnTime <= 0F && spawn != 0 && armyVehicles.Length > 0)
		{
			spawnTime = spawnInterval;
			spawn--;
			GameObject vehicle = armyVehicles[Random.Range(0, armyVehicles.Length - 1)];
			NavMeshHit navHit;
			
			if(NavMesh.SamplePosition(transform.position, out navHit, 100, 1)) // Nessecary for NavMesh bug/quirk
			{
				GameObject.Instantiate(vehicle, navHit.position, vehicle.transform.rotation);
			}
		} else
		{
			spawnTime -= Time.deltaTime;
		}
	}
}
