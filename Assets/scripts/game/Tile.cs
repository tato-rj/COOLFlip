using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour {

	public GameObject[] neighbours;
	public bool up;
	Color32 frontColor;
	Color32 backColor;

	void Awake () {

		frontColor = GameController.instance.frontColor;
		backColor = GameController.instance.grey;

		GetComponent<Image>().color = (up) ? frontColor : backColor;

	}

	public void FlipNeighbours() {

		if (GameController.instance.gameOn) {
			
			if (GameController.instance.gridIndex == 0 && name != GameController.instance.handPos) {
				GameController.instance.tutorial.SetActive (false);
			}
			// Animate tapped tile
			GetComponent<Animation> ().Play ("touchedTile");
			// If there is an X inside tile, remove it on touch
			if (transform.childCount > 0) {
				transform.GetChild (0).gameObject.SetActive (false);
			}

			foreach (GameObject tile in neighbours) {
				//Switch color
				Image neighbourTile = tile.GetComponent<Image> ();
				neighbourTile.color = (neighbourTile.color == frontColor) ? backColor : frontColor ;

				//Animate
				tile.GetComponent<Animation>()["flipTile"].speed = Random.Range(0.8f,1.4f);
				tile.GetComponent<Animation> ().Play ("flipTile");
				GameController.instance.flipTile.Play ();

			}

			GameController.instance.UpdateTapCounter ();			
		}

	}
		
}
