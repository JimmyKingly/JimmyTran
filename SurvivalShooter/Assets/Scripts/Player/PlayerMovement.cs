using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6.0f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask;
	float camRayLength = 100.0f;

	void Awake()
	{
		// Pass the Floor layer
		floorMask = LayerMask.GetMask ("Floor");
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
	}

	void FixedUpdate()
	{
		// Variables to move the player -1, 0, 1 on the axis
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");

		// Call functions so player can move
		Move (h, v);
		Turning ();
		Animating (h, v);
	}

	void Move (float h, float v)
	{
		// Set player's movement on the x and z axis to h and v respectively
		movement.Set (h, 0.0f, v);

		// Make sure the distance when walking is always consistent with speed
		movement = movement.normalized * speed * Time.deltaTime;

		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning ()
	{
		// Cast a ray from the position of the mouse from the main camera
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit floorHit;


		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask))
		{
			// Get the distance between the player and the point where the mouse ray hit
			Vector3 playerToMouse = floorHit.point - transform.position;

			// Make sure the player is positioned up correctly
			playerToMouse.y = 0.0f;

			// Have the player turn to where the mouse position is
			Quaternion newRotation = Quaternion.LookRotation (playerToMouse);
			playerRigidbody.MoveRotation (newRotation);
		}
	}

	void Animating(float h, float v)
	{
		// If the h and v values are not 0 then the player is walking
		bool walking = h != 0.0f || v != 0.0f;

		// Set boolean to "IsWalking" animator parameter
		anim.SetBool ("IsWalking", walking);
	}
}
