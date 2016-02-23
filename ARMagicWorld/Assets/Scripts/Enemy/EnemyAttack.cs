using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
    public float timeBetweenAttacks = 1f;
    public int attackDamage = 10;
	
    Animator anim;
    GameObject player;
    PlayerHealth playerHealth;
    EnemyHealth enemyHealth;
    bool playerInRange;
    float timer;
	bool laugh=false;

    void Awake ()
    {
        player = GameObject.FindWithTag ("Player");
        playerHealth = player.GetComponent <PlayerHealth> ();
        enemyHealth = GetComponent<EnemyHealth>();
        anim = GetComponent <Animator> ();
    }


    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject == player)
            playerInRange = true;
    }


    void OnTriggerExit (Collider other)
    {
        if(other.gameObject == player)
            playerInRange = false;
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if (playerHealth.currentHealth <= 0 && !laugh) {
			anim.SetTrigger ("playerDead");
			laugh = true;
		} else if(timer >= timeBetweenAttacks && playerInRange && enemyHealth.currentHealth > 0)
			Attack ();

    }


    void Attack ()
    {
        timer = 0f;
		anim.SetTrigger ("attacking");
        //if(playerHealth.currentHealth > 0)
            //playerHealth.TakeDamage (attackDamage);
    }
}
