using UnityEngine;
using System.Collections;

public class BuildingSpawner : MonoBehaviour 
{
	//Declares Buildings to be spawned
	public GameObject[] BuildingTip;
	public GameObject[] BuildingTipLeft;
	public GameObject[] BuildingTipRight;
	public GameObject[] BuildingMidRight;
	public GameObject[] BuildingMidLeft;
	public GameObject[] BuildingMidTip;
	public GameObject[] BuildingMidBack;
	public GameObject[] BuildingBackLeft;
	public GameObject[] BuildingBackRight;

	//Declares Spawnpoint
	public GameObject SpawnPointTip;
	public GameObject SpawnPointTipLeft;
	public GameObject SpawnPointTipRight;
	public GameObject SpawnPointMidRight;
	public GameObject SpawnPointMidLeft;
	public GameObject SpawnPointMidTip;
	public GameObject SpawnPointMidBack;
	public GameObject SpawnPointBackLeft;
	public GameObject SpawnPointBackRight;

	// Use this for initialization
	void Start () 
	{
		SpawnBuildings ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void SpawnBuildings ()
	{
		//Determines spawnpoint location
		Vector3 spawnLocationTip = SpawnPointTip.transform.position;
		Vector3 spawnLocationTipLeft = SpawnPointTipLeft.transform.position;
		Vector3 spawnLocationTipRight = SpawnPointTipRight.transform.position;
		Vector3 spawnLocationMidRight = SpawnPointMidRight.transform.position;
		Vector3 spawnLocationMidLeft = SpawnPointMidLeft.transform.position;
		Vector3 spawnLocationMidTip = SpawnPointMidTip.transform.position;
		Vector3 spawnLocationMidBack = SpawnPointMidBack.transform.position;
		Vector3 spawnLocationBackLeft = SpawnPointBackLeft.transform.position;
		Vector3 spawnLocationBackRight = SpawnPointBackRight.transform.position;

		//Chooses an random prefab from array
		int randomBuildingTip = Random.Range (0, BuildingTip.Length);
		int randomBuildingTipLeft = Random.Range (0, BuildingTipLeft.Length);
		int randomBuildingTipRight = Random.Range (0, BuildingTipRight.Length);
		int randomBuildingMidRight = Random.Range (0, BuildingMidRight.Length);
		int randomBuildingMidLeft = Random.Range (0, BuildingMidLeft.Length);
		int randomBuildingMidTip = Random.Range (0, BuildingMidTip.Length);
		int randomBuildingMidBack = Random.Range (0, BuildingMidBack.Length);
		int randomBuildingBackLeft = Random.Range (0, BuildingBackLeft.Length);
		int randomBuildingBackRight = Random.Range (0, BuildingBackRight.Length);

		//Spawns Buildings useing spawn location
		Instantiate (BuildingTip [randomBuildingTip], spawnLocationTip, SpawnPointTip.transform.rotation);
		Instantiate (BuildingTipLeft [randomBuildingTipLeft], spawnLocationTipLeft, SpawnPointTipLeft.transform.rotation);
		Instantiate (BuildingTipRight [randomBuildingTipRight], spawnLocationTipRight, SpawnPointTipRight.transform.rotation);
		Instantiate (BuildingMidRight [randomBuildingMidRight], spawnLocationMidRight, SpawnPointMidRight.transform.rotation);
		Instantiate (BuildingMidLeft [randomBuildingMidLeft], spawnLocationMidLeft, SpawnPointMidLeft.transform.rotation);
		Instantiate (BuildingMidTip [randomBuildingMidTip], spawnLocationMidTip, SpawnPointMidTip.transform.rotation);
		Instantiate (BuildingMidBack [randomBuildingMidBack], spawnLocationMidBack, SpawnPointMidBack.transform.rotation);
		Instantiate (BuildingBackLeft [randomBuildingBackLeft], spawnLocationBackLeft, SpawnPointBackLeft.transform.rotation);
		Instantiate (BuildingBackRight [randomBuildingBackRight], spawnLocationBackRight, SpawnPointBackRight.transform.rotation);
	}
}
