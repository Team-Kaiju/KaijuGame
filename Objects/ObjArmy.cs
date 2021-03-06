﻿using UnityEngine;
using System.Collections;

public class ObjArmy : ObjDestroyable
{
	NavMeshAgent navigation;
	BasicPlayer player;
	public float damage = 1F;
	public float fireRate = 1F;
	float reloadTime = 0F;
	public float attackDistance = 50F;
	public float gunHeight = 1F;
	public AudioClip attackSound;
	// Use this for initialization
	public override void Start()
	{
		base.Start();
		navigation = this.GetComponent<NavMeshAgent> ();
		//navigation.enabled = true;
		player = FindObjectOfType<BasicPlayer> ();

		if(navigation != null)
		{
			navigation.stoppingDistance = attackDistance *0.75F;
		}
	}
	
	// Update is called once per frame
	public override void Update()
	{
		base.Update();
		
		navigation.enabled = true;
		
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
			
			Vector3 targetPos = player.transform.position + (Vector3.up * 10F);
			Vector3 firePos = this.transform.position + (Vector3.up * gunHeight);

			if(Vector3.Distance(targetPos, firePos) <= attackDistance && reloadTime <= 0F)
			{
				RaycastHit[] hitInfo = Physics.RaycastAll(firePos, (targetPos - firePos).normalized, Vector3.Distance(targetPos, firePos));
				bool flag = true;
				foreach(RaycastHit hit in hitInfo)
				{
					if(hit.transform.gameObject != player.gameObject && hit.transform.gameObject != this.gameObject && hit.transform.gameObject.GetComponent<Collider>() != null)
					{
						flag = false;
						break;
					}
				}
				
				reloadTime = 1F/fireRate;

				if(flag)
				{
					player.AttackObj(this.gameObject, ObjDestroyable.DamageType.EXPLODE, this.damage);
					
					if(attackSound != null)
					{
						AudioSource.PlayClipAtPoint(attackSound, firePos);
					}
				}
			}
		} else if(navigation != null)
		{
			player = FindObjectOfType<BasicPlayer> ();
			navigation.ResetPath();
		} else
		{
			navigation = this.GetComponent<NavMeshAgent> ();
			
			if(navigation != null)
			{
				navigation.stoppingDistance = attackDistance *0.75F;
			}
		}
	}
}
