using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject enemy;
    public float spawnTime = 3f;
    public Transform[] spawnPoints;


    void Start ()
    {
		// Repeat spawn function every 'spawntime'
        InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }


    void Spawn ()
    {
		// If the player's health is less than or equal to 0 then return from function
        if(playerHealth.currentHealth <= 0f)
        {
            return;
        }

		// If the player is still alive then generate spawnpoint and instantiate enemy
        int spawnPointIndex = Random.Range (0, spawnPoints.Length);

        Instantiate (enemy, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
    }
}
