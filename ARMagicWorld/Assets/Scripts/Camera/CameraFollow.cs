using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	public Transform target; // The player
	public float smoothing = 5f;

	Vector3 offset;

	void Start()
	{
		offset = transform.position - target.position;
	}

	void FixedUpdate()
	{
		Vector3 targetCamPos = target.position + offset;
		// Smooth transitioning
		transform.position = Vector3.Lerp (transform.position, targetCamPos, smoothing * Time.deltaTime);
	}
}
