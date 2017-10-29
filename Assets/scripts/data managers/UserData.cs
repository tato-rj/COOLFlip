using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.SceneManagement;

public class UserData : MonoBehaviour {

	public static UserData instance;

	public bool resetData;

	void Awake () {

		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
		}

		if (resetData) {
			ResetCurrentLevel ();
			ResetSolvedLevels ();
			ResetRatings ();
			File.Delete (Application.persistentDataPath + "/daytimeSettings.dat");
		}
	}

	void Start () {
		SceneManager.LoadScene ("intro");
		AdsManager.instance.RequestBanner ();
	}

	// LEVEL SETTINGS

	public void SaveLevel (int gridIndex, int levelIndex) {

		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/levelSettings.dat");
		CurrentLevels data = new CurrentLevels ();

		data.levelIndex = levelIndex;
		data.gridIndex = gridIndex;

		binaryFormatter.Serialize (file, data);
		file.Close ();
	}

	public int LoadGrid () {

		if (File.Exists (Application.persistentDataPath + "/levelSettings.dat")) {

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/levelSettings.dat", FileMode.Open);
			CurrentLevels data = (CurrentLevels)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.gridIndex;

		} else {
			return 0;
		}
	}

	public void SaveSolvedLevels (int[] levels, int[,] moves) {

		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/solvedLevelSettings.dat");
		SolvedLevels data = new SolvedLevels ();

		data.solvedLevels = levels;
		data.numberOfMoves = moves;

		binaryFormatter.Serialize (file, data);
		file.Close ();
	}
		
	public int LoadLevel () {

		if (File.Exists (Application.persistentDataPath + "/levelSettings.dat")) {

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/levelSettings.dat", FileMode.Open);
			CurrentLevels data = (CurrentLevels)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.levelIndex;

		} else {
			return 0;
		}
	}

	public int[] LoadSolvedLevels () {

		if (File.Exists (Application.persistentDataPath + "/solvedLevelSettings.dat")) {

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/solvedLevelSettings.dat", FileMode.Open);
			SolvedLevels data = (SolvedLevels)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.solvedLevels;

		} else {
			return new int[4] {0,0,0,0};
		}
	}

	public int[,] LoadMoves () {

		if (File.Exists (Application.persistentDataPath + "/solvedLevelSettings.dat")) {

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/solvedLevelSettings.dat", FileMode.Open);
			SolvedLevels data = (SolvedLevels)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.numberOfMoves;

		} else {
			return new int[4,25] {
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
				{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
		}
	}

	public void ResetCurrentLevel () {
		File.Delete (Application.persistentDataPath + "/levelSettings.dat");
	}

	public void ResetSolvedLevels () {
		File.Delete (Application.persistentDataPath + "/solvedLevelSettings.dat");
	}

	// DAY TIME SETTINGS

	public void SaveColorMode (bool mode) {

		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/daytimeSettings.dat");
		DayTimeSettings data = new DayTimeSettings ();

		data.isDayTime = mode;

		binaryFormatter.Serialize (file, data);
		file.Close ();
	}

	public bool LoadColorMode () {

		if (File.Exists (Application.persistentDataPath + "/daytimeSettings.dat")) {

			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/daytimeSettings.dat", FileMode.Open);
			DayTimeSettings data = (DayTimeSettings)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.isDayTime;

		} else {
			return true;
		}
	}
		
	// RATINGS

	public int LoadRating () {
		//Check if data file for feedback request exists...
		if (File.Exists (Application.persistentDataPath + "/feedbackRecord.dat")) {
			//If it does, open it and returns its content (true or false)
			BinaryFormatter binaryFormatter = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/feedbackRecord.dat", FileMode.Open);
			Rating data = (Rating)binaryFormatter.Deserialize (file);
			file.Close ();

			return data.userRating;

		} else {
			return -1;
		}
	}

	public void SaveRating (int rating) {
		BinaryFormatter binaryFormatter = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/feedbackRecord.dat");
		Rating data = new Rating ();

		data.userRating = rating;

		binaryFormatter.Serialize (file, data);
		file.Close ();
	}

	public void ResetRatings () {
		File.Delete (Application.persistentDataPath + "/feedbackRecord.dat");
	}
}

[Serializable]
class CurrentLevels {

	public int gridIndex;
	public int levelIndex;

}

[Serializable]
class SolvedLevels {

	public int[] solvedLevels = new int[4];
	public int[,] numberOfMoves = new int[4,25];

}

[Serializable]
class DayTimeSettings {

	public bool isDayTime;

}

[Serializable]
class Rating {

	public int userRating;

}