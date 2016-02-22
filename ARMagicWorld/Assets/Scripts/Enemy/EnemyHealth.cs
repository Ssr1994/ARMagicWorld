using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public int scoreValue = 10;
    public AudioClip deathClip;

    Animator anim;
    AudioSource enemyAudio;
    CapsuleCollider capsuleCollider;
    bool isDead = false;
	bool isSinking = false;
	int wandDamage = 20;
	float sinkSpeed = 2.5f;

    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
    }


    public void TakeDamage (int amount)
    {
        if(isDead)
            return;

        enemyAudio.Play ();
		anim.SetTrigger ("damaged");
        currentHealth -= amount;
        if(currentHealth <= 0)
            Death ();
    }


    void Death ()
    {
        isDead = true;
        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking () // called as an animation event
    {
        GetComponent <NavMeshAgent> ().enabled = false;
        GetComponent <Rigidbody> ().isKinematic = true; // Avoid static computing
		isSinking = true;
		EnemyManager.enemyNum--;
		ScoreManager.score += scoreValue;
		Destroy (gameObject, 2f);
    }

	public bool IsDead(){
		return isDead;
	}

	public void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Wand") && col.transform.root.gameObject.GetComponent<Animation>().IsPlaying("Attack")) {
			TakeDamage (wandDamage);
		}
	}
}
