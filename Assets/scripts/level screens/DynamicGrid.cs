using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicGrid : MonoBehaviour {


	public int size;

	void Start () {

		RectTransform parent = GetComponent<RectTransform> ();
		GridLayoutGroup grid = GetComponent<GridLayoutGroup> ();

		grid.cellSize = new Vector2 (parent.rect.width/size, parent.rect.height/size);

	}
	
}
