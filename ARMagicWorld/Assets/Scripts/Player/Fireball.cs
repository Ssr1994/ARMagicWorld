using UnityEngine;
using System.Collections;

public class Fireball : MonoBehaviour {
	public GameObject Target;
	public float angularSpeed = 40f;
	public float speed = 5f;
	public float DestroyTime=20f;
	public GameObject effectObject;
	public int dealDamage = 20;

	float timer=0f;
	GameObject effect=null;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if (timer > DestroyTime)
			Destroy (this.gameObject);

		//instantiate effect
		if (effect == null) {
			effect = Instantiate (effectObject, transform.position, transform.rotation) as GameObject;
			effect.transform.parent = transform;
		}
		if (Target != null) {
			Vector3 ballToEnemy = Target.transform.position - transform.position;
			float estimatedTimeOfImpact = ballToEnemy.magnitude / speed;
			Vector3 estimatedImpactPos = Target.transform.position + Target.transform.forward * estimatedTimeOfImpact;
			Vector3 ballToImpact = estimatedImpactPos - transform.position;
			ballToImpact.y = 0f;
			transform.rotation = Quaternion.RotateTowards (transform.rotation, Quaternion.LookRotation (ballToImpact), angularSpeed * Time.deltaTime);
			Vector3 dir = (new Vector3 (transform.forward.x, 0f, transform.forward.z)).normalized;
			transform.position = transform.position + dir * speed * Time.deltaTime;
		}
	}

	public void OnTriggerEnter(Collider col){
		if (col.CompareTag ("Enemy") && !col.isTrigger) {
			col.gameObject.GetComponent<EnemyHealth> ().TakeDamage (dealDamage);
		}
	}
}
