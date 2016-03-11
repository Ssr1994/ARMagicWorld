using UnityEngine;
using System.Collections;

public class PlayerTutorial : MonoBehaviour {

	public GameObject tornado;
	public GameObject holyFire;
	public GameObject lightningStrike;
	public GameObject lightningBlast;

	Animation anim;
	bool shotAnimated = false;
	bool castAnimated = false;
	bool lightningCharged = false;
	GameObject wind;

	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
		anim = GetComponent<Animation> ();
		Time.timeScale = 1f;
	}
	
	// Update is called once per frame
	void Update () {

		if (anim.isPlaying)
			return;

		if (Input.gyro.userAcceleration.z > 0.14f) {
			anim.Play ("Skill01");
			shotAnimated = true;
		}

		if (Input.gyro.userAcceleration.y > 0.12f) {
			anim.Play ("Skill03");
			Instantiate (holyFire, transform.position + transform.forward*5f, transform.rotation);
		}

		if (Input.gyro.userAcceleration.x > 0.12f) {
			anim.Play ("Skill02");
			castAnimated = true;
		}

		if (shotAnimated && !anim.IsPlaying("Skill01")) {
			wind = Instantiate (tornado, transform.position + transform.forward*2f, transform.rotation) as GameObject;
			shotAnimated = false;
		}
		
		if (castAnimated && !anim.IsPlaying ("Skill02")) {
			Instantiate (lightningStrike, transform.position + transform.up*0.15f + transform.forward*5f, Quaternion.identity);
			Instantiate (lightningBlast, transform.position + transform.forward*5f, Quaternion.identity);
			castAnimated = false;
		}

		if (wind != null) {
			Vector3 dir = (new Vector3 (wind.transform.forward.x, 0f, wind.transform.forward.z)).normalized;
			wind.transform.position += dir * 5f * Time.deltaTime;
		}
	}
}
