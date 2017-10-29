using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RateApp : MonoBehaviour {

	public static RateApp instance;

	public GameObject starsContainer;
	public Sprite yellowStar;
	public Sprite greyStar;
	public GameObject actionButton;

	void Awake () {
		instance = this;
	}

	void Start () {
		UpdateStars (UserData.instance.LoadRating());
	}

	public void UpdateStars (int starPos) {

		for (int i = 0; i <= 4; i++) {
			if (i <= starPos) {
				starsContainer.transform.GetChild (i).GetComponent<Image> ().sprite = yellowStar;	
			} else {
				starsContainer.transform.GetChild (i).GetComponent<Image> ().sprite = greyStar;
			}

			if (starPos >= 3) {
				actionButton.GetComponentInChildren<Text>().text = "CLICK TO RATE";
				actionButton.SetActive (true);
			} else if (starPos >= 0) {
				actionButton.GetComponentInChildren<Text>().text = "GET IN TOUCH";
				actionButton.SetActive (true);
			}
		}
	}

	public void SetRatings () {
		
		int starPos = int.Parse(this.gameObject.name);
		UpdateStars (starPos);
		UserData.instance.SaveRating (starPos);

	}

	public void ActionButton () {

		if (actionButton.GetComponentInChildren<Text> ().text == "CLICK TO RATE") {
			UIFunctions.instance.RateApp();
		} else {
			UIFunctions.instance.SendEmail ();
		}

	}
}
