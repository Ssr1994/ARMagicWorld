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
	public GameObject target; // enemy to shoot

    void Awake ()
    {
        shootableMask = LayerMask.GetMask ("Shootable");
    }

    void Update ()
    {
		#if (UNITY_EDITOR && !GEARVR)
		if (Input.GetMouseButton(0)) {
			Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit enemyHit;
			
			if (Physics.Raycast (camRay, out enemyHit, camRayLength, shootableMask) && enemyHit.collider.gameObject.tag == "Enemy")
				target = enemyHit.collider.gameObject;
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
    }
}
