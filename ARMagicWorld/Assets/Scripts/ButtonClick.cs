using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour {

	public void OpenTutorial() {
		Application.LoadLevel ("Tutorial");
	}

	public void StartGame() {
		Application.LoadLevel ("AR");
	}
}
