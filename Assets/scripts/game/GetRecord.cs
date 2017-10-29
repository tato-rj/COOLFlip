using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetRecord : MonoBehaviour {

	public Text record;
	int num;

	void Awake () {

		num = 0;

		foreach (Transform child in transform) {
			if (child.name.Contains("+")) {
				num++;
			}
		}

		//record.text = num.ToString ();

	}
}
