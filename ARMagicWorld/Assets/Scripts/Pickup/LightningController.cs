using UnityEngine;
using System.Collections;

public class LightningController : MonoBehaviour {
	public GameObject effect;
	public GameObject lightningStrike;
	public GameObject lightningBlast;
	public GameObject effectObject=null;
	public int strikeDamage = 100;

	// Use this for initialization
	void Start () {
	
	}
		
	// Update is called once per frame
	void Update () {
		if (effectObject == null){
			effectObject = Instantiate (effect, transform.position, transform.rotation) as GameObject;
			effectObject.transform.parent = transform;
		}
	}

	public void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Player")) {
			//ligtning strike
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject enemy in enemies) {
				GameObject strike = Instantiate (lightningStrike, enemy.transform.position-enemy.transform.up*0.15f, Quaternion.identity) as GameObject;
				GameObject blast = Instantiate (lightningBlast, enemy.transform.position, Quaternion.identity) as GameObject;
				strike.transform.parent = enemy.transform;
				blast.transform.parent = enemy.transform;
				EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth> ();
				enemyHealth.TakeDamage (strikeDamage);
			}
			Destroy (gameObject);
		}
	}
}
