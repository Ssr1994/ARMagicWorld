using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    //ParticleSystem hitParticles;
	//ParticleSystem deathParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        //ParticleSystem[] temp = GetComponentsInChildren <ParticleSystem> ();
		//hitParticles = temp [0];
		//deathParticles = temp [1];
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();
		anim.SetTrigger ("damaged");
        currentHealth -= amount;
            
        //hitParticles.transform.position = hitPoint;
		//hitParticles.Simulate (0.01f); // Maybe a bug in unity
		//hitParticles.Play ();

        if(currentHealth <= 0)
            Death ();
    }


    void Death ()
    {
        isDead = true;

		//deathParticles.transform.position = transform.position;
		//deathParticles.Play ();

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
        ScoreManager.score += scoreValue; // since it is static
        Destroy (gameObject, 2f);
    }

	public bool IsDead(){
		return isDead;
	}
}
