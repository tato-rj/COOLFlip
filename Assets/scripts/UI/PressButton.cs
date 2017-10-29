using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PressButton : MonoBehaviour {

	public static PressButton instance;

	public string targetScene;
	public int levelIndex;
	public int gridIndex;
	public GameObject title;

	Color32 bg;
	bool active;

	float shadowDist;

	AudioSource buttonSound;

	void Awake () {

		instance = this;

		shadowDist = GetComponent<Shadow> ().effectDistance.y;
		bg = GetComponent<Image> ().color;
		buttonSound = GetComponent<AudioSource> ();

	}

	void Start () {
		active = GetComponent<Button> ().IsInteractable ();

		if (!active) {
			DisableButton ();
		}
	}

	public void Press () {
		if (active) {
			buttonSound.Play ();
			ButtonDown ();
		}
	}

	public void Release () {
		if (active) {

			ButtonUp ();

			if (levelIndex >= 0) {
				UserData.instance.SaveLevel (gridIndex, levelIndex);	
				StartCoroutine (GoToLevel ());
			} else {
				if (targetScene == "rate") {
					UIFunctions.instance.RateApp ();
				} else if (targetScene == "email") {
					UIFunctions.instance.SendEmail ();
				} else {
					SceneManager.LoadScene (targetScene);
				}
			}
		}
	}

	public void WatchVideo () {
		if (active) {
			ButtonUp ();
			#if UNITY_EDITOR
			GameController.instance.AddMoves();
			#else
			// Show video
			AdsManager.instance.ShowInterstitialAd ();
			#endif

		}
	}

	void DisableButton () {
		ButtonDown ();
		GetComponent<Image> ().color = new Color32 (bg.r, bg.g, bg.b, 50);
	}

	public void ButtonDown() {
		GetComponent<Shadow> ().enabled = false;
		transform.position = new Vector3 (transform.position.x, transform.position.y + shadowDist, transform.position.z);
	}

	public void ButtonUp() {
		GetComponent<Shadow> ().enabled = true;
		transform.position = new Vector3 (transform.position.x, transform.position.y - shadowDist, transform.position.z);
	}

	public IEnumerator GoToLevel () {

		title.GetComponent<Animation> ().Play ();

		yield return new WaitForSeconds (0.4f);

		foreach (Transform child in transform.parent) {
			child.GetComponent<Animation>().Play();	
		}

		yield return new WaitForSeconds(0.1f);

		transform.parent.GetComponent<AudioSource> ().Play ();

		yield return new WaitForSeconds(0.5f);

		SceneManager.LoadScene (targetScene);

	}
}
