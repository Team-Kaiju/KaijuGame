using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class BuildingSpawnNode : MonoBehaviour
{
	public float gridSpacing = 50F; // Spacing between buildings
	public int gridSizeX = 5; // No. buildings on the X axis
	public int gridSizeZ = 5; // No. buildings on the Z axis
	public bool snapToGround = false; // Snap buildings to ground level. Useful for hillsides
	public float maxSnap = 50F; // Max distance this node will search downward to find ground level
	public GameObject[] buildings; // List of valid buildings this node can spawn
	GameObject bakingBox;
	float bakeHeight = 100F;
	
	void Start()
	{
		if(buildings == null || buildings.Length <= 0 || !Application.isPlaying)
		{
			if(bakingBox == null && !Application.isPlaying && !Application.isLoadingLevel)
			{
				for(int i = transform.childCount - 1; i >= 0; i--)
				{
					Transform trans = transform.GetChild(i);
					if(trans.gameObject.tag.Equals("BakingBox"))
					{
						bakingBox = trans.gameObject;
						break;
					}
				}
				
				if(bakingBox == null)
				{
					bakingBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
					bakingBox.tag = "BakingBox";
				}
            }
            return;
		} else
		{
			for(int i = transform.childCount - 1; i >= 0; i--)
			{
				Transform trans = transform.GetChild(i);
				if(trans.gameObject.tag.Equals("BakingBox"))
				{
					GameObject.DestroyImmediate(trans.gameObject);
                }
            }
        }
        
        for(int i = 0; i < gridSizeX; i++)
		{
			for(int k = 0; k < gridSizeZ; k++)
			{
				Vector3 offset = new Vector3((i * gridSpacing) + (gridSpacing/2F), 0F, (k * gridSpacing) + (gridSpacing/2F));
				Vector3 spawnPos = (transform.rotation * offset) + transform.position;
				
				if(snapToGround)
				{
					RaycastHit hitInfo;
					if(Physics.Raycast(spawnPos, Vector3.down, out hitInfo, maxSnap))
					{
						spawnPos.y = hitInfo.point.y;
					}
				}
				
				GameObject build = buildings.Length > 1? buildings[Random.Range(0, buildings.Length)] : buildings[0];
				GameObject tmp = GameObject.Instantiate(build, spawnPos, build.transform.rotation) as GameObject;
				tmp.name = build.name;
			}
		}
	}
	
	void Update()
	{
		if(Application.isLoadingLevel || Application.isPlaying || bakingBox == null)
		{
            return;
        }
		
		Vector3 offset = transform.position + (transform.rotation * new Vector3(gridSizeX/2F * gridSpacing, snapToGround? (bakeHeight-maxSnap)/2F : bakeHeight/2F, gridSizeZ/2F * gridSpacing));
		bakingBox.transform.position = offset;
		bakingBox.transform.localScale = new Vector3(gridSizeX * gridSpacing, (snapToGround? maxSnap : 0F) + bakeHeight, gridSizeZ * gridSpacing);
		bakingBox.transform.SetParent(this.gameObject.transform);
	}
	
	// Helper gizmo for planning the building spawning
	void OnDrawGizmos()
	{
		Gizmos.color = Color.green;
		
		for(int i = 0; i <= gridSizeX; i++)
		{
			Vector3 offA = transform.rotation * new Vector3(i*gridSpacing, 0F, 0F);
			Vector3 offB = transform.rotation * new Vector3(i*gridSpacing, 0F, gridSizeZ*gridSpacing);
			Gizmos.DrawLine(transform.position + offA, transform.position + offB);
			
			offA = transform.rotation * new Vector3(i*gridSpacing, bakeHeight, 0F);
			offB = transform.rotation * new Vector3(i*gridSpacing, bakeHeight, gridSizeZ*gridSpacing);
			Gizmos.DrawLine(transform.position + offA, transform.position + offB);
			//Gizmos.DrawLine(new Vector3(transform.position.x + i*gridSpacing, transform.position.y, transform.position.z), new Vector3(transform.position.x + i*gridSpacing, transform.position.y, transform.position.z + gridSizeZ*gridSpacing));
		}
		
		for(int k = 0; k <= gridSizeZ; k++)
		{
			Vector3 offA = transform.rotation * new Vector3(0F, 0F, k*gridSpacing);
			Vector3 offB = transform.rotation * new Vector3(gridSizeX*gridSpacing, 0F, k*gridSpacing);
			Gizmos.DrawLine(transform.position + offA, transform.position + offB);
			
			offA = transform.rotation * new Vector3(0F, bakeHeight, k*gridSpacing);
			offB = transform.rotation * new Vector3(gridSizeX*gridSpacing, bakeHeight, k*gridSpacing);
			Gizmos.DrawLine(transform.position + offA, transform.position + offB);
			//Gizmos.DrawLine(new Vector3(transform.position.x, transform.position.y, transform.position.z + k*gridSpacing), new Vector3(transform.position.x + gridSizeX*gridSpacing, transform.position.y, transform.position.z + k*gridSpacing));
		}
	}
}
