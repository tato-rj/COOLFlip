using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour {

	public static SelectLevel instance;


	public int gridIndex;
	Button[] buttons;

	void Awake () {

		instance = this;
		buttons = GetComponentsInChildren<Button> ();
		EnableButtons ();
	}

	public void Return () {
		SceneManager.LoadScene ("game");
	}

	void EnableButtons () {

		int[] solvedLevels = MainSettings.instance.solvedLevels;

		for (int i = 0; i < buttons.Length; i++) {

			if (i <= solvedLevels[gridIndex]) {
				buttons [i].interactable = true;
			} else {
				buttons [i].interactable = false;
			}

		}

	}
}
