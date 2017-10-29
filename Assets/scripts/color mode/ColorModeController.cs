using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorModeController : MonoBehaviour {

	public static ColorModeController instance;

	//Background colors
	public Color32[] background;
	public Camera cam;

	//Overlay colors
	public Color32[] overlayColors;
	public GameObject[] overlays;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}
	}

	void Start () {

		if (MainSettings.instance.isDayTime) {

			cam.GetComponent<Camera> ().backgroundColor = background [0];
			foreach (GameObject child in overlays) {
				child.GetComponent<Image> ().color = overlayColors [0];
			}

		} else {

			cam.GetComponent<Camera> ().backgroundColor = background [1];
			foreach (GameObject child in overlays) {
				child.GetComponent<Image> ().color = overlayColors [1];
			}

		}

	}

}
