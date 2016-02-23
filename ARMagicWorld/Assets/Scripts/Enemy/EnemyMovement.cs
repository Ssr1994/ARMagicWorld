using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
	//float speed = 3f;
	Transform player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    NavMeshAgent nav;
	Animator anim;

    void Awake ()
    {
		// Because enemies do not exist at the very start of the game, we cannot use a public var
		player = GameObject.FindWithTag ("Player").transform;
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent <EnemyHealth> ();
        nav = GetComponent <NavMeshAgent> ();
		anim = GetComponent <Animator> ();
    }


    void Update ()
    {
		if (enemyHealth.currentHealth > 0 && playerHealth.currentHealth > 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("walk")) {
			nav.enabled = true;
			nav.SetDestination (player.position);
		} else {
			nav.enabled = false;
		}
    }

	/*void OnTriggerStay (Collider collider)
	{
		if (collider.CompareTag ("Safezone")) {
			Vector3 posOffset = transform.position - collider.transform.position;
			float distanceToSafezone = posOffset.sqrMagnitude;
			float killingRadius = collider.GetComponent<Safezone> ().killingRadius * collider.GetComponent<Safezone> ().killingRadius;
			//in killing zone, zombie dies
			if (distanceToSafezone < killingRadius && !enemyHealth.IsDead ()) {
				enemyHealth.TakeDamage (100, transform.position);
			} else { //zombie gets pushed away
				transform.position = transform.position + posOffset.normalized * speed * Time.deltaTime;
			}
		}
	}*/
}
