using UnityEngine;
using System.Collections;

public class TenjinCode : MonoBehaviour {
	
  	// Use this for initialization
  	public void callStart () {
		
    	BaseTenjin instance = Tenjin.getInstance("T2WDDB18C9FSBMDZACZURGZ2ZUCYYQRZ");// T2WDDB18C9FSBMDZACZURGZ2ZUCYYQRZ
    	instance.Connect();
    	
    	Debug.Log("###:Tenjin Connect in Start()");
  		
  	}
	
	void OnApplicationPause(bool pauseStatus){
		
    	if (pauseStatus) {
    		
    	  	//do nothing
    	  	
    	} else {
    		
    		BaseTenjin instance = Tenjin.getInstance("T2WDDB18C9FSBMDZACZURGZ2ZUCYYQRZ");// T2WDDB18C9FSBMDZACZURGZ2ZUCYYQRZ
   			instance.Connect();
    		
    		Debug.Log("###:Tenjin Connect in OnApplicationPause()");
    		
    	}
  		
  	}
  	
}

