using UnityEngine;
using System.Collections;

public class BootController : MonoBehaviour {

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.CompareTag ("Player")) {
			col.gameObject.GetComponent<PlayerMovement>().StartBoost();
			Destroy(gameObject);
		}
	}
}
