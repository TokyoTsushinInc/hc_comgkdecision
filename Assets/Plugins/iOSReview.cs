using UnityEngine;
using System;
using System.Runtime.InteropServices;

// 10.3���A����ȏ�Ŏg����̂�
// �Ăяo���i�K�Ń`�F�b�N�����Ă����܂���[�B

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

