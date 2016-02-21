using UnityEngine;
using System.Collections;

public class PickupManager : MonoBehaviour {
	//public GameObject imageTarget;
	public GameObject spawnObject;
	public float spawnTime = 20f;
	public Transform[] spawnPoints;

	GameObject instantiatedObj;
	void Start ()
	{
		InvokeRepeating ("Spawn", 10f, spawnTime);
	}

		
	void Spawn ()
	{
		if (instantiatedObj != null) {
			Destroy (instantiatedObj);
		}
		int spawnPointIndex = Random.Range (0, spawnPoints.Length);
		instantiatedObj = Instantiate (spawnObject, spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation) as GameObject;
		//instantiatedObj.transform.parent = imageTarget.transform;
	}

}
