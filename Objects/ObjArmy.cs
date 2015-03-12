using UnityEngine;
using System.Collections;

public class ObjArmy : ObjDestroyable
{
	NavMeshAgent navigation;
	BasicPlayer player;
	public float damage = 1F;
	public float fireRate = 1F;
	float reloadTime = 0F;
	public float attackDistance = 50F;
	public AudioClip attackSound;
	// Use this for initialization
	void Start()
	{
		base.Start();
		navigation = this.GetComponent<NavMeshAgent> ();
		player = FindObjectOfType<BasicPlayer> ();

		if(navigation != null)
		{
			navigation.stoppingDistance = attackDistance *0.99F;
		}
	}
	
	// Update is called once per frame
	void Update()
	{
		base.Update();
		if(reloadTime > 0F)
		{
			reloadTime = Mathf.Clamp(reloadTime - Time.deltaTime, 0F, float.MaxValue);
		}

		if (player != null)
		{
			if(navigation != null)
			{
				navigation.SetDestination(player.transform.position);
			}
			
			this.transform.LookAt(new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z));

			if(Vector3.Distance(player.transform.position, this.transform.position) <= attackDistance && reloadTime <= 0F)
			{
				RaycastHit[] hitInfo = Physics.RaycastAll(transform.position, (player.transform.position - transform.position).normalized, Vector3.Distance(player.transform.position, transform.position));
				bool flag = true;
				foreach(RaycastHit hit in hitInfo)
				{
					if(hit.transform.gameObject != player && hit.transform.gameObject != this.gameObject)
					{
						flag = false;
						break;
					}
				}

				if(flag)
				{
					reloadTime = 1F/fireRate;
					player.AttackObj(this.gameObject, ObjDestroyable.DamageType.EXPLODE, this.damage);
					
					if(attackSound != null)
					{
						AudioSource.PlayClipAtPoint(attackSound, this.transform.position);
					}
				} else
				{
					reloadTime = 1F/fireRate;
				}
			}
		} else if(navigation != null)
		{
			navigation.ResetPath();
		}
	}
}
