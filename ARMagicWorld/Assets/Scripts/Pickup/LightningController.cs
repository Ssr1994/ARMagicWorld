using UnityEngine;
using System.Collections;

public class LightningController : MonoBehaviour {
	public GameObject effect;
//	public GameObject lightningStrike;
//	public GameObject lightningBlast;

	GameObject effectObject = null;

//	int strikeDamage = 100;
		
	// Update is called once per frame
	void Update () {
		if (effectObject == null){
			effectObject = Instantiate (effect, transform.position, transform.rotation) as GameObject;
			effectObject.transform.parent = transform;
		}
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Player")) {
			//ligtning strike
//			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
//			GetComponent<SphereCollider>().enabled = false;
//			foreach (GameObject enemy in enemies)
//				yield return StartCoroutine(initLightnings(enemy));
			PlayerAttack.lightningCharged = true;
			Destroy (gameObject);
		}
	}
//
//	IEnumerator initLightnings(GameObject enemy) {
//			Instantiate (lightningStrike, enemy.transform.position+enemy.transform.up*0.15f, Quaternion.identity);
//			Instantiate (lightningBlast, enemy.transform.position, Quaternion.identity);
//			EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth> ();
//			enemyHealth.TakeDamage (strikeDamage);
//			
//			yield return new WaitForSeconds(0.1f); // Wait
//	}
}
