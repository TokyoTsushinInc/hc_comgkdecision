using UnityEngine;
using System.Collections;
 
public class FixAspectFor2DCamera : MonoBehaviour {
    
    // 外部から確認する値
    static float camera_orthographic_size = 5.0f;// デフォルトのサイズを初期値として書いておこうっと。
    static float camera_orthographic_size_width = 2.8f;// デフォルトのサイズを初期値として書いておこうっとな。
    
    void Start(){
    	
    	// 対象のカメラを取得する
		Camera camera = this.GetComponent<Camera>();
		
    	// 開発している画面を元に縦横比取得 (縦画面)
    	float developAspect = 640.0f / 1136.0f;
    	
        // 横画面で開発している場合は以下の用に切り替えます
        //float developAspect = 1136.0f / 640.0f;
        
        // 実機のサイズを取得して、縦横比取得
        float deviceAspect = (float)Screen.width / (float)Screen.height;
		
		// 640x1136の端末より縦の比率が小さいものは縦でフィックスさせる(なにもしない方の処理)
		if(developAspect > deviceAspect){
			
            // 実機と開発画面との対比
            float scale = deviceAspect / developAspect;
			
            // カメラに設定していたorthographicSizeを実機との対比でスケール
            float deviceSize = camera.orthographicSize;
            
            // デバイスサイズは固定
            deviceSize = 5.0f;
            
            // scaleの逆数
            float deviceScale = 1.0f / scale;
            
            // orthographicSizeを計算し直す
            camera.orthographicSize = deviceSize * deviceScale;
            
       	}
        
        // 値を保持
    	camera_orthographic_size = camera.orthographicSize;
    	
    	Debug.Log("### camera_orthographic_size..." + camera_orthographic_size);
    	
    	// width値を求めるぞー
    	camera_orthographic_size_width = camera_orthographic_size * deviceAspect;
    	
        Debug.Log("### camera_orthographic_size_width..." + camera_orthographic_size_width);
        
    }
    
    ///////////////////////////////////
    // 現在のorthographicSize(Height)を返す
    ///////////////////////////////////
    public float getCameraorthographicSizeHeight(){
    	
    	return camera_orthographic_size;
    	
    }
    
    ///////////////////////////////////
    // 現在のorthographicSize(Width)を返す
    ///////////////////////////////////
    public float getCameraorthographicSizeWidth(){
    	
    	return camera_orthographic_size_width;
    	
    }
    
    
    
    
    
    
    
    
    
}
