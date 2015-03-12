using UnityEngine;
using System.Collections;

public class DamageCollider : MonoBehaviour
{
	public float damage = 10F;
	public ObjDestroyable.DamageType damageType = ObjDestroyable.DamageType.MELEE;
	public bool hurtsPlayers = true;

	void OnCollisionEnter(Collision col)
	{
		ObjDestroyable structure = col.gameObject.GetComponent<ObjDestroyable>();
		
		if(structure != null && (hurtsPlayers || col.gameObject.GetComponent<BasicPlayer>() == null))
		{
			structure.AttackObj(this.gameObject, ObjDestroyable.DamageType.MELEE, damage);
		}
	}
	
	void OnTriggerEnter(Collider col)
	{
		ObjDestroyable structure = col.gameObject.GetComponent<ObjDestroyable>();

		if(structure != null && (hurtsPlayers || col.gameObject.GetComponent<BasicPlayer>() == null))
		{
			structure.AttackObj(this.gameObject, damageType, damage);
		}
	}
}
