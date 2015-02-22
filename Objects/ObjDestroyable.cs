using UnityEngine;
using System.Collections;

public class ObjDestroyable : MonoBehaviour
{
	public AudioClip deathSound;
	public AudioClip injureSound;

	float maxHealth = 100;
	public float health = 100;

	public void Start()
	{
		maxHealth = health;
	}

	public void Update()
	{
		health = Mathf.Clamp(health, 0F, maxHealth); // Keep things within reasonable range

		if(health <= 0F)
		{
			this.OnDeath();
			GameObject.Destroy(this.gameObject);
		}
	}

	// Used to perform actions upon being marked for removal
	public virtual void OnDeath()
	{
		if(deathSound != null)
		{
			AudioSource.PlayClipAtPoint(deathSound, this.transform.position);
		}
	}

	public void SetHealth(float amount)
	{
		health = Mathf.Clamp(amount, 0F, maxHealth);
	}

	public float GetHealth()
	{
		return health;
	}

	public void AddHealth(float amount)
	{
		this.SetHealth(this.GetHealth() + amount);
	}

	public virtual void AttackObj(GameObject attacker, DamageType type, float damage)
	{
		health -= damage;
		if(injureSound != null)
		{
			AudioSource.PlayClipAtPoint(injureSound, this.transform.position);
		}
	}

	public enum DamageType
	{
		BULLET,
		MELEE,
		EXPLODE
	}
}
