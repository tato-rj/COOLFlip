using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAnimation : MonoBehaviour {

	public GameObject startButton;
	public GameObject rateButton;

	void Start () {
		StartCoroutine (FlipMe ());	
	}

	public IEnumerator FlipMe () {
	
		yield return new WaitForSeconds (0.1f);

		GetComponents<AudioSource> ()[0].Play ();
		startButton.SetActive (true);
		rateButton.SetActive (true);

		yield return new WaitForSeconds (0.4f);

		GetComponents<AudioSource> ()[1].Play ();
		GetComponent<Animation> ().Play ();

	}

}
