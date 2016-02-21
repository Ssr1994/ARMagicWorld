using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
	public float damageInterval = 1f;



    Animation anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    //PlayerShooting playerShooting;
    bool isDead;
	bool damaged;
	float damageTimer;

    void Awake ()
    {
        anim = GetComponent <Animation> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        //playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
		damageTimer = damageInterval;
    }


    void Update ()
    {
		damageTimer += Time.deltaTime;
        /*if(damaged)
            damageImage.color = flashColour;
        else
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        damaged = false;*/
    }


    public void TakeDamage (int amount)
    {
        damaged = true;
        currentHealth -= amount;
        //healthSlider.value = currentHealth;
        playerAudio.Play ();
		if (!anim.IsPlaying ("Wound"))
			anim.Play ("Wound");
        if(currentHealth <= 0 && !isDead)
            Death ();
    }


    void Death ()
    {
        isDead = true;

        //playerShooting.DisableEffects ();

		anim.Play ("HitAway");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        //playerShooting.enabled = false;
    }

	public void OnTriggerEnter(Collider col){
		if(( col.gameObject.CompareTag ("RegularSword") || col.gameObject.CompareTag ("PowerfulSword") ) && damageTimer>damageInterval && col.transform.root.gameObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack_slice")){
			if (col.gameObject.CompareTag ("RegularSword")) {
				if (!isDead)
					TakeDamage (10);
			} else if (col.gameObject.CompareTag ("PowerfulSword")) {
				if (!isDead)
					TakeDamage (20);
			}
			damageTimer = 0f;
		}
	}
}
