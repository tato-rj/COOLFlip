using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctions : MonoBehaviour {

	public static UIFunctions instance;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad (this);
	}

	public void RateApp () {

		string link;

		#if UNITY_EDITOR
		link = "https://www.leftlaneapps.com";
		#elif UNITY_ANDROID
		link = "https://www.leftlaneapps.com";
		#elif UNITY_IOS
		link = "https://www.leftlaneapps.com";
		#else
		link = "https://www.leftlaneapps.com";
		#endif

		Application.OpenURL (link);
	}

	public void SendEmail () {
		Application.OpenURL ("mailto:contact@leftlaneapps.com?subject=CoolFLIP%20Feedback");
	}

}
