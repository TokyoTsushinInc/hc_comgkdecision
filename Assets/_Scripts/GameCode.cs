using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCode : MonoBehaviour{
    
    // ファイナル
	public const int STATE_NULL = 0;
	public const int STATE_INTRO = 1000;
	public const int STATE_PLAY = 2000;
    public const int STATE_SELECT = 3000;
	public const int STATE_FINISH = 4000;
    
	// プレハブ(アタッチしてね)
	public GameObject[] prefabCan = new GameObject[3];
	
	// 外部コード参照用(コードでアタッチしてね)
	public MainCode mainCode = null;
	
	// ゲームオブジェクト(アタッチしてね)
	public GameObject objectCamera = null;
	public GameObject[] objectChara = new GameObject[8];
	public GameObject objectAnimQue = null;
	
	// ゲームオブジェクト(コードでアタッチしてね)
	public GameObject objectAnimFukidasi = null;
	public GameObject objectAnimSelectA = null;
	public GameObject objectAnimSelectB = null;
	
	// ゲームオブジェクト(プレハブから生成する)
	public GameObject[] objectCan = new GameObject[32];
	
	// メンバ
	public int use_chara_index = 0;
	public int state = STATE_NULL;
	public int sub = 0;
	public float timer = 0.0f;
	public string select_ab_str = "";
	public int can_index = 0;
	public Vector3 vec_can_target = Vector3.zero;
	
	////////////////////////////////////////////////////////
	// Awake 1st
	////////////////////////////////////////////////////////
    void Awake(){
        
		try{
			
			mainCode = GameObject.Find("MainCode").GetComponent<MainCode>();
			
			objectAnimFukidasi = GameObject.Find("UI/anim_fukidasi").gameObject;
			objectAnimSelectA = GameObject.Find("UI/anim_selectA").gameObject;
			objectAnimSelectB = GameObject.Find("UI/anim_selectB").gameObject;
			
		}catch(Exception e){
			
			// シーンをintroへ
			SceneManager.LoadScene("intro");
			
		}
		
    }
	
    ////////////////////////////////////////////////////////
	// Start 2nd
	////////////////////////////////////////////////////////
    void Start(){
        
		// ワーク変数
		int i = 0;
		string question_str = "";
		string a_str = "";
		string b_str = "";
		
		// メンバの初期化
		mainCode.current_index++;
		if(mainCode.current_index >= 4){
			mainCode.current_index = 0;
		}
		
		Debug.Log("### current_index:" + mainCode.current_index);
		
		if(mainCode.current_index == 0 || mainCode.current_index == 1 || mainCode.current_index == 2){
			use_chara_index = 0;// 0(jfk) or 1(trump)
		}
		if(mainCode.current_index == 3){
			use_chara_index = 1;// 0(jfk) or 1(trump)
		}
		
		state = STATE_INTRO;
		sub = 0;
		timer = 0.0f;
		select_ab_str = "";
		can_index = 0;
		vec_can_target = GameObject.Find("Targets/CanTarget").transform.position;
		
		// 大統領や秘書、記者のスキンを設定しておく
		objectChara[0].GetComponent<CharaCode>().setSkin(use_chara_index);
		objectChara[1].GetComponent<CharaCode>().setSkin(2);
		for(i = 2;i < 8;i++){
			objectChara[i].GetComponent<CharaCode>().setSkin(UnityEngine.Random.Range(3,6));
		}
		
		// それぞれのキャラのACを設定しておく
		// シンプルなクロスフェード
		mainCode.crossFade(objectChara[0].GetComponent<CharaCode>().getSkinObject(),"TALK");
		mainCode.crossFade(objectChara[1].GetComponent<CharaCode>().getSkinObject(),"IDLE1");
		
		// 大統領のフェイスを設定しておく
		objectChara[0].GetComponent<CharaCode>().setTalkFace(true);
		
		if(mainCode.current_index == 0){
			question_str = "Mr. President!\n Tikotok is ban?";
			a_str = "permission";
			b_str = "BAN";
		}
		if(mainCode.current_index == 1){
			question_str = "Mr. President!\n It's a pandemic!";
			a_str = "lockdown";
			b_str = "do nothing";
		}
		if(mainCode.current_index == 2){
			question_str = "You're cheating on me?";
			a_str = "Admit";
			b_str = "play dumb";
		}
		if(mainCode.current_index == 3){
			question_str = "Mr. President!\n Where to build new cities?";
			a_str = "country";
			b_str = "Near\n my house";
		}
		
		
		// テキストをセット
		objectAnimFukidasi.transform.FindChild("base/fukidasi/text").GetComponent<TextMesh>().text = question_str;
		objectAnimSelectA.transform.FindChild("base/text").GetComponent<TextMesh>().text = a_str;
		objectAnimSelectB.transform.FindChild("base/text").GetComponent<TextMesh>().text = b_str;
		
		// セッティングボタン
		mainCode.showSettingButton(true);
		
		// フルボタン
		mainCode.showFullButton(true);
		
		// ステータスの更新
		mainCode.setStatus();
		
	}
	
    ////////////////////////////////////////////////////////
	// 更新処理
	////////////////////////////////////////////////////////
    void LateUpdate(){
		
		if(mainCode.skip){
			
			mainCode.skip = false;
			
			// セットアニメ
			mainCode.setAnimation(objectAnimFukidasi,"anim_fukidasi_reset");
			mainCode.setAnimation(objectAnimSelectA,"anim_select_reset");
			mainCode.setAnimation(objectAnimSelectB,"anim_select_reset");
			
			mainCode.full = false;
								
			// フルボタン
			mainCode.showFullButton(false);
			
			// シーンを再読み込み
			SceneManager.LoadScene("game");
			
		}
		
		if(mainCode.colors){
			
			mainCode.colors = false;
			
			// ステータスの更新
			mainCode.setStatus();
			
		}
		
		switch(state){
			
			case STATE_NULL:
			
			break;
			
			case STATE_INTRO:
				
				if(mainCode.full){
					
					mainCode.full = false;
					state = STATE_PLAY;
					sub = 0;
					timer = 1.0f;
					
					// セッティングボタン
					mainCode.showSettingButton(false);
					
					// フルボタン
					mainCode.showFullButton(false);
					
					// カメラの移動
					setCameraPosition("CamPosLongShot",25.0f,0.0f);
					
				}
				
			break;
			
			case STATE_PLAY:
				
				if(sub == 0){
					
					timer -= Time.deltaTime * 1.0f;
					if(timer <= 0.0){
						
						sub = 1;
						timer = 2.0f;
						
						// セットアニメ
						mainCode.setAnimation(objectAnimFukidasi,"anim_fukidasi_in");
						
					}
					
				}else if(sub == 1){
					
					timer -= Time.deltaTime * 1.0f;
					if(timer <= 0.0){
						
						sub = 2;
						timer = 0.5f;
						
						// セットアニメ
						mainCode.setAnimation(objectAnimSelectA,"anim_select_in");
						mainCode.setAnimation(objectAnimSelectB,"anim_select_in");
						
					}
					
				}else if(sub == 2){
					
					timer -= Time.deltaTime * 1.0f;
					if(timer <= 0.0){
						
						timer = 0.0f;
						
						// InputCodeの確認
						if(mainCode.checkInput(InputCode.INPUT_BUTTON_SELECT_A)){
							
							// セットアニメ
							mainCode.setAnimation(objectAnimSelectA,"anim_select_choice");
							
							select_ab_str = "a";
							
							sub = 3;
							timer = 2.0f;
							
						}
						
						// InputCodeの確認
						if(mainCode.checkInput(InputCode.INPUT_BUTTON_SELECT_B)){
							
							// セットアニメ
							mainCode.setAnimation(objectAnimSelectB,"anim_select_choice");
							
							if(mainCode.current_index == 2){
								select_ab_str = "c";
							}else{
								select_ab_str = "b";
							}
							
							sub = 3;
							timer = 2.0f;
							
						}
						
					}
					
				}else if(sub == 3){
					
					timer -= Time.deltaTime * 1.0f;
					if(timer <= 0.0){
						
						// セットアニメ
						mainCode.setAnimation(objectAnimFukidasi,"anim_fukidasi_out");
						mainCode.setAnimation(objectAnimSelectA,"anim_select_out");
						mainCode.setAnimation(objectAnimSelectB,"anim_select_out");
						
						sub = 4;
						timer = 0.0f;
						
					}
					
				}else if(sub == 4){
					
					timer -= Time.deltaTime * 1.0f;
					if(timer <= 0.0){
						
						// セットアニメ
						if(use_chara_index == 0){
							mainCode.setAnimation(objectAnimQue,"anim_que_jfk_" + select_ab_str);
						}
						if(use_chara_index == 1){
							mainCode.setAnimation(objectAnimQue,"anim_que_trump_" + select_ab_str);
						}
						
						// カメラの移動
						if(select_ab_str == "a"){
							setCameraPosition("CamPosCloseUp",20.0f,1.0f);
						}else{
							if(mainCode.current_index == 2){
								setCameraPosition("CamPosCloseUp",20.0f,1.0f);
							}else{
								setCameraPosition("CamPosCloseUp",20.0f,3.0f);
							}
						}
						
						// フルボタン
						mainCode.showFullButton(true);
						
						sub = 5;
						timer = 0.0f;
						
					}
					
				}else if(sub == 5){
					
					if(use_chara_index == 0){
						
						if(false == mainCode.checkIsPlayAnimation(objectAnimQue,"anim_que_jfk_" + select_ab_str)){
							
							if(mainCode.full){
								
								mainCode.full = false;
								
								// フルボタン
								mainCode.showFullButton(false);
								
								// シーンを再読み込み
								SceneManager.LoadScene("game");
								
							}
							
						}
					}
					if(use_chara_index == 1){
						
						if(false == mainCode.checkIsPlayAnimation(objectAnimQue,"anim_que_trump_" + select_ab_str)){
							
							if(mainCode.full){
								
								mainCode.full = false;
								
								// フルボタン
								mainCode.showFullButton(false);
								
								// シーンを再読み込み
								SceneManager.LoadScene("game");
								
							}
							
						}
						
					}
					
				}
				
			break;
			
		}
		
	}
	
	////////////////////////////////////////////////////////
	// カメラの移動
	////////////////////////////////////////////////////////
	public void setCameraPosition(string str,float speed,float sleep){
		
		// コルーチン
		StartCoroutine(setCam(str,speed,sleep));
		
	}
	
	// コルーチン
	public IEnumerator setCam(string str,float speed, float interval){
		
		yield return new WaitForSeconds (interval);
		
		objectCamera.GetComponent<CameraCode>().objectCameraPosition = GameObject.Find("Targets/" + str).gameObject;
		
		objectCamera.GetComponent<CameraCode>().setCameraSpeed(speed);
		
	}
	
	////////////////////////////////////////////////////////
	// キューの実行
	////////////////////////////////////////////////////////
	public void actionQueCharaMotion(int index,string str){
		
		// シンプルなクロスフェード
		mainCode.crossFade(objectChara[index].GetComponent<CharaCode>().getSkinObject(),str);
		
		// カン投げ処理
		if(str == "THROW"){
			
			// コルーチン
			StartCoroutine(throwCan(index,1.5f));
			
		}
	}
	
	// コルーチン
	public IEnumerator throwCan(int index, float interval){
		
		yield return new WaitForSeconds (interval);
		
		objectCan[can_index] = Instantiate(prefabCan[UnityEngine.Random.Range(0,3)]) as GameObject;
		
		// 初期位置
		objectCan[can_index].transform.position = new Vector3(
			objectChara[index].transform.position.x - 0.5f,
			objectChara[index].transform.position.y + 1,
			objectChara[index].transform.position.z
		);
		
		// ターゲットに飛ばす
		objectCan[can_index].GetComponent<CanCode>().throwObject(vec_can_target);
		
		can_index++;
		
	}
	
	////////////////////////////////////////////////////////
	// キューの実行
	////////////////////////////////////////////////////////
	public void actionQueSetFace(int index,int id,bool flag,int start,int target){
		
		objectChara[index].GetComponent<CharaCode>().setFace(id,flag,start,target);
		
	}
	
	////////////////////////////////////////////////////////
	// キューの実行
	////////////////////////////////////////////////////////
	public void actionQueTalkFace(int index,bool flag){
		
		objectChara[index].GetComponent<CharaCode>().setTalkFace(flag);
		
	}
	
	////////////////////////////////////////////////////////
	// キューの実行
	////////////////////////////////////////////////////////
	public void actionQueMove(int index,string str){
		
		// セットアニメ
		mainCode.setAnimation(objectChara[index].transform.parent.transform.gameObject,str);
		
	}
	
	////////////////////////////////////////////////////////
	// キューの実行
	////////////////////////////////////////////////////////
	public void actionQueEffect(int index,string str){
		
		if(str == "sweat"){
			objectChara[index].GetComponent<CharaCode>().playEffectSweat();
		}
		
		if(str == "angry"){
			objectChara[index].GetComponent<CharaCode>().playEffectAngry();
		}
		
	}
	
}
