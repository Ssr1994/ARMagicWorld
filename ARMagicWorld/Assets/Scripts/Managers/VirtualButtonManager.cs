using UnityEngine;
using System.Collections;
using Vuforia;

public class VirtualButtonManager : MonoBehaviour, IVirtualButtonEventHandler{
	//public GameObject buttonPool;
	//public GameObject button;

	public GameObject player;
	VirtualButtonBehaviour[] vbs;
	// Use this for initialization
	void Start () {

		vbs = GetComponentsInChildren<VirtualButtonBehaviour>();
		for (int i = 0; i < vbs.Length; ++i) {
			vbs[i].RegisterEventHandler(this);
		}
	}
		

	public void OnButtonPressed(VirtualButtonAbstractBehaviour vb){
		if (vb.GetComponent<VirtualButtonController> ().IsActive ()) {
			player.GetComponentInChildren<PlayerAttack> ().SetTarget(vb.GetComponent<VirtualButtonController> ().GetTarget ());

		}
	}

	public void OnButtonReleased(VirtualButtonAbstractBehaviour vb){
		//Debug.Log ("Released");
	}

	//assign a virtual Button to game object
	public void AssignVirtualButton(GameObject character){
		for(int i = 0; i < vbs.Length; ++i){
			if (!vbs[i].GetComponent<VirtualButtonController>().IsActive()) {
				vbs[i].GetComponent<VirtualButtonController>().SetEnemy(character);
				break;
			}
		}
	}
}
