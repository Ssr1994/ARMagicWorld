#define GEARVR
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
	public float smooth = 0.1f;
	public float boostTime = 20f;
	public GameObject boostEffect;
	public Transform boostEffectTransform;
	public Slider speedSlider;
	public GameObject speedUI;

	Animation anim;
    Vector3 movement;
	Vector3 dest;
    Rigidbody playerRigidbody;
	float originalSmooth;
    int floorMask;
	int shootableMask;
    float camRayLength = 100f; // how far from camera
	float boostTimer;
	float waypointPrecision = 0.8f;
	PlayerAttack playerAttack;

    void Awake()
    {
		originalSmooth = smooth;
        floorMask = LayerMask.GetMask("Floor");
		shootableMask = LayerMask.GetMask ("Shootable");
		boostTimer = 0f;
		speedSlider.maxValue = boostTime;
		dest = transform.position;
		playerRigidbody = GetComponent<Rigidbody> ();
		anim = GetComponent<Animation> ();
		playerAttack = GetComponent<PlayerAttack> ();
    }

	void FixedUpdate() // Build-in function called on every framerate update
	{
		#if (UNITY_EDITOR && !GEARVR)
		if (Input.GetMouseButton(0)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit floorHit, otherHit;

			if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
				if (!Physics.Raycast(camRay, out otherHit, camRayLength, shootableMask) || otherHit.collider.gameObject.tag != "Enemy") {
					dest = floorHit.point;
					dest.y = 0f;
				}
			}
		}
		#elif ((UNITY_ANDROID || UNITY_IOS) && !GEARVR)
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended) {
				Ray camRay = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit floorHit, otherHit;

				if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
					if (!Physics.Raycast(camRay, out otherHit, camRayLength, shootableMask) || otherHit.collider.gameObject.tag != "Enemy") {
						dest = floorHit.point;
						dest.y = 0f;
					}
				}
			}
		}
		#elif GEARVR
		Ray camRay = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		RaycastHit floorHit, otherHit;

		if (Physics.Raycast (camRay, out floorHit, camRayLength, floorMask)) {
			if (!Physics.Raycast(camRay, out otherHit, camRayLength, shootableMask) || otherHit.collider.gameObject.tag != "Enemy") {
				dest = floorHit.point;
				dest.y = 0f;
			}
		}
		#endif

		if (boostTimer > 0) {
			boostTimer -= Time.deltaTime;
			speedSlider.value = boostTimer;
		} else {
			smooth = originalSmooth;
			speedUI.SetActive(false);
		}

		//turn and animate
		if (playerAttack.IsCasting () || anim.IsPlaying("Wound"))
			return;
		Vector3 playerToMouse = dest - transform.position;
		playerToMouse.y = 0f;
		if (playerToMouse.sqrMagnitude > waypointPrecision*waypointPrecision) {
			playerRigidbody.MoveRotation (Quaternion.LookRotation (playerToMouse));
			if (!anim.IsPlaying ("Run"))
				anim.Play ("Run");
			//deactive melee attack while running
//			playerAttack.target = null;

			transform.position = Vector3.MoveTowards (transform.position, dest, smooth);

		} else {
			dest = transform.position;
			if (!anim.IsPlaying("idle2") && !anim.IsPlaying("Attack"))
				anim.Play ("idle2");
		}
	}

	public void StartBoost() {
		boostTimer = boostTime;
		smooth *= 1.5f;
		speedUI.SetActive (true);
		GameObject effect = Instantiate (boostEffect, boostEffectTransform.position, boostEffectTransform.rotation) as GameObject;
		effect.transform.parent = boostEffectTransform;
	}
}