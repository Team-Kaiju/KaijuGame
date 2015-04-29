using UnityEngine;
using System.Collections;

public class ObjDestroyable : MonoBehaviour
{
    [HideInInspector]
	public AudioClip[] deathSound;
	public AudioClip[] injureSound;

	[HideInInspector]
	protected float maxHealth = 100;
	public float health = 100;

	public virtual void Start()
	{
		maxHealth = health;
	}

	public virtual void Update()
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
		if(deathSound != null && deathSound.Length > 0)
		{
            AudioHelper.PlayClip(deathSound[Random.Range(0, deathSound.Length - 1)], this.transform.position, Random.Range(0.9F, 1.1F), 1.0F, false);
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
	
	public float GetMaxHealth()
	{
		return maxHealth;
	}

	public void AddHealth(float amount)
	{
		this.SetHealth(this.GetHealth() + amount);
	}

	public virtual void AttackObj(GameObject attacker, DamageType type, float damage)
	{
		health -= damage;
        if (injureSound != null && injureSound.Length > 0)
        {
            AudioHelper.PlayClip(injureSound[Random.Range(0, injureSound.Length - 1)], this.transform.position, Random.Range(0.9F, 1.1F), 1.0F, false);
		}
	}

	public enum DamageType
	{
		BULLET,
		MELEE,
		EXPLODE,
		CRUSH
	}
}
