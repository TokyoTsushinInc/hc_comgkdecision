using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCode : MonoBehaviour{
    
	// デバッグ
	public bool debug = false;
	
	// 外部コード参照用(アタッチしてね)
	public GACode gaCode = null;
	public TenjinCode tenjinCode = null;
	public FacebookCode facebookCode = null;
	
	// 外部コード参照用(アタッチしてね)
	public FixAspectFor2DCamera fixAspectFor2DCamera = null;
	public InputCode inputCode = null;
	
	// ゲームオブジェクト(アタッチしてね)
	public GameObject objectUICamera = null;
	
	// メンバ
	public bool first = false;
	public int current_index = 0;
	public bool setting = false;
	public bool full = false;
	
	
	
    /////////////////////////////////////
    // Awake処理
    /////////////////////////////////////
	void Awake(){
		
		// タイムスケールの調整
		Time.timeScale = 1.0f;
		
		// iPhone版に必要になりそう？
		Application.targetFrameRate = 60;
		
		// VSyncをOFFにする()
		QualitySettings.vSyncCount = 0;
		
	}
    
	/////////////////////////////////////
    // Start処理
    /////////////////////////////////////
	void Start(){
		
		// メンバの初期化
		first = false;
		current_index = -1;
		setting = false;
		full = false;
		
		// ステータスの更新
		setStatus();
		
    }
	
	/////////////////////////////////
    // ステータスの設定
    /////////////////////////////////
	public void setStatus(){
		
		// ワーク変数
		float fah = 0.0f;
		float faw = 0.0f;
		Vector3 vec = Vector3.zero;
		GameObject obj = null;
		
		// ％指定用
		fah = fixAspectFor2DCamera.getCameraorthographicSizeHeight();
		fah = fah / 100.0f;
		faw = fixAspectFor2DCamera.getCameraorthographicSizeWidth();
		faw = faw / 100.0f;
		
		// messageウィンドウ
		obj = GameObject.Find("UI/anim_fukidasi").gameObject;
		vec = obj.transform.position;
		vec.y = 0 - fah * 60;
		obj.transform.position = vec;
		
		// a & bボタン
		obj = GameObject.Find("UI/anim_selectA").gameObject;
		vec = obj.transform.position;
		vec.y = 0 + fah * 70;
		obj.transform.position = vec;
		
		obj = GameObject.Find("UI/anim_selectB").gameObject;
		vec = obj.transform.position;
		vec.y = 0 + fah * 70;
		obj.transform.position = vec;
		
	}
	
	////////////////////////////////////////////////////////////
	// SDKの初期化
	////////////////////////////////////////////////////////////
	public void setupSDK(){
		
		gaCode.GetComponent<GACode>().callStart();
		facebookCode.GetComponent<FacebookCode>().callStart();
		tenjinCode.GetComponent<TenjinCode>().callStart();
		
	}
	
	////////////////////////////////////////////////////////////
	// アニメーションをセットする
	////////////////////////////////////////////////////////////
	public void setAnimation(GameObject obj,string anim_str){
		
		// ワーク変数
		Animation anim = null;
		
		anim = obj.GetComponent<Animation>();
		
		anim[anim_str].speed = 1.0f;
		anim[anim_str].time = 0.0f;
		anim.Play(anim_str);
		
	}
	
	////////////////////////////////////////////////////////////
	// アニメーションの再生を確認する
	////////////////////////////////////////////////////////////
	public bool checkIsPlayAnimation(GameObject obj,string anim_str){
		
		// ワーク変数
		Animation anim = null;
		bool ret = false;
		
		anim = obj.GetComponent<Animation>();
		
		if(true == anim.IsPlaying(anim_str)){
	    	ret = true;
		}
		
		return ret;
		
	}
	
	////////////////////////////////////////////////////////////////////
	// アニメーションの速度だけを設定する
	////////////////////////////////////////////////////////////////////
	public void setAnimationSpeed(GameObject obj,string anim_str,float anim_speed){
		
		// ワーク変数
		Animation anim = null;
		
		anim = obj.GetComponent<Animation>();
		
		anim[anim_str].speed = anim_speed;
		
	}
    
    ////////////////////////////////////////////////////////////
	// シンプルなクロスフェード
	////////////////////////////////////////////////////////////
	public void crossFade(GameObject obj,string str){
		
		Animator anim = obj.GetComponent<Animator>();
		float fadeLength = 0.3f;
		float duration = fadeLength / anim.GetCurrentAnimatorStateInfo(0).length;
		anim.CrossFade(str, duration);
		
	}
	
	////////////////////////////////////////////////////////////
	// UnityAnalyticsへカスタムイベントを送る
	////////////////////////////////////////////////////////////
	public void sendUACustomEvent_STR(string event_name,
										string data_name0,string data_param0){
		
		Debug.Log(event_name + "=" + 
		          data_name0 + ":" + data_param0);
		
		Dictionary<string, object> dict = new Dictionary<string, object> ();
		dict.Add(data_name0, data_param0);
		UnityEngine.Analytics.Analytics.CustomEvent(event_name, dict);
		
	}
	
	////////////////////////////////////////////////////////////
	// UnityAnalyticsへカスタムイベントを送る
	////////////////////////////////////////////////////////////
	public void sendUACustomEvent_INT(string event_name,
										string data_name0,int data_param0){
		
		Debug.Log(event_name + "=" + 
		          data_name0 + ":" + data_param0);
		
		Dictionary<string, object> dict = new Dictionary<string, object> ();
		dict.Add(data_name0, data_param0);
		UnityEngine.Analytics.Analytics.CustomEvent(event_name, dict);
		
	}
	
	////////////////////////////////////////////////////////////
	// UnityAnalyticsへカスタムイベントを送る
	////////////////////////////////////////////////////////////
	public void sendUACustomEvent_INT_STR(string event_name,
	                                        string data_name0,int data_param0,
										    string data_name1,string data_param1){
		
		Debug.Log(event_name + "=" + 
		          data_name0 + ":" + data_param0 + "," + 
				  data_name1 + ":" + data_param1);
		
		Dictionary<string, object> dict = new Dictionary<string, object> ();
		dict.Add(data_name0, data_param0);
		dict.Add(data_name1, data_param1);
		UnityEngine.Analytics.Analytics.CustomEvent(event_name, dict);
		
	}
	
	////////////////////////////////////////////////////////////
	// iOSのレビューをコール
	////////////////////////////////////////////////////////////
	public void reviewFromiOS(){
		
		// ワーク変数
		int c = 0;
		
		#if !UNITY_EDITOR && UNITY_IOS
			
			// チェック用
			c = 0;
			
			// メジャーバージョンが11以上であればok
			if(getiOSMajorVersion() >= 11){
				c = 1;
			}
			
			// メジャーバージョンが10以上かつ、マイナーバージョンが3以上であればok
			if(getiOSMajorVersion() >= 10 && getiOSMinorVersion() >= 3){
				c = 1;
			}
			
			if(c == 1){
				
				// 利用可能
				iOSReview.requestReview();
				
			}
			
		#endif
	
	}
	
	////////////////////////////////////////////////////////////
	// iOSのメジャーバージョンを返す
	////////////////////////////////////////////////////////////
	public int getiOSMajorVersion(){
		
		// ワーク変数
		int i = 0;
		string str = "";
		
		try{
			
			str = SystemInfo.operatingSystem;
			
			// iPhone OSを削除
			str = str.Replace("iPhone OS", "");
			
			// iOSを削除
			str = str.Replace("iOS", "");
			
			// 空白を徹底的に削除
			for(i = 0;i < 10;i++){
				str = str.Replace(" ", "");
			}
			
			// .でスプリット
			string[] sprit_str = str.Split("."[0]);
			
			// メジャーバージョンをリターンして完了
			return int.Parse(sprit_str[0]);
			
		}catch(System.Exception e){
			
			return -1;
			
		}
		
	}
	
	////////////////////////////////////////////////////////////
	// iOSのマイナーバージョンを返す
	////////////////////////////////////////////////////////////
	public int getiOSMinorVersion(){
		
		// ワーク変数
		int i = 0;
		string str = "";
		
		try{
			
			str = SystemInfo.operatingSystem;
			
			// iPhone OSを削除
			str = str.Replace("iPhone OS", "");
			
			// iOSを削除
			str = str.Replace("iOS", "");
			
			// 空白を徹底的に削除
			for(i = 0;i < 10;i++){
				str = str.Replace(" ", "");
			}
			
			// .でスプリット
			string[] sprit_str = str.Split("."[0]);
			
			// マイナーバージョンをリターンして完了
			return int.Parse(sprit_str[1]);
			
		}catch(System.Exception e){
			
			return -1;
			
		}
		
	}
	
	////////////////////////////////////////////////////////////
	// UIボタンの入力
	////////////////////////////////////////////////////////////
	public void pushedSettingButton(){
		
		GameObject.Find("Canvas/ViewArea/Settings").gameObject.SetActive(true);
		
		setting = true;
		
	}
	
	public void pushedCloseButton(){
		
		GameObject.Find("Canvas/ViewArea/Settings").gameObject.SetActive(false);
		
		setting = false;
		
	}
	
	public void pushedPP0Button(){
		
		Application.OpenURL("https://app.babangida.be/privacy/en.php");
		
	}
	
	public void pushedFullButton(){
		
		if(full){
			full = false;
		}else{
			full = true;
		}
		
	}
	
	////////////////////////////////////////////////////////////
	// UIボタンの表示設定
	////////////////////////////////////////////////////////////
	public void showSettingButton(bool flag){
		
		GameObject.Find("Canvas/ViewArea/ButtonSettings").gameObject.SetActive(flag);
		
	}
	
	public void showFullButton(bool flag){
		
		GameObject.Find("Canvas/ViewArea/ButtonFull").gameObject.SetActive(flag);
		
	}
	
	////////////////////////////////////////////////////////////
	// 入力のチェック
	////////////////////////////////////////////////////////////
	public bool checkInput(int code){
		
		// ワーク変数
		int i = 0;
		bool ret = false;
		
		for(i = 0;i < 2;i++){
			if(inputCode.input_status[i] == code){
				inputCode.input_status[i] = InputCode.INPUT_NULL;
				ret = true;
			}
		}
		
		return ret;
		
	}
	
}
