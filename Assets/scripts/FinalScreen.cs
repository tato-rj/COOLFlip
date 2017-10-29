using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScreen : MonoBehaviour {

	public GameObject congrats;
	public GameObject number;
	public GameObject continueButton;
	public GameObject backToHome;
	public GameObject rate;
	public GameObject[] otherNumbers;

	int levelName;
	int[] index;

	void Awake () {

		levelName = GameController.instance.gridIndex + 2;
		Color32 levelColor = GameController.instance.colors [GameController.instance.gridIndex].color;

		number.GetComponent<Text> ().text = levelName.ToString();
		number.GetComponent<Text> ().color = levelColor;

		if (levelName > 3) {
			otherNumbers [0].SetActive (true);
		}

		if (levelName > 4) {
			otherNumbers [1].SetActive (true);
		}

		continueButton.GetComponent<Image> ().color = levelColor;

		index = new int[17] {8,3,14,4,1,9,10,2,11,15,2,7,0,6,13,5,12};
	}

	void Start () {
	
		StartCoroutine (Sequence ());

	}

	public IEnumerator Sequence () {

		yield return new WaitForSeconds (1.5f);

		number.SetActive (true);

		yield return new WaitForSeconds (1.6f);

		number.GetComponent<Animation> ().Play ("jumpNumber");
		otherNumbers [0].GetComponent<Animation> ().Play ();
		otherNumbers [1].GetComponent<Animation> () ["jumpOtherNumber"].speed = 1.1f;
		otherNumbers [1].GetComponent<Animation> ().Play ();
		GetComponent<AudioSource> ().Play ();

		for (int i = 0; i < index.Length; i++) {
			congrats.transform.GetChild (index[i]).GetComponent<Animation> ().Play ();
			yield return new WaitForSeconds (0.1f);
		}

		yield return new WaitForSeconds (0.75f);

		if (levelName == 5) {
			backToHome.SetActive (true);
		} else {
			continueButton.SetActive (true);
		}

		rate.SetActive (true);

	}

}
