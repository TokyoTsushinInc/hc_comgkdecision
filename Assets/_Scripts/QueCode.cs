using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueCode : MonoBehaviour{
    
    // 外部コード参照用(コードでアタッチしてね)
	public GameCode gameCode = null;
    
	////////////////////////////////////////////////////////
	// Awake 1st
	////////////////////////////////////////////////////////
    void Awake(){
        
        gameCode = GameObject.Find("GameCode").GetComponent<GameCode>();
        
    }
	
	////////////////////////////////////////////////////////
	// キュー(指定のキャラにモーションをつける)
	////////////////////////////////////////////////////////
	public void queCharaMotion(string str){
		
		// 入力形式(インデックスとモーション名)
		// 1,TALK1
		
		// [,]でスプリット
		string[] sprit_str = str.Split(","[0]);
		
		gameCode.actionQueCharaMotion(int.Parse(sprit_str[0]),sprit_str[1]);
		
	}
	
	////////////////////////////////////////////////////////
	// キュー(指定のキャラにモーションをつける)
	////////////////////////////////////////////////////////
	public void queCharaMotionRandom(string str){
		
		// 入力形式(インデックスとモーション名)
		// 1,CLAP,HANDWAVE
		
		// [,]でスプリット
		string[] sprit_str = str.Split(","[0]);
		
		int ran = UnityEngine.Random.Range(1,3);
		
		gameCode.actionQueCharaMotion(int.Parse(sprit_str[0]),sprit_str[ran]);
		
	}
	
	////////////////////////////////////////////////////////
	// キュー(セットフェイス)
	////////////////////////////////////////////////////////
	public void queSetFace(string str){
		
		// 入力形式(インデックス,フェイスid,フラグ,開始param,ターゲットparam)
		// 0,1,true,50,100
		
		// [,]でスプリット
		string[] sprit_str = str.Split(","[0]);
		
		bool flag = false;
		if(sprit_str[2] == "true"){
			flag = true;
		}
		if(sprit_str[2] == "false"){
			flag = false;
		}
		
		gameCode.actionQueSetFace(
			int.Parse(sprit_str[0]),
			int.Parse(sprit_str[1]),
			flag,
			int.Parse(sprit_str[3]),
			int.Parse(sprit_str[4])
		);
		
	}
	
	////////////////////////////////////////////////////////
	// キュー(トークフェイス)
	////////////////////////////////////////////////////////
	public void queTalkFace(string str){
		
		// 入力形式(インデックスとフラグ)
		// 0,false
		
		// [,]でスプリット
		string[] sprit_str = str.Split(","[0]);
		
		if(sprit_str[1] == "true"){
			gameCode.actionQueTalkFace(int.Parse(sprit_str[0]),true);
		}
		if(sprit_str[1] == "false"){
			gameCode.actionQueTalkFace(int.Parse(sprit_str[0]),false);
		}
		
	}
    
}
