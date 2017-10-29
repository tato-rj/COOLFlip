using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Settings : MonoBehaviour {

	public static Settings instance;

	public GameObject settingsPanel;

	void Start () {
		MainSettings.instance.SetColors ();
	}

	public void SelectLevel (string name) {

		SceneManager.LoadScene (name);

	}

	public void Tutorial () {
		UserData.instance.ResetCurrentLevel ();
		StartGame ();
	}

	public void Retry () {
		Scene game = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (game.buildIndex);
	}

	public void CloseSettings () {
		
		this.transform.parent.gameObject.SetActive (false);
	
	}

	public void StartGame () {
		SceneManager.LoadScene ("game");
	}

	public void HomeScreen () {
		SceneManager.LoadScene ("intro");
	}

	public void ShowSettings () {
	
		settingsPanel.SetActive (true);

	}

	public void SaveColorMode () {
		MainSettings.instance.isDayTime = !MainSettings.instance.isDayTime;
		UserData.instance.SaveColorMode (MainSettings.instance.isDayTime);
	}

}

