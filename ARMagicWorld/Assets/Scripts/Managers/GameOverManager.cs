using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    public PlayerHealth playerHealth;
	public float restartDelay = 5f;

	static bool paused;

    Animator anim;
	float restartTimer;

    void Awake()
    {
        anim = GetComponent<Animator>();
		paused = false;
    }


    void Update()
    {
		if (playerHealth.currentHealth <= 0) {
			anim.SetTrigger ("GameOver");

			restartTimer += Time.deltaTime;

			// .. if it reaches the restart delay...
			if(restartTimer >= restartDelay)
				Application.LoadLevel("Start");
		}

		if (Input.touchCount == 2 && Input.GetTouch(1).phase == TouchPhase.Began) {
			paused = !paused;
			if (paused)
				Time.timeScale = 0f;
			else
				Time.timeScale = 1f;
		}
    }
}
