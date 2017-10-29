using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour {

	public static AdsManager instance;

	string bannerID;
	string videoID;
	string rewardVideoID;
	bool rewardBasedEventHandlerSet;

	InterstitialAd interstitial;
	RewardBasedVideoAd rewardBasedVideo;

	void Awake () {

		if (instance != null && instance != this) {
			Destroy (this.gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad (gameObject);
		}

		rewardBasedEventHandlerSet = false;

		#if UNITY_EDITOR
		print("Ads will not work on Unity editor.");
		bannerID = "editor";
		videoID = "editor";
		rewardVideoID = "editor";
		#elif UNITY_ANDROID
		bannerID = "ca-app-pub-8759797633054789/8630808228";
		videoID = "ca-app-pub-8759797633054789/2065399878";
		rewardVideoID = "ca-app-pub-8759797633054789/7126154860";
		#else
		bannerID = "ca-app-pub-8759797633054789/3342042272";
		videoID = "ca-app-pub-8759797633054789/2646895943";
		rewardVideoID = "ca-app-pub-8759797633054789/9344166231";
		#endif

	}

	void Start () {


	}

	// BANNER

	public void RequestBanner () {

		AdRequest request = new AdRequest.Builder().Build();

		BannerView bannerAd = new BannerView(bannerID, AdSize.SmartBanner, AdPosition.Bottom);
		bannerAd.LoadAd(request);

		rewardBasedVideo = RewardBasedVideoAd.Instance;
	}

	// INTERSTITIAL

	public void RequestInterstitialAds () {

		interstitial = new InterstitialAd(videoID);
		AdRequest request = new AdRequest.Builder().Build();

		interstitial.LoadAd(request);

	}

	public void ShowInterstitialAd() {

		if (InterstitialIsLoaded()) {
			interstitial.Show ();
		}

		RequestInterstitialAds ();
		GameController.instance.AddMoves ();
	}

	public bool InterstitialIsLoaded () {

		if (interstitial.IsLoaded ()) {
			return true;
		} else {
			return false;
		}
	}

	// REWARDED VIDEO
	
	public void RequestRewardBasedVideo () {

		rewardBasedVideo = RewardBasedVideoAd.Instance;

	if (!rewardBasedEventHandlerSet) {

		rewardBasedVideo.OnAdRewarded += HandleOnAdRewarded;
		rewardBasedVideo.OnAdClosed += HandleOnAdClosed;
		rewardBasedVideo.OnAdFailedToLoad += HandleOnAdClosed;

		rewardBasedEventHandlerSet = true;
	}

		AdRequest request = new AdRequest.Builder().Build();
		rewardBasedVideo.LoadAd(request, rewardVideoID);


	}

	public bool RewardedVideoIsLoaded () {

		if (rewardBasedVideo.IsLoaded ()) {
			return true;
		} else {
			return false;
		}
	}

	public void ShowRewardVideo () {
		if (RewardedVideoIsLoaded()) {
			rewardBasedVideo.Show ();
		}
	}

	public void HandleOnAdRewarded (object sender, Reward args) {
		MainSettings.instance.usedHelp = true;
	}

	public void HandleOnAdClosed (object sender, System.EventArgs args) {
		Scene game = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (game.buildIndex);
	}

	public void HandleOnAdFailedToLoad (object sender, AdFailedToLoadEventArgs args) {
		RequestRewardBasedVideo ();
	}

}