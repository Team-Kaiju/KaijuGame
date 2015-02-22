using UnityEngine;
using System.Collections;

public class Building : ObjDestroyable {
	public GameObject debris;
	public float pointWorth;

	public override void OnDeath()
	{
		base.OnDeath();
		int num = Random.Range(5, 20);
		
		for(int i = 0; i < num; i++)
		{
			GameObject.Instantiate(debris, this.transform.position + new Vector3(Random.Range(-10F, 10F), Random.Range(-10F, 10F), Random.Range(-10F, 10F)), this.transform.rotation);
		}
		
		GameObject.Destroy(this.gameObject);
	}
}
