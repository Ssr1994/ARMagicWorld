#define GEARVR
using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
	public static bool lightningCharged = false;

	public GameObject Fireball;
	public GameObject HolyFire;
	public GameObject Shield;
	public GameObject lightningStrike;
	public GameObject lightningBlast;
	public GameObject MagicCircle;
	public Transform shieldTransform;
	public Transform fireballTransform;
	float timeBetweenShot = 0.8f;

	GameObject target = null;
	GameObject targetHighlight = null;
    float timer;
    int shootableMask;
	Rigidbody playerRigidbody;
	float effectsDisplayTime;
	float camRayLength = 100f;
	float fireballTimer = 0f;
	float lightningTimer = 0f;
	Animation anim;
	bool shotAnimated = false;
	bool castAnimated = false;
	PlayerHealth playerHealth;
	bool enemyInRange = false;
	int holyFireDamage = 90;
	int strikeDamage = 100;
	AudioSource tornadoSound;
	AudioSource holyFireSound;
	AudioSource lightningStrikeSound;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
		AudioSource[] audios = GetComponents<AudioSource> ();
		tornadoSound = audios [1];
		lightningStrikeSound = audios [2];
		holyFireSound = audios [3];
    }

	void Start(){
		Input.gyro.enabled = true;
		fireballTimer = timeBetweenShot;
		anim = GetComponent<Animation> ();
		playerRigidbody = GetComponent<Rigidbody> ();
		playerHealth = GetComponent<PlayerHealth> ();
		targetHighlight = Instantiate (MagicCircle, Vector3.zero, Quaternion.identity) as GameObject;
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

		if (target == null) {
			if (EnemyManager.enemyNum > 5) {
				// Autofocus
				GameObject enemy = GameObject.FindGameObjectWithTag("Enemy");
				if (enemy == null)
					targetHighlight.SetActive (false);
				else {
					target = enemy;
					targetHighlight.transform.position = target.transform.position;
				}
			} else
				targetHighlight.SetActive (false);
		} else {
			targetHighlight.SetActive(true);
			targetHighlight.transform.position = target.transform.position;
		}

		//condition that player can attack after time interval and there is target and target has health and player has health
		if (target != null && fireballTimer >= timeBetweenShot && !anim.IsPlaying("Wound")
		    && target.GetComponent<EnemyHealth> ().currentHealth > 0 && playerHealth.currentHealth > 0) {
//			fireballTimer = 0f;
			if (!enemyInRange) {
				if (Input.gyro.userAcceleration.z > 0.14f)
					AnimateShoot();
				else if (Input.gyro.userAcceleration.y > 0.12f)
					AnimateCast();
			} else {
				MeleeAttack ();
			}
		}

		if (lightningCharged && Input.gyro.userAcceleration.x > 0.12f) {
			anim.Play ("Skill02");
			castAnimated = true;
			lightningCharged = false;
		}

		fireballTimer += Time.deltaTime;
		lightningTimer += Time.deltaTime;
			
		//instantiate the tornado
		if (shotAnimated && !anim.IsPlaying("Skill01")) {
			Vector3 playerToEnemy = target.transform.position - transform.position;
			GameObject fireball = Instantiate (Fireball, fireballTransform.position, Quaternion.LookRotation (playerToEnemy)) as GameObject;
			fireball.GetComponent<Fireball>().Target = target;
			tornadoSound.Play();
//			target = null;
			shotAnimated = false;
		}

		if (castAnimated && !anim.IsPlaying ("Skill02")) {
			StartCoroutine(CastLightnings());
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
		anim.Play ("Skill03");
		Instantiate (HolyFire, target.transform.position, target.transform.rotation);
		holyFireSound.Play ();
		target.GetComponent<EnemyHealth>().TakeDamage(holyFireDamage);
		fireballTimer = 0f;
	}

	IEnumerator CastLightnings() {
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
			yield return StartCoroutine(InitLightnings(enemy));
	}
	
	IEnumerator InitLightnings(GameObject enemy) {
		Instantiate (lightningStrike, enemy.transform.position+enemy.transform.up*0.15f, Quaternion.identity);
		Instantiate (lightningBlast, enemy.transform.position, Quaternion.identity);
		lightningStrikeSound.Play ();
		EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth> ();
		enemyHealth.TakeDamage (strikeDamage);
		
		yield return new WaitForSeconds(0.1f); // Wait
	}

	void MeleeAttack(){
		TurnToEnemy ();
		anim.Play ("Attack");
		Instantiate (Shield, shieldTransform.position, shieldTransform.rotation);
		enemyInRange = false;
		fireballTimer = 0f;
	}

	void TurnToEnemy(){
		Vector3 playerToEnemy = target.transform.position - transform.position;
		playerToEnemy.y = 0f;
		playerRigidbody.MoveRotation (Quaternion.LookRotation (playerToEnemy));
	}

	public bool IsCasting(){
		return shotAnimated || castAnimated || anim.IsPlaying("Skill03");
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
