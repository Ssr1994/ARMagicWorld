//#define GEARVR
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    float timer;
	float flareTimer=0;
    Ray shootRay;
    RaycastHit shootHit;
    int shootableMask;
    ParticleSystem gunParticles;
    LineRenderer gunLine;
    AudioSource gunAudio;
	Light gunLight;
	Rigidbody playerRigidbody;
	float effectsDisplayTime;
	float camRayLength = 100f;
	float fireballTimer;
	Animation anim;
	bool shotAnimated=false;
	PlayerHealth playerHealth;

	public GameObject target; // enemy to shoot
	public GameObject Fireball;
	public GameObject Shield;
	public Transform shieldTransform;
	public Transform fireballTransform;
	public float timeBetweenShot = 1f;
	public float meleeAttackRange = 2.5f;

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
    }

	void Start(){
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
		#elif ((UNITY_ANDROID || UNITY_IOS) && !GEARVR)
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended) {
				Ray camRay = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit enemyHit;
				
				if (Physics.Raycast (camRay, out enemyHit, camRayLength, shootableMask) && enemyHit.collider.gameObject.tag == "Enemy")
					target = enemyHit.collider.gameObject;
			}
		}
		#endif

		//condition that player can attack after time interval and there is target and target has health and player has health
		if (fireballTimer >= timeBetweenShot && target!=null && target.GetComponent<EnemyHealth>().currentHealth>0 && playerHealth.currentHealth>0) {
			fireballTimer = 0f;
			Vector3 playerToEnemy = target.transform.position - transform.position;
			if (playerToEnemy.magnitude > meleeAttackRange)
				AnimateShoot ();
			else
				MeleeAttack ();
		} else {
			fireballTimer += Time.deltaTime;
		}
		//instantiate the fireball
		if (shotAnimated && !anim.IsPlaying("Skill01")) {
			Vector3 playerToEnemy = target.transform.position - transform.position;
			GameObject fireball = Instantiate (Fireball, fireballTransform.position, Quaternion.LookRotation (playerToEnemy)) as GameObject;
			fireball.GetComponent<Fireball>().Target = target;
			target = null;
			shotAnimated = false;
		}
	}
		
	void AnimateShoot(){
		TurnToEnemy ();
		if (!anim.IsPlaying ("Skill01"))
			anim.Play ("Skill01");
		shotAnimated = true;
	}

	void MeleeAttack(){
		TurnToEnemy ();
		if (!anim.IsPlaying ("Attack")) {
			anim.Play ("Attack");
		}
		Instantiate (Shield, shieldTransform.position, shieldTransform.rotation);
		//target = null;
	}

	void TurnToEnemy(){
		Vector3 playerToEnemy = target.transform.position - transform.position;
		playerToEnemy.y = 0f;
		playerRigidbody.MoveRotation (Quaternion.LookRotation (playerToEnemy));
	}

	public bool ShotAnimated(){
		return shotAnimated;
	}
}
