using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
	public int startingHealth = 100;
	public int currentHealth;
	public float sinkSpeed = 2.5f;
	public int scoreValue = 10;
	public AudioClip deathClip;


	Animator anim;
	AudioSource enemyAudio;
	ParticleSystem hitParticles;
	CapsuleCollider capsuleCollider;
	bool isDead;
	bool isSinking;


	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent <Animator> ();
		enemyAudio = GetComponent <AudioSource> ();
		hitParticles = GetComponentInChildren <ParticleSystem> ();
		capsuleCollider = GetComponent <CapsuleCollider> ();

		// Setting the current health when the enemy first spawns.
		currentHealth = startingHealth;
	}


	void Update ()
	{
		// If the enemy should be sinking move the enemy down by the sinkSpeed per second.
		if(isSinking)
		{
			transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
		}
	}


	public void TakeDamage (int amount, Vector3 hitPoint)
	{
		// If the enemy is dead then leave return function
		if (isDead)
			return;

		enemyAudio.Play ();

		// Reduce the current health by the amount of damage sustained.
		currentHealth -= amount;

		// Set the position of the particle system to where the hit was sustained.
		hitParticles.transform.position = hitPoint;

		hitParticles.Play();

		// If the current health is less than or equal to zero then the enemy is dead
		if(currentHealth <= 0)
		{
			Death ();
		}
	}


	void Death ()
	{
		// The enemy is dead.
		isDead = true;

		// Turn the collider into a trigger so shots can pass through it.
		capsuleCollider.isTrigger = true;

		// Tell the animator that the enemy is dead.
		anim.SetTrigger ("Dead");

		enemyAudio.clip = deathClip;
		enemyAudio.Play ();
	}

	// Function called in animation event
	public void StartSinking ()
	{
		// Find and disable the Nav Mesh Agent.
		GetComponent <UnityEngine.AI.NavMeshAgent> ().enabled = false;

		// Find the rigidbody component and make it kinematic (since we use Translate to sink the enemy).
		GetComponent <Rigidbody> ().isKinematic = true;

		isSinking = true;

		// Increase the score by the enemy's score value.
		ScoreManager.score += scoreValue;

		// After 2 seconds destory the enemy.
		Destroy (gameObject, 2f);
	}
}