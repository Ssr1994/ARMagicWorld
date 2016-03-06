﻿#define GEARVR
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	public GameObject Fireball;
	public GameObject HolyFire;
	public GameObject Shield;
	public Transform shieldTransform;
	public Transform fireballTransform;
	public float timeBetweenShot = 1f;

	GameObject target = null;
    float timer;
//    int shootableMask;
	Rigidbody playerRigidbody;
	float effectsDisplayTime;
//	float camRayLength = 100f;
	float fireballTimer;
	Animation anim;
	bool shotAnimated = false;
	bool castAnimated = false;
	PlayerHealth playerHealth;
	bool enemyInRange = false;
//	bool isDefaultSkill = true;
	int holyFireDamage = 90;

    void Awake ()
    {
//        shootableMask = LayerMask.GetMask ("Shootable");
    }

	void Start(){
		Input.gyro.enabled = true;
		fireballTimer = timeBetweenShot;
		anim = GetComponent<Animation> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		playerHealth = GetComponent<PlayerHealth> ();
	}

    void Update ()
    {
		#if (UNITY_EDITOR && !GEARVR)
		if (Input.GetMouseButton(0)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit enemyHit;
			
			if (Physics.Raycast (camRay, out enemyHit, camRayLength, shootableMask) && enemyHit.collider.gameObject.tag == "Enemy"){
				target = enemyHit.collider.gameObject;
			}
		}
		// switch skill
		if (Input.GetMouseButtonDown(1))
			isDefaultSkill = !isDefaultSkill;
		#elif ((UNITY_ANDROID || UNITY_IOS) && !GEARVR)
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended) {
				Ray camRay = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit enemyHit;
				
				if (Physics.Raycast (camRay, out enemyHit, camRayLength, shootableMask) && enemyHit.collider.gameObject.tag == "Enemy")
					target = enemyHit.collider.gameObject;
			}
		}
		#else
//		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began)
//			isDefaultSkill = !isDefaultSkill;
		#endif
		Debug.Log (target != null);
		//condition that player can attack after time interval and there is target and target has health and player has health
		if (target != null && fireballTimer >= timeBetweenShot && !anim.IsPlaying("Wound")
		    && target.GetComponent<EnemyHealth> ().currentHealth > 0 && playerHealth.currentHealth > 0) {
//			fireballTimer = 0f;
			if (!enemyInRange) {
//				if (isDefaultSkill)
//					AnimateShoot();
//				else 
//					AnimateCast();
				if (Input.gyro.userAcceleration.z > 0.2f)
					AnimateShoot();
				else if (Input.gyro.userAcceleration.y > 0.13f)
					AnimateCast();
			} else {
				if (Input.gyro.userAcceleration.x > 0.13f)
					MeleeAttack ();
			}
		}

		fireballTimer += Time.deltaTime;
			
		//instantiate the tornado
		if (shotAnimated && !anim.IsPlaying("Skill01")) {
			Vector3 playerToEnemy = target.transform.position - transform.position;
			GameObject fireball = Instantiate (Fireball, fireballTransform.position, Quaternion.LookRotation (playerToEnemy)) as GameObject;
			fireball.GetComponent<Fireball>().Target = target;
//			target = null;
			shotAnimated = false;
		}

		if (castAnimated && !anim.IsPlaying ("Skill03")) {
//			target = null;
			castAnimated = false;
		}
	}
		
	void AnimateShoot(){
		TurnToEnemy ();
		anim.Play ("Skill01");
		shotAnimated = true;
		fireballTimer = 0f;
	}

	void AnimateCast() {
		TurnToEnemy ();
		if (!anim.IsPlaying("Skill03"))
			anim.Play ("Skill03");		
		Instantiate (HolyFire, target.transform.position, target.transform.rotation);
		target.GetComponent<EnemyHealth>().TakeDamage(holyFireDamage, 3);
		castAnimated = true;
		fireballTimer = 0f;
	}

	void MeleeAttack(){
		TurnToEnemy ();
		anim.Play ("Attack");
		Instantiate (Shield, shieldTransform.position, shieldTransform.rotation);
		enemyInRange = false;
		fireballTimer = 0f;
		target = null;
	}

	void TurnToEnemy(){
		Vector3 playerToEnemy = target.transform.position - transform.position;
		playerToEnemy.y = 0f;
		playerRigidbody.MoveRotation (Quaternion.LookRotation (playerToEnemy));
	}

	public bool IsCasting(){
		return shotAnimated || castAnimated;
	}

	public void SetTarget(GameObject enemy) {
		target = enemy;
	}

	public void OnTriggerStay(Collider col){
		if (col.gameObject.CompareTag ("Enemy")) {
			target = col.gameObject;
			enemyInRange = true;
		}
	}

	public void OnTriggerExit(Collider col){
		if (col.gameObject.CompareTag ("Enemy")) {
			enemyInRange = false;
		}
	}
}
