using UnityEngine;
using System.Collections;

public class LookAtSmoothCode : MonoBehaviour {
	
	public float time = 1.0f;
	public Transform lookTransform;
	
	
	void Update(){
		
		if(lookTransform != null){
			
			Quaternion lookOnLook = Quaternion.LookRotation(lookTransform.position - transform.position);
 
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				lookOnLook,
				Time.deltaTime * time
			);
			
		}
		
	}
	
	
}