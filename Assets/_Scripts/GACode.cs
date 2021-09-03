using UnityEngine;
using System.Collections;

using GameAnalyticsSDK;

public class GACode : MonoBehaviour {
	
  	// Use this for initialization
  	public void callStart () {
		
  		Debug.Log("###:GA init...");

    	GameAnalytics.Initialize();
  		
  	}
	
}