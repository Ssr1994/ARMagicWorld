using UnityEngine;
using System.Collections;

public class ButtonClick : MonoBehaviour {

	public void OpenTutorial() {
		Application.LoadLevel ("Tutorial1");
	}

	public void StartGame() {
		Application.LoadLevel ("AR");
	}

	public void NextTutorial() {
		Application.LoadLevel ("Tutorial2");
	}

	public void BackToMenu() {
		Application.LoadLevel ("Start");
	}
}
