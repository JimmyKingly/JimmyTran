using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothing = 5.0f;

	Vector3 offset;

	void Start()
	{
		// Store the distance between the camera to the player
		offset = transform.position - target.position;
	}

	void FixedUpdate()
	{
		// Have camera position to be the player's position plus the offset
		Vector3 targetCamPos = target.position + offset;

		// Move the camera
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
