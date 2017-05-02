using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;									// How far the player's bullets can go


    float timer;
    Ray shootRay = new Ray();
    RaycastHit shootHit;
	int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
    Light gunLight;
    float effectsDisplayTime = 0.2f;


    void Awake ()
    {
		// Set shootablemask to shootable layer
        shootableMask = LayerMask.GetMask ("Shootable");

		// Reference components
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
        gunLight = GetComponent<Light> ();
    }


    void Update ()
    {
        timer += Time.deltaTime;

		// If the user left clicks the mouse and the time between each shot is greater than the set time then shoot
		if(Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

		// Disable effects after enough time has progressed after firing
        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }

	// Function to disable gunline and gunlight component
    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }

    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);	// Set points of the line created when shooting

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

		// Fire a ray when the player shoots to return the point of whatever it hits
        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
			// If the ray hits something shootable then return the enemy health script
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

			// If we hit an enemy then the enemy takes damage
            if(enemyHealth != null)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }

			// End the line because we hit something
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
			// If we didn't hit anything then just draw a line
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }
}
