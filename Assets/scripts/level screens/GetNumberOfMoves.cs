using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetNumberOfMoves : MonoBehaviour {

	public int grid;

	public GameObject controllerPrefab;
	public GameObject crownPrefab;
	public GameObject numberPrefab;
	public GameObject numOfMovesPrefab;
	public Font bold;

	int[,] moves = new int[4,25];

	void Awake () {

		moves = UserData.instance.LoadMoves ();

	}

	void Start () {

		for (int i = 0; i < transform.childCount; i++) {

			Transform button = transform.GetChild (i);

			GameObject number = InstantiateObject (numberPrefab, button, Vector3.zero);
			number.GetComponent<Text> ().text = (i+1).ToString ();

			GameObject numOfMoves = InstantiateObject (numOfMovesPrefab, button, new Vector3 (0, -108f, 0));

			if (moves[grid,i] > 0) {

				numOfMoves.GetComponent<Text>().text = moves[grid,i] + " moves";

				if (moves[grid,i] == MainSettings.instance.records[grid,i]) {
					InstantiateObject (crownPrefab, button, Vector3.zero);
					numOfMoves.GetComponent<Text> ().font = bold;
					numOfMoves.GetComponent<Text> ().material = button.GetComponent<Image> ().material;
					number.SetActive (false);
				}
			}

			if (moves[grid,i] == 0 && button.GetComponent<Button>().IsInteractable()) {
				InstantiateObject (controllerPrefab, button, new Vector3(0,3f,0));
				number.SetActive (false);
			}
		}
	}

	GameObject InstantiateObject (GameObject obj, Transform parent, Vector3 pos) {
		GameObject newObj = Instantiate (obj, Vector3.zero, Quaternion.identity);
		newObj.transform.SetParent (parent);
		newObj.transform.localPosition = pos;
		newObj.transform.localScale = new Vector3 (1,1,1);

		return newObj;
	}

}
