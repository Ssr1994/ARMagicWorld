using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public int scoreValue = 10;
	public AudioClip deathClip;
	public GameObject deathEffect;
	/*
	public GameObject dustStorm;
	public GameObject flashBang;
	public GameObject 
*/
    Animator anim;
    AudioSource enemyAudio;
    CapsuleCollider capsuleCollider;
	Rigidbody rigidbody;
    bool isDead = false;
	bool isSinking = false;
	int wandDamage = 35;
	float sinkSpeed = 0.3f;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();
		rigidbody = GetComponent<Rigidbody> ();
        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
    }


    public void TakeDamage (int amount, int skill)
    {
        if(isDead)
            return;

        enemyAudio.Play ();
		anim.SetTrigger ("damaged");
        currentHealth -= amount;
        if(currentHealth <= 0)
            Death (skill);
    }


    void Death (int skill)
    {
		isDead = true;
        capsuleCollider.isTrigger = true;
		Instantiate (deathEffect, transform.position - transform.forward * 0.4f, transform.rotation);
        anim.SetTrigger ("dead");		
		Destroy (gameObject, 1.1f);
		EnemyManager.enemyNum--;
		ScoreManager.score += scoreValue;
		GetComponent <NavMeshAgent> ().enabled = false;
		isSinking = true;
		GetComponent <Rigidbody> ().isKinematic = true;

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }
    
	public bool IsDead(){
		return isDead;
	}


	public void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Wand") && col.transform.root.gameObject.GetComponent<Animation>().IsPlaying("Attack")) {
			TakeDamage (wandDamage, 0);
		}
	}
}
