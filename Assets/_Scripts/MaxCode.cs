using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MaxCode : MonoBehaviour {
	
	// ios
    //string bannerAdUnitId       = "xxxxxxxxxxxxxxxx";       // バナー用
    //string interstitialAdUnitId = "xxxxxxxxxxxxxxxx";       // インステ用
    //string rewardedAdUnitId     = "xxxxxxxxxxxxxxxx";       // リワード用
	//string mrecAdUnitId 		= "xxxxxxxxxxxxxxxx";		// レクタングル(MRec)用
	
	// android
	string bannerAdUnitId       = "xxxxxxxxxxxxxxxx";       // バナー用
    string interstitialAdUnitId = "xxxxxxxxxxxxxxxx";       // インステ用
    string rewardedAdUnitId     = "xxxxxxxxxxxxxxxx";       // リワード用
	string mrecAdUnitId 		= "xxxxxxxxxxxxxxxx";		// レクタングル(MRec)用
	
	
	
	// メンバ
	public bool reward_dismissed = false;
	public bool interstitial_dismissed = false;
	
  	/////////////////////////////////////
    // 初期化処理
    /////////////////////////////////////
  	void Start(){
		
		
		// メンバの初期化
		reward_dismissed = false;
		interstitial_dismissed = false;
		
		// コールバックの設定
		MaxSdkCallbacks.OnSdkInitializedEvent += SdkInitializedEvent;
		// (MaxSdkBase.SdkConfiguration sdkConfiguration)
		
		// 初期化処理の実行
		MaxSdk.SetSdkKey("FTca20TycVH8BArkyW-ewbhy6dPiD2USa51d5aZi4ssG1g-zCyc7TJ3eySM9Vsb4ydi2CHlH7aX9GW6TTka01e");
		MaxSdk.InitializeSdk();
		
		// 初期化完了のコールバック
		MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
			
			// ios
			
			//#if UNITY_IOS || UNITY_IPHONE || UNITY_EDITOR
			//	
			//	if (MaxSdkUtils.CompareVersions(UnityEngine.iOS.Device.systemVersion, "14.5") != MaxSdkUtils.VersionComparisonResult.Lesser)
			//	{
			//		// Note that App transparency tracking authorization can be checked via `sdkConfiguration.AppTrackingStatus` for Unity Editor and iOS targets
			//		// 1. Set Facebook ATE flag here, THEN
			//		
			//		// Facebook AudienceNetworkのトラッキング設定
            //        AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(sdkConfiguration.AppTrackingStatus == MaxSdkBase.AppTrackingStatus.Authorized);
            //    	
			//	}	
			//#endif
			
			
			initThisSDK();
			
			// シーンをTTConsentへ
			SceneManager.LoadScene("TTConsent");
			
		};
		
  	}

    void SdkInitializedEvent(MaxSdkBase.SdkConfiguration sdkConfiguration){

        Debug.Log("###:Max:SdkInitializedEvent");
		
    }
	
	void initThisSDK(){
		
		Debug.Log("###:Max:initThisSDK");
		
        if(SystemInfo.deviceModel.Contains ("iPad")) {
			
			// バナー無視する場合はコメントアウトしてね。
			InitializeBannerAds();
			
        }else{
            
            // バナーの初期設定
            InitializeBannerAds();

        }

        // インステの初期設定
        InitializeInterstitialAds();

        // リワードの初期化
        InitializeRewardedAds();
		
		// レクタングル(MRec)の初期化
		InitializeMRecAds();
		
	}
	
	/////////////////////////////////////
    // レクタングル(MRec)関係
    /////////////////////////////////////
	private void InitializeMRecAds(){
		
		Debug.Log("###:Max:InitializeMRecAds");
		
		// MRECs are sized to 300x250 on phones and tablets
		MaxSdk.CreateMRec(mrecAdUnitId, MaxSdkBase.AdViewPosition.Centered);
		
	}
	
	private void mrec_change_center(){
		
		Debug.Log("###:Max:mrec_change_center");
		
		MaxSdk.UpdateMRecPosition(mrecAdUnitId, MaxSdkBase.AdViewPosition.Centered);
		
	}
	
	private void mrec_change_bottom(){
		
		Debug.Log("###:Max:mrec_change_bottom");
		
		MaxSdk.UpdateMRecPosition(mrecAdUnitId, MaxSdkBase.AdViewPosition.BottomCenter);
		
	}
	
	private void mrec_show(){

      Debug.Log("###:Max:mrec_show");

      MaxSdk.ShowMRec(mrecAdUnitId);

    }

    private void mrec_hide(){

      Debug.Log("###:Max:mrec_hide");

      MaxSdk.HideMRec(mrecAdUnitId);
  
    }
	
    /////////////////////////////////////
    // バナー関係
    /////////////////////////////////////
    private void InitializeBannerAds()
    {

        Debug.Log("###:Max:InitializeBannerAds");

        // Banners are automatically sized to 320x50 on phones and 728x90 on tablets
        // You may use the utility method `MaxSdkUtils.isTablet()` to help with view sizing adjustments
        MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

        // Set background or background color for banners to be fully functional
        MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, new Color(0.0f, 0.0f, 0.0f, 1.0f));

    }

    public void banner_show(){

      Debug.Log("###:Max:banner_show");

      MaxSdk.ShowBanner(bannerAdUnitId);

    }

    public void banner_hide(){

      Debug.Log("###:Max:banner_hide");

      MaxSdk.HideBanner(bannerAdUnitId);
  
    }



    /////////////////////////////////////
    // インステ関係
    /////////////////////////////////////
    private void InitializeInterstitialAds()
    {

        Debug.Log("###:Max:InitializeInterstitialAds");
    
        // Attach callback
        MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;

        // Load the first interstitial
        LoadInterstitial();
    }

    private void LoadInterstitial()
    {

        MaxSdk.LoadInterstitial( interstitialAdUnitId );

    }

    private void OnInterstitialLoadedEvent(string adUnitId)
    {
        // Interstitial ad is ready to be shown. MaxSdk.IsInterstitialReady(interstitialAdUnitId) will now return 'true'
    }

    private void OnInterstitialFailedEvent(string adUnitId, int errorCode)
    {
        // Interstitial ad failed to load. We recommend re-trying in 3 seconds.
        Invoke("LoadInterstitial", 3);
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
    {

        // Interstitial ad failed to display. We recommend loading the next ad
        LoadInterstitial();
    }

    private void OnInterstitialDismissedEvent(string adUnitId)
    {

        Debug.Log("###:Max:interstitial_dismissed_event:" + adUnitId);

        // 処理の呼び出し
        setInterStitialDismissed(true);

        // Interstitial ad is hidden. Pre-load the next ad
        LoadInterstitial();
    }
	
	public bool interstitial_check(){

        Debug.Log("###:Max:interstitial_check");

        if ( MaxSdk.IsInterstitialReady(interstitialAdUnitId) ){
			
			return true;
			
        }else{
			
			return false;
			
        }

    }

    public void interstitial_show(){

        Debug.Log("###:Max:interstitial_show");

      if ( MaxSdk.IsInterstitialReady(interstitialAdUnitId) )
      {

        // 処理の呼び出し
		setInterStitialDismissed(false);

        MaxSdk.ShowInterstitial(interstitialAdUnitId);
      }
  
    }
	
	
	public void setInterStitialDismissed(bool flag){
		
		interstitial_dismissed = flag;
		
	}
	
	public bool checkInterStitialDismissed(){
		
		return interstitial_dismissed;
		
	}
	
	
	
    /////////////////////////////////////
    // リワード関係
    /////////////////////////////////////
    private void InitializeRewardedAds()
    {

        Debug.Log("###:Max:InitializeRewardedAds");

        // Attach callback
        MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

        // Load the first RewardedAd
        LoadRewardedAd();

    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd( rewardedAdUnitId );
    }

    private void OnRewardedAdLoadedEvent(string adUnitId)
    {
        // Rewarded ad is ready to be shown. MaxSdk.IsRewardedAdReady(rewardedAdUnitId) will now return 'true'
    }

    private void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
    {
        // Rewarded ad failed to load. We recommend re-trying in 3 seconds.
        Invoke("LoadRewardedAd", 3);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
    {
        // Rewarded ad failed to display. We recommend loading the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId) {}

    private void OnRewardedAdClickedEvent(string adUnitId) {}

    private void OnRewardedAdDismissedEvent(string adUnitId)
    {

        Debug.Log("###:Max:reward_dismissed_event:" + adUnitId);

        // 処理の呼び出し
		setRewardDismissed(true);
        
        // Rewarded ad is hidden. Pre-load the next ad
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
    {
        // Rewarded ad was displayed and user should receive the reward
    }

    public bool reward_check(){

        Debug.Log("###:Max:reward_check");

        if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId)) {

            return true;

        }else{

            return false;

        }

    }

    public void reward_show(){

        Debug.Log("###:Max:reward_show");

        if (MaxSdk.IsRewardedAdReady(rewardedAdUnitId))
        {

            // 処理の呼び出し
			setRewardDismissed(false);
			
            MaxSdk.ShowRewardedAd(rewardedAdUnitId);
        }

    }
	
	public void setRewardDismissed(bool flag){
		
		reward_dismissed = flag;
		
	}
	
	public bool checkRewardDismissed(){
		
		return reward_dismissed;
		
	}
	
	/////////////////////////////////////
    // デバッガー表示
    /////////////////////////////////////
	public void showDebugger(){
		
		// デバッガー
		MaxSdk.ShowMediationDebugger();
		
	}
	
}
