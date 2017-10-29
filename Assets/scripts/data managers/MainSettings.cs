using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSettings : MonoBehaviour {

	public static MainSettings instance;

	public bool isDayTime;
	public int[] solvedLevels = new int[4];
	public int[,] numberOfMoves = new int[4, 25];

	public Material lightFontMaterial;
	public Material darkFontMaterial;
	public Material bgMaterial;

	public Color32[] lightFontColors;
	public Color32[] darkFontColors;
	public Color32[] bgColors;
	public Image logo;
	public Sprite[] logoSprites;

	public bool usedHelp;

	// Arrays that contain the minimum number of moves for each level
	public int[,] records;

	int index;

	void Awake () {
		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}

		DontDestroyOnLoad (this);

		isDayTime = UserData.instance.LoadColorMode ();
		solvedLevels = UserData.instance.LoadSolvedLevels ();
		numberOfMoves = UserData.instance.LoadMoves ();

		usedHelp = false;

		// Records: minimum number of moves for each level
		records = new int[,] {
			// Tutorial (empty because the array needs to have equal amount of numbers
			{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
			// 3x3
			{2,2,2,2,3,3,3,3,3,3,3,3,3,3,3,0,0,0,0,0,0,0,0,0,0},
			// 4x4
			{2,2,2,2,3,3,3,3,3,3,3,3,4,4,4,4,5,5,5,5,0,0,0,0,0},
			// 5x5
			{3,3,3,3,3,3,4,4,4,4,4,4,4,4,4,5,5,5,5,5,5,5,5,5,5}
		};
			
	}

	void Start () {

		SetColors ();

	}

	public void SetColors () {
		
		index = (isDayTime) ? 0 : 1;

		bgMaterial.color = bgColors [index];
		lightFontMaterial.color = lightFontColors [index];
		darkFontMaterial.color = darkFontColors [index];
		// If on the main screen, update the logo
		if (SceneManager.GetActiveScene ().ToString() == "intro") {
			logo = GameObject.Find ("Logo").GetComponent<Image>();
			logo.sprite = logoSprites [index];	
		}

	}
	
}
