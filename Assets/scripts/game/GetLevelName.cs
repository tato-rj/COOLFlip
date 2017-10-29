using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetLevelName : MonoBehaviour {

	void Start () {

		int gridIndex = GameController.instance.gridIndex;
		gridIndex += 2;
		int levelIndex = GameController.instance.levelIndex;
		levelIndex += 1;

		if (gridIndex == 2) {
			GetComponent<Text>().text = "Tutorial";
		} else {
			GetComponent<Text>().text = gridIndex + "x" + gridIndex + " Level " + levelIndex;	
		}

	}

}
