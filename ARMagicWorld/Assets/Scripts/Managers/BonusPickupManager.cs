using UnityEngine;
using System.Collections;

public class BonusPickupManager : MonoBehaviour {
	public GameObject flareBonus;
	//public GameObject imageTarget;
	public int scoreToSpawnFlare;
	public Transform[] flareSpawnPoint;

	int prevBonusScore=0;
	GameObject instantiatedFlare=null;
	
	// Update is called once per frame
	void Update () {
		//possible to spawn flare when score point reached
		if (ScoreManager.score-prevBonusScore >= scoreToSpawnFlare) {
			//cannot spawn new one if old one is not picked up
			if (instantiatedFlare == null) {
				int index = Random.Range (0, flareSpawnPoint.Length);
				instantiatedFlare = Instantiate (flareBonus, flareSpawnPoint [index].position, flareSpawnPoint [index].rotation) as GameObject;
				//instantiatedFlare.transform.parent = imageTarget.transform;
			}
			prevBonusScore = ScoreManager.score;
		}
	}
}
