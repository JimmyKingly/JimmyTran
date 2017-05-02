using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
	public float timeBetweenAttacks = 0.5f;
	public int attackDamage = 10;


	Animator anim; 
	GameObject player;
	PlayerHealth playerHealth;
	EnemyHealth enemyHealth;
	bool playerInRange;
	float timer;


	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
		enemyHealth = GetComponent<EnemyHealth>();
		anim = GetComponent <Animator> ();
	}


	void OnTriggerEnter (Collider other)
	{
		// If the entering collider is the player then they are in range
		if(other.gameObject == player)
		{
			playerInRange = true;
		}
	}


	void OnTriggerExit (Collider other)
	{
		// If the exiting collider is the player is not in range anymore
		if(other.gameObject == player)
		{
			playerInRange = false;
		}
	}


	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;

		// If the timer exceeds the time between attacks, the player is in range and this enemy is alive then attack
		if((timer >= timeBetweenAttacks && playerInRange) && (enemyHealth.currentHealth > 0))
		{
			Attack ();
		}

		// If the player has zero or less health then set playerdead animator trigger
		if(playerHealth.currentHealth <= 0)
		{
			anim.SetTrigger ("PlayerDead");
		}
	}


	void Attack ()
	{
		timer = 0f;

		// If the player has health to lose then the enemy can attack and do damage
		if(playerHealth.currentHealth > 0)
		{
			playerHealth.TakeDamage (attackDamage);
		}
	}
}