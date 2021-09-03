using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyCode : MonoBehaviour{
	
	// このオブジェクトとその配下のオブジェクトは、DontDestroy扱いであるよ！
	
    /////////////////////////////////////
    // Awake処理
    /////////////////////////////////////
	void Awake(){
		
		DontDestroyOnLoad(gameObject);
		
	}
    
}
