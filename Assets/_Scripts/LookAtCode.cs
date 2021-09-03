using UnityEngine;
using System.Collections;

public class LookAtCode : MonoBehaviour {
	
	public Transform lookTransform;
	
	
	void Update(){
    	transform.LookAt(lookTransform);
	}
	
	
}