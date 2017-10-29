using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static GameController instance;

	/************************
	 * 
	 *	SCREEN CONTAINERS
	 * 
	 ************************/

	// Holds settings button, moves counter and current level
	public GameObject header;
	// To fade out screen when player solves the puzzle
	public GameObject mainOverlay;
	// Screen to show when player runs out of moves
	public GameObject outOfMovesScreen;

	/************************
	 *
	 *	GRIDS AND LEVELS
	 *
	 ************************/

	//Contains all levels, organized by their containers: 3x3, 4x4 and 5x5
	public Transform[] gridContainers;
	// Tutorial grid 2x2 has only one level
	public GameObject tutorial;
	// Holds the position of the hand on the tutorial level
	public string handPos;
	// Will contain the grid currently on
	public Transform selectedGrid;
	// Determines how fast the tiles will animate at the beginning
	private float delay = 0.1f;

	/************************
	 * 
	 *	SOUNDS
	 * 
	 ************************/

	public AudioSource flipTile;
	public AudioSource fail;
	public AudioSource positive;

	/************************
	 * 
	 *	MOVES COUNTER
	 * 
	 ************************/

	public Text tapsCounter;
	public int numOfMoves;
	public int movesCount;
	public Text minMovesNum;

	/************************
	 * 
	 *	COLORS
	 * 
	 ************************/

	// Holds the colors of each grid: 2x2 is organe, 3x3 is blue, 4x4 is green and 5x5 is red
	public Material[] colors;
	// Color of the front of the tile
	public Color32 frontColor;
	// Color of the back of the tile
	public Color32 grey;

	/************************
	 * 
	 *	DATA
	 * 
	 ************************/

	// Indexes used to save and load data in form of arrays
	public int gridIndex;
	public int levelIndex;


	// Controls the flow of the game
	public bool gameOn;

	void Awake () {

		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}

		// Will count how many moves player took to finish a level and save it to the player's records
		movesCount = 0;

		// Get sounds
		flipTile = GetComponents<AudioSource> () [0];
		fail = GetComponents<AudioSource> () [1];
		positive = GetComponents<AudioSource> () [2];

		// Loads data
		gridIndex = UserData.instance.LoadGrid ();
		levelIndex = UserData.instance.LoadLevel ();
		// Calculates the number of moves based on the number of tiles: more tiles, more moves
		numOfMoves = (gridIndex + 2) * 5 ;

		// Adjust speed depending on the number of tiles: the more tiles, the faster they will animate
		delay -= (float)(gridIndex * 2) / 150;

		gameOn = false;

	}

	void Start (){

		AdsManager.instance.RequestRewardBasedVideo ();
		AdsManager.instance.RequestInterstitialAds ();

		SetupLevel (gridContainers [gridIndex]);

		StartCoroutine (LayoutTiles(selectedGrid));

		if (gridIndex == 0) {
			// Display tutorial animations
			StartCoroutine (ShowTutorial());
		}

	}

	public IEnumerator ShowTutorial () {
	
		yield return new WaitForSeconds (1.5f);
		if (movesCount == 0) {
			tutorial.SetActive (true);
			handPos = "21+";	
		}

	}

	private void UpdateTutorial () {
		tutorial.transform.GetChild (0).GetComponent<RectTransform>().localPosition = new Vector3(50f,-8f,0);
		tutorial.transform.GetChild (1).GetComponent<Text> ().text = "Your goal is to match all tiles!";
		handPos = "12+";
	}

	private void SetupLevel (Transform container) {

		tapsCounter.text = numOfMoves.ToString ();
		frontColor = colors[gridIndex].color;
		// Update the color of the button to redeem more moves
		outOfMovesScreen.transform.GetChild(2).GetComponent<Image>().color = colors [gridIndex].color;
		selectedGrid = container.GetChild (levelIndex);
		selectedGrid.gameObject.SetActive(true);
	
	}

	public void AddMoves () {
		outOfMovesScreen.SetActive (false);
		numOfMoves = (gridIndex + 2) * 2;
		tapsCounter.text = numOfMoves.ToString();
		gameOn = true;
	}

	public IEnumerator LayoutTiles (Transform container) {

		yield return new WaitForSeconds (0.1f);	

		foreach (Transform tile in container) {
			tile.gameObject.SetActive (true);
			yield return new WaitForSeconds (delay);	
		}

		header.SetActive (true);

		if (gridIndex != 0 ) {
			minMovesNum.text = MainSettings.instance.records[gridIndex,levelIndex].ToString ();
			minMovesNum.transform.parent.gameObject.SetActive (true);	
		}

		gameOn = true;

	}

	public void UpdateTapCounter () {

		if (gridIndex == 0) {
			UpdateTutorial();
		}

		movesCount++;
		numOfMoves--;
		tapsCounter.text = numOfMoves.ToString ();

		if (AllTilesUp(selectedGrid)) {
			StartCoroutine (PuzzleSolved());
		} else if (numOfMoves == 0) {
			gameOn = false;
			fail.Play ();
			outOfMovesScreen.SetActive (true);
			outOfMovesScreen.transform.GetChild (0).gameObject.SetActive (true);

			outOfMovesScreen.transform.GetChild (1).GetComponent<Text> ().text = (AdsManager.instance.InterstitialIsLoaded ()) ? "You're out of moves!" : "We couldn't load any ads... but you'll get more moves anyway :)";

		}

	}

	public IEnumerator PuzzleSolved () {

		int numOfLevels = (gridIndex + 2) * 5;
		int levelNum = levelIndex + 1;

		gameOn = false;
		positive.Play ();

		if (levelIndex == MainSettings.instance.solvedLevels[gridIndex]) {
			MainSettings.instance.solvedLevels [gridIndex]++;	
		}
			
		MainSettings.instance.numberOfMoves [gridIndex,levelIndex] = movesCount;

		// If level was the tutorial
		if (gridIndex == 0) {
			UserData.instance.SaveLevel (1, 0);
		} else {


			if (levelNum < numOfLevels) {
				UserData.instance.SaveLevel (gridIndex, levelNum);
			} else {
				UserData.instance.SaveLevel (gridIndex+1, 0);
			}
		}

		UserData.instance.SaveSolvedLevels (MainSettings.instance.solvedLevels, MainSettings.instance.numberOfMoves);

		yield return new WaitForSeconds (0.2f);

		mainOverlay.SetActive (true);
		selectedGrid.GetComponent<Animation> ().Play ();

		yield return new WaitForSeconds (1f);

		Scene game = SceneManager.GetActiveScene ();

		// If this was not the last puzzle, go to the next
		if (levelNum < numOfLevels) {
			SceneManager.LoadScene (game.buildIndex);
			// If this IS the last puzzle, show credits screen
		} else {
			SceneManager.LoadScene ("end");
		}

	}

	private bool AllTilesUp (Transform container) {

		Image[] colorsArray = container.GetComponentsInChildren<Image> ();
		bool allUp = true;

		for (int i = 0; i < colorsArray.Length; i++) {
			if (colorsArray[i].color != frontColor) {
				allUp = false;
			}
		}

		return allUp;

	}

}
