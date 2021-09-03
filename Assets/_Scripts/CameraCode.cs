using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCode : MonoBehaviour{
	
	public float camera_speed = 10.0f;
	
	public Vector3 velocity_for_camera_position = Vector3.zero;		// SmoothDampで利用するワーク用
	public GameObject objectCameraPosition = null;					// カメラポジション
	
	////////////////////////////////////////////////////////////
	// 更新処理
	////////////////////////////////////////////////////////////
	void Update(){
		
		// カメラの更新
		updateCameraPosition();
		
	}
	
	////////////////////////////////////////////////////////////
	// カメラの追従速度の設置
	////////////////////////////////////////////////////////////
	public void setCameraSpeed(float time){
		
		camera_speed = time;
		
	}
	
	////////////////////////////////////////////////////////////
	// カメラの更新
	////////////////////////////////////////////////////////////
	public void updateCameraPosition(){
		
		// ワーク変数
		float speed = 0.0f;
		Vector3 target = Vector3.zero;
		
		// ターゲット
		target = objectCameraPosition.transform.position;
		
		// 追従速度
		speed = Time.deltaTime * camera_speed;
		
		this.transform.position = Vector3.SmoothDamp(
			this.transform.position,
			target,
			ref velocity_for_camera_position,
			speed
		);
		
	}
	
}