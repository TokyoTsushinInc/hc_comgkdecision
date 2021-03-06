using UnityEngine;
using System.Collections;

public class InputCode : MonoBehaviour {
	
	// ファイナル定数
	public const int MODE_SINGLE = 0;
	public const int MODE_DOUBLE = 1000;
	
	public const int INPUT_NULL = 0;
	public const int INPUT_DOWN = 1000;
	public const int INPUT_DOWNING = 2000;
	public const int INPUT_UP = 3000;
	
	public const float SCALE_ZOOM = 0.8f;
	public const float SCALE_OFF = 0.75f;
	
	// タッチアップで認識する系のボタン
	public const int INPUT_BUTTON_SELECT_A = 			10000000;
	public const int INPUT_BUTTON_SELECT_B = 			10001000;
	
	// セル用ボタン
	public const int INPUT_BUTTON_CELL_ = 				40000000;
	public const int INPUT_BUTTON_CELL_MAX = 			64;
	
	// カメラオブジェクト用(アタッチしてね)
	public Camera camera = null;
	
	// レイキャスト用
	[HideInInspector]public Vector2 tapPoint2D = Vector2.zero;
	[HideInInspector]public Collider2D collition2D = null;
	[HideInInspector]public RaycastHit2D hit2D;
	[HideInInspector]public GameObject hitObject2D = null;
	
	// レイキャストで選択したゲームオブジェクトを入れておく
	[HideInInspector]public GameObject selectedGameObject = null; 
	
	// メンバ
	[HideInInspector]public int mode = 0;
	
	[HideInInspector]public int[] input_status = new int[2];
	[HideInInspector]public int[] input_down_status = new int[2];
	[HideInInspector]public float[] input_time = new float[2];
	
	[HideInInspector]public int[] input_down_x = new int[2];
	[HideInInspector]public int[] input_down_y = new int[2];
	[HideInInspector]public int[] input_now_x = new int[2];
	[HideInInspector]public int[] input_now_y = new int[2];
	[HideInInspector]public int[] input_up_x = new int[2];
	[HideInInspector]public int[] input_up_y = new int[2];
	
	[HideInInspector]public float[] input_world_down_x = new float[2];
	[HideInInspector]public float[] input_world_down_y = new float[2];
	[HideInInspector]public float[] input_world_now_x = new float[2];
	[HideInInspector]public float[] input_world_now_y = new float[2];
	[HideInInspector]public float[] input_world_up_x = new float[2];
	[HideInInspector]public float[] input_world_up_y = new float[2];
	
	[HideInInspector]public int input_type = 0;
	
	////////////////////////////////////////////////////////////
	// 初期化処理
	////////////////////////////////////////////////////////////
	void Start(){
		
		#if UNITY_IOS
			input_type = 1;
			//Debug.Log("run:UNITY_IOS");
	 	#endif
	 	
	 	#if UNITY_ANDROID
			input_type = 1;
			//Debug.Log("run:UNITY_ANDROID");
	 	#endif
	  	
	  	#if UNITY_EDITOR
	  		input_type = 0;
	    	//Debug.Log("run:UNITY_EDITOR");
	  	#endif
		
		// デフォルトの入力モード
		setMode(MODE_SINGLE);
	
	}
	
	////////////////////////////////////////////////////////////
	// フレーム処理
	////////////////////////////////////////////////////////////
	void Update(){
		
		// ワーク変数
		int i = 0;
		int touch_count = 0;
		
		// 実機の場合の入力/////////////////////////////////////////
		if(input_type == 1){
			
			// マルチタップのカウントを取得する
			touch_count = Input.touchCount;
			if(touch_count > 1){// 特にマルチタップは使わないので1が最高とする
				touch_count = 1;
			}
			
			// 入力処理/////////////////////////////////
			for(i = 0;i < touch_count;i++){
				
				if(Input.GetTouch(i).phase == TouchPhase.Began){
					
					//Debug.Log("MouseButtonDown...");
					
					input_status[i] = INPUT_DOWN;
					input_time[i] = Time.realtimeSinceStartup * 1.0f;
					
					// ワールド座標として変換
					tapPoint2D = camera.ScreenToWorldPoint(Input.GetTouch(i).position);
					
					// コリダー取得
					collition2D = Physics2D.OverlapPoint(tapPoint2D);
					
  	  				input_down_x[i] = (int)Input.GetTouch(i).position.x;
					input_down_y[i] = (int)Input.GetTouch(i).position.y;
					input_now_x[i] = (int)Input.GetTouch(i).position.x;
					input_now_y[i] = (int)Input.GetTouch(i).position.y;
  	  				input_world_down_x[i] = tapPoint2D.x;
  	  				input_world_down_y[i] = tapPoint2D.y;
  	  				input_world_now_x[i] = tapPoint2D.x;
  	  				input_world_now_y[i] = tapPoint2D.y;
					
					if(collition2D){
        				hit2D = Physics2D.Raycast(tapPoint2D,-Vector2.up);
        				if(hit2D){
        					
        					if(input_status[i] == INPUT_DOWN){
            					//hit2D.collider.gameObject.transform.Find("on").gameObject.SetActive(true);
        						//hit2D.collider.gameObject.transform.Find("off").gameObject.SetActive(false);
            					//hit2D.collider.gameObject.transform.localScale = new Vector2(SCALE_ZOOM,SCALE_ZOOM);
            					hit2D.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.75f,0.75f,0.75f,1.0f);
            					hitObject2D = hit2D.collider.gameObject;
            					input_status[i] = INPUT_NULL;
            				}
            				
            			}
					}
				
				}else if(Input.GetTouch(i).phase == TouchPhase.Moved){
					
					//Debug.Log("MouseButton...");
					
					input_status[i] = INPUT_DOWNING;
					
					// ワールド座標として変換
					tapPoint2D = camera.ScreenToWorldPoint(Input.GetTouch(i).position);
					
					// コリダー取得
					collition2D = Physics2D.OverlapPoint(tapPoint2D);
					
  	  				input_now_x[i] = (int)Input.GetTouch(i).position.x;
					input_now_y[i] = (int)Input.GetTouch(i).position.y;
  	  				input_world_now_x[i] = tapPoint2D.x;
  	  				input_world_now_y[i] = tapPoint2D.y;
  	  				
					if(collition2D){
        				hit2D = Physics2D.Raycast(tapPoint2D,-Vector2.up);
        				if(hit2D){
        					
        					input_status[i] = INPUT_NULL;
            				
						}
        				
					}else{
						
						if(hitObject2D != null){
							
							//hitObject2D.transform.Find("on").gameObject.SetActive(false);
        					//hitObject2D.transform.Find("off").gameObject.SetActive(true);
							//hitObject2D.transform.localScale = new Vector2(SCALE_OFF,SCALE_OFF);
							hitObject2D.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
							hitObject2D = null;
							
						}
						
					}
					
				}else if(Input.GetTouch(i).phase == TouchPhase.Ended){
					
					//Debug.Log("MouseButtonUp...");
					
					input_status[i] = INPUT_UP;
					input_time[i] = (Time.realtimeSinceStartup * 1.0f) - input_time[i];
					
					// ワールド座標として変換
					tapPoint2D = camera.ScreenToWorldPoint(Input.GetTouch(i).position);
					
					// コリダー取得
					collition2D = Physics2D.OverlapPoint(tapPoint2D);
					
  	  				input_up_x[i] = (int)Input.GetTouch(i).position.x;
					input_up_y[i] = (int)Input.GetTouch(i).position.y;
  	  				input_world_up_x[i] = tapPoint2D.x;
  	  				input_world_up_y[i] = tapPoint2D.y;
  	  				
  	  				if(collition2D){
        				hit2D = Physics2D.Raycast(tapPoint2D,-Vector2.up);
        				if (hit2D) {
        					
            				//Debug.Log("ray hit object is:" + hit2D.collider.gameObject.name);
            				
            				if(hitObject2D != null){
            					
            					// タッチアップを確認
            					checkInputUp(i);
            					
            					//hitObject2D.transform.Find("on").gameObject.SetActive(false);
        						//hitObject2D.transform.Find("off").gameObject.SetActive(true);
            					//hitObject2D.transform.localScale = new Vector2(SCALE_OFF,SCALE_OFF);
            					hitObject2D.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
								hitObject2D = null;
								
            				}
            				
        				}
        				
    				}else{
						
						if(hitObject2D != null){
							
							//hitObject2D.transform.Find("on").gameObject.SetActive(false);
        					//hitObject2D.transform.Find("off").gameObject.SetActive(true);
							//hitObject2D.transform.localScale = new Vector2(SCALE_OFF,SCALE_OFF);
							hitObject2D.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
							hitObject2D = null;
							
						}
						
					}
					
				}else{
					
					//input_status[i] = INPUT_NULL;
					
				}
			
			}
			
			// ダブルタッチで同じ入力が設定されたら片方を消す
			if(mode == MODE_SINGLE){
				if(input_status[0] == input_status[1]){
					input_status[1] = INPUT_NULL;
				}
			}
			
		}
		
		
		
		
		
		// エディタの場合の入力/////////////////////////////////////////
		if(input_type == 0){
			
			// 入力処理/////////////////////////////////
			for(i = 0;i < 1;i++){// マウスでダブルタップはありえないので1としておく。
				
				if(Input.GetMouseButtonDown(i)){
					
					//Debug.Log("MouseButtonDown...");
					
					input_status[i] = INPUT_DOWN;
					input_time[i] = Time.realtimeSinceStartup * 1.0f;
					
					// ワールド座標として変換
					tapPoint2D = camera.ScreenToWorldPoint(Input.mousePosition);
					
					// コリダー取得
					collition2D = Physics2D.OverlapPoint(tapPoint2D);
					
  	  				input_down_x[i] = (int)Input.mousePosition.x;
					input_down_y[i] = (int)Input.mousePosition.y;
					input_now_x[i] = (int)Input.mousePosition.x;
					input_now_y[i] = (int)Input.mousePosition.y;
  	  				input_world_down_x[i] = tapPoint2D.x;
  	  				input_world_down_y[i] = tapPoint2D.y;
  	  				input_world_now_x[i] = tapPoint2D.x;
  	  				input_world_now_y[i] = tapPoint2D.y;
  	  				
  	  				if(collition2D){
        				hit2D = Physics2D.Raycast(tapPoint2D,-Vector2.up);
        				if(hit2D){
        					
        					if(input_status[i] == INPUT_DOWN){
            					//hit2D.collider.gameObject.transform.Find("on").gameObject.SetActive(true);
        						//hit2D.collider.gameObject.transform.Find("off").gameObject.SetActive(false);
            					//hit2D.collider.gameObject.transform.localScale = new Vector2(SCALE_ZOOM,SCALE_ZOOM);
            					hit2D.collider.gameObject.GetComponent<SpriteRenderer>().color = new Color(0.75f,0.75f,0.75f,1.0f);
            					hitObject2D = hit2D.collider.gameObject;
            					input_status[i] = INPUT_NULL;
            				}
            				
            			}
					}
					
				}else if(Input.GetMouseButton(i)){
					
					//Debug.Log("MouseButton...");
					
					input_status[i] = INPUT_DOWNING;
					
					// ワールド座標として変換
					tapPoint2D = camera.ScreenToWorldPoint(Input.mousePosition);
  	  				
  	  				// コリダー取得
					collition2D = Physics2D.OverlapPoint(tapPoint2D);
					
  	  				input_now_x[i] = (int)Input.mousePosition.x;
					input_now_y[i] = (int)Input.mousePosition.y;
  	  				input_world_now_x[i] = tapPoint2D.x;
  	  				input_world_now_y[i] = tapPoint2D.y;
					
  	  				if(collition2D){
        				hit2D = Physics2D.Raycast(tapPoint2D,-Vector2.up);
        				if(hit2D){
        					
        					input_status[i] = INPUT_NULL;
            				
						}
        				
					}else{
						
						if(hitObject2D != null){
							
							//hitObject2D.transform.Find("on").gameObject.SetActive(false);
        					//hitObject2D.transform.Find("off").gameObject.SetActive(true);
							//hitObject2D.transform.localScale = new Vector2(SCALE_OFF,SCALE_OFF);
							hitObject2D.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
							hitObject2D = null;
							
						}
						
					}
  	  				
				}else if(Input.GetMouseButtonUp(i)){
					
					//Debug.Log("MouseButtonUp...");
					
					input_status[i] = INPUT_UP;
					input_time[i] = (Time.realtimeSinceStartup * 1.0f) - input_time[i];
					
					// ワールド座標として変換
					tapPoint2D = camera.ScreenToWorldPoint(Input.mousePosition);
  	  				
  	  				// コリダー取得
					collition2D = Physics2D.OverlapPoint(tapPoint2D);
					
  	  				input_up_x[i] = (int)Input.mousePosition.x;
					input_up_y[i] = (int)Input.mousePosition.y;
					input_world_up_x[i] = tapPoint2D.x;
  	  				input_world_up_y[i] = tapPoint2D.y;
  	  				
  	  				if(collition2D){
        				hit2D = Physics2D.Raycast(tapPoint2D,-Vector2.up);
        				if (hit2D) {
        					
            				//Debug.Log("ray hit object is:" + hit2D.collider.gameObject.name);
            				
            				if(hitObject2D != null){
            					
            					// タッチアップを確認
            					checkInputUp(i);
            					
            					//hitObject2D.transform.Find("on").gameObject.SetActive(false);
        						//hitObject2D.transform.Find("off").gameObject.SetActive(true);
            					//hitObject2D.transform.localScale = new Vector2(SCALE_OFF,SCALE_OFF);
            					hitObject2D.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
								hitObject2D = null;
								
            				}
            				
        				}
        				
    				}else{
						
						if(hitObject2D != null){
							
							//hitObject2D.transform.Find("on").gameObject.SetActive(false);
        					//hitObject2D.transform.Find("off").gameObject.SetActive(true);
							//hitObject2D.transform.localScale = new Vector2(SCALE_OFF,SCALE_OFF);
							hitObject2D.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
							hitObject2D = null;
							
						}
						
					}
  	  				
				}else{
					
					//input_status[i] = INPUT_NULL;
					
				}
			
			}
			
			// ダブルタッチで同じ入力が設定されたら片方を消す
			if(mode == MODE_SINGLE){
				if(input_status[0] == input_status[1]){
					input_status[1] = INPUT_NULL;
				}
			}
			
		}
		
	}
	
	
	////////////////////////////////////////////////////////////
	// 入力モードをセットする
	////////////////////////////////////////////////////////////
	public void setMode(int mode_param){
		
		if(mode_param == MODE_SINGLE){
			
			//?}???`?^?b?`????
			Input.multiTouchEnabled = false;
			
		}else if(mode_param == MODE_DOUBLE){
			
			//?}???`?^?b?`?L??
			Input.multiTouchEnabled = true;
		
		}
		
		mode = mode_param;
		
	}
	
	////////////////////////////////////////////////////////////
	// 入力のクリア
	////////////////////////////////////////////////////////////
	public void clearInput(){
		
		input_status[0] = INPUT_NULL;
		input_status[1] = INPUT_NULL;
		
	}
	
	////////////////////////////////////////////////////////////
	// 2pints距離計測
	////////////////////////////////////////////////////////////
	public float checkInputDistance(float dx,float dy,float nx,float ny){
		
		return Vector2.Distance(new Vector2(dx,dy),new Vector2(nx,ny));
		
	}
	
	////////////////////////////////////////////////////////////
	// タッチアップを検知したら確認
	////////////////////////////////////////////////////////////
	public void checkInputUp(int i){
		
		// 下の方に書かれているものが優先的に処理されるので注意せしめよ！
		if("INPUT_BUTTON_SELECT_A" == hit2D.collider.gameObject.name && "INPUT_BUTTON_SELECT_A" == hitObject2D.name){input_status[i] = INPUT_BUTTON_SELECT_A;}
		if("INPUT_BUTTON_SELECT_B" == hit2D.collider.gameObject.name && "INPUT_BUTTON_SELECT_B" == hitObject2D.name){input_status[i] = INPUT_BUTTON_SELECT_B;}
		
	}
	
}


