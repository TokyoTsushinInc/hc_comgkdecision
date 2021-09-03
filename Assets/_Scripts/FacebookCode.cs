using UnityEngine;
using System.Collections;
using Facebook.Unity;

public class FacebookCode : MonoBehaviour {
	
  	    public void callStart()
        {
            Init();
        }
        private void OnApplicationPause(bool pause)
        {
            if (!pause)
            {
                Init();
            }
        }

        private void Init()
        {
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                FB.Init(() => {
                    
					//string logMessage = string.Format(
                    //    "OnInitCompleteCalled AppId='{0}' IsLoggedIn='{1}' IsInitialized='{2}'",
                    //        FB.AppId,
                    //        FB.IsLoggedIn,
                    //        FB.IsInitialized);
                    //        Debug.Log(logMessage);
                    
                    FB.ActivateApp();
                });
            }
        }
  	
}
