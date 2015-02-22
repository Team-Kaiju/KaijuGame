using UnityEngine;
using System.Collections;

public class DamageCollider : MonoBehaviour
{
	public float damage = 10F;

	void OnCollisionEnter(Collision col)
	{
		ObjDestroyable structure = col.gameObject.GetComponent<ObjDestroyable>();
		
		if(structure != null)
		{
			structure.AttackObj(this.gameObject, ObjDestroyable.DamageType.MELEE, damage);
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		ObjDestroyable structure = col.gameObject.GetComponent<ObjDestroyable>();

		if(structure != null)
		{
			structure.AttackObj(this.gameObject, ObjDestroyable.DamageType.MELEE, damage);
		}
	}
}
