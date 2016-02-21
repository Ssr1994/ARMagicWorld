using UnityEngine;
using System.Collections;

public class PickupMovement : MonoBehaviour {

	public float spinSpeed = 10f;
	public float floatSpeed = 5f;
	public float verticalMovement = 0.2f;

	float time;
	float originalY;
	// Use this for initialization
	void Start () {
		originalY = transform.position.y;
	}

	// Update is called once per frame
	void FixedUpdate () {
		time += Time.deltaTime;
		transform.rotation = Quaternion.Euler (0, 10*spinSpeed*Time.deltaTime, 0) * transform.rotation;
		transform.position = new Vector3(transform.position.x, (originalY + (float)verticalMovement*Mathf.Sin(floatSpeed*time)), transform.position.z);
	}
}
