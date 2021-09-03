using UnityEngine;
using System;
using System.Runtime.InteropServices;

// 10.3か、それ以上で使えるので
// 呼び出す段階でチェックをしておきましょー。

public class iOSReview{
	
	#if UNITY_IOS
		
  	  	[DllImport("__Internal")]
  	  		
  	  	private static extern void _requestReview();
  	  	
  	  	public static void requestReview(){
        	
        	if (Application.platform != RuntimePlatform.OSXEditor) {
        		_requestReview();
        	}
        	
  	  	}
  	  	
  	#endif
	
}

