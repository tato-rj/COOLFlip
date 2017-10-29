using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorModeIcon : MonoBehaviour {

	public Sprite dayTime;
	public Sprite nightTime;

	void Start () {

		if (!MainSettings.instance.isDayTime) {
			GetComponent<Image> ().sprite = dayTime;
		} else {
			GetComponent<Image> ().sprite = nightTime;
		}

	}
	
}
