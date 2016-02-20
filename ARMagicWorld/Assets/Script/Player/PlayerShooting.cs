//#define EPSON
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
	public GameObject FlareUI;
	public GameObject safeZone;
	public GameObject imageTarget;
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.12f;
    public float range = 100f;

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
	public GameObject target; // enemy to shoot

	Transform[] flareImages;
	int flareCount = 0;
    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
		playerRigidbody = transform.parent.GetComponent<Rigidbody> ();
		effectsDisplayTime = 0.2f * timeBetweenBullets;
    }

	void Start(){
		flareImages = new Transform[6];
		int i = 0;
		foreach (Transform child in FlareUI.transform) {
			flareImages [i] = child;
			i++;
		}
	}

    void Update ()
    {
        timer += Time.deltaTime;
		if (flareTimer > 0)
			flareTimer -= Time.deltaTime;
		else
			flareTimer = 0;
		/*
		#if (UNITY_EDITOR && !EPSON)
		if (Input.GetMouseButton(0)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit enemyHit;
			
			if (Physics.Raycast (camRay, out enemyHit, camRayLength, shootableMask) && enemyHit.collider.gameObject.tag == "Enemy")
				target = enemyHit.collider.gameObject;
		}
		#elif ((UNITY_ANDROID || UNITY_IOS) && !EPSON)
		foreach (Touch touch in Input.touches) {
			if (touch.phase != TouchPhase.Ended) {
				Ray camRay = Camera.main.ScreenPointToRay (touch.position);
				RaycastHit enemyHit;
				
				if (Physics.Raycast (camRay, out enemyHit, camRayLength, shootableMask) && enemyHit.collider.gameObject.tag == "Enemy")
					target = enemyHit.collider.gameObject;
			}
		}
		#endif
		*/
		if (target != null) { // Turning
			Vector3 playerToMouse = target.transform.position - transform.position;
			playerToMouse.y = 0f; // No rotation on y, actually not needed here
			playerRigidbody.MoveRotation (Quaternion.LookRotation (playerToMouse));
		}

		if(target != null && target.GetComponent<EnemyHealth>().currentHealth > 0 && timer >= timeBetweenBullets && Time.timeScale != 0)
            Shoot ();

        if(timer >= effectsDisplayTime)
            DisableEffects ();

		if (flareTimer <= 0 && flareCount > 0 && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetButton("Fire3"))) {
			SpendFlare ();
			flareTimer = 1f;
		}
    }


    public void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

        gunAudio.Play ();

        gunLight.enabled = true;

		gunParticles.Simulate (0.01f);
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
            if(enemyHealth != null)
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            
            gunLine.SetPosition (1, shootHit.point);
        }
        else
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
    }

	public void GetFlare(){
		flareCount++;
		if (flareCount > 6)
			flareCount = 6;
		flareImages [flareCount-1].gameObject.SetActive(true);
	}

	void SpendFlare(){
		if (flareCount > 0) {
			flareImages [flareCount - 1].gameObject.SetActive (false);
			GameObject flare = Instantiate (safeZone, transform.position, transform.rotation) as GameObject;
			flare.transform.parent = imageTarget.transform;
			flareCount--;
		}
	}
}
