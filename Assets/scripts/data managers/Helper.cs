using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Helper : MonoBehaviour {

	public GameObject msgBox;
	public GameObject needMoreHelp;
	public GameObject x;
	Transform puzzle;
	int numOfHelps;
	bool helpWasClicked;

	void Awake () {

		helpWasClicked = false;

		puzzle = GameController.instance.selectedGrid;

		GetComponent<Button> ().interactable = (MainSettings.instance.usedHelp) ? false : true;	
	
		if (!GetComponent<Button>().interactable) {
			transform.localScale = new Vector3 (1,1,1);
		}

		MainSettings.instance.usedHelp = false;

		if (MainSettings.instance.records [GameController.instance.gridIndex, GameController.instance.levelIndex] > 3) {
			numOfHelps = 2;
		} else {
			numOfHelps = 1;
		}

		needMoreHelp.transform.GetChild(3).GetComponent<Image>().color = GameController.instance.colors [GameController.instance.gridIndex].color;
	}

	void Start () {
		if (GameController.instance.gridIndex != 0) {
			if (GetComponent<Button> ().interactable) {
				if (AdsManager.instance.RewardedVideoIsLoaded()) {
					StartCoroutine (ShowLightBulb ());
				}
			} else {
				ShowHelp ();
			}
		}

	}

	void ShowHelp () {

		int count = numOfHelps;
		Transform[] tiles = puzzle.GetComponentsInChildren<Transform> ();

		for (int i=0; i < tiles.Length; i++) {
			if (tiles[i].name.Contains("+") && count > 0) {

				GameObject marker = Instantiate (x, new Vector3(0,0,0), Quaternion.identity);
				marker.transform.SetParent(tiles [i].transform);
				marker.transform.localScale = new Vector3 (1,1,1);
				marker.GetComponent<RectTransform> ().offsetMin = new Vector2 (0, 0);
				marker.GetComponent<RectTransform> ().offsetMax = new Vector2 (0, 0);
				//count--;

			}
		}
			
	}

	public void ToggleHelpScreen() {
		needMoreHelp.SetActive (!needMoreHelp.activeSelf);
		helpWasClicked = true;
	}

	public void WatchRewardedVideo () {
		StartCoroutine (ShowHelpSequence ());
	}

	public IEnumerator ShowHelpSequence () {

		PressButton.instance.ButtonUp ();

		yield return new WaitForSeconds (0.1f);

		#if UNITY_EDITOR
		MainSettings.instance.usedHelp = true;
		Scene game = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (game.buildIndex);
		#else
		// Show video
		AdsManager.instance.ShowRewardVideo ();
		#endif

	}

	public IEnumerator ShowLightBulb () {

		yield return new WaitForSeconds (0.75f);

		GetComponent<Animation> ().Play ();
		GetComponent<AudioSource> ().Play ();

		yield return new WaitForSeconds (5f);

		if (!helpWasClicked) {
			msgBox.SetActive (true);

			yield return new WaitForSeconds (3f);

			msgBox.SetActive (false);
		}

	}
}
