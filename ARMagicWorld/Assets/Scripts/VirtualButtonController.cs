using UnityEngine;
using System.Collections;

public class VirtualButtonController : MonoBehaviour {
	GameObject enemy=null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(enemy != null)
			transform.position = enemy.transform.position;
	}

	public void SetEnemy(GameObject Enemy){
		if (enemy == null) {
			enemy = Enemy;
		}
	}

	public bool IsActive(){
		return enemy != null;
	}

	public GameObject GetTarget(){
		return enemy;
	}
}
