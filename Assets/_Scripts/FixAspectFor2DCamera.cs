using UnityEngine;
using System.Collections;
 
public class FixAspectFor2DCamera : MonoBehaviour {
    
    // �O������m�F����l
    static float camera_orthographic_size = 5.0f;// �f�t�H���g�̃T�C�Y�������l�Ƃ��ď����Ă��������ƁB
    static float camera_orthographic_size_width = 2.8f;// �f�t�H���g�̃T�C�Y�������l�Ƃ��ď����Ă��������ƂȁB
    
    void Start(){
    	
    	// �Ώۂ̃J�������擾����
		Camera camera = this.GetComponent<Camera>();
		
    	// �J�����Ă����ʂ����ɏc����擾 (�c���)
    	float developAspect = 640.0f / 1136.0f;
    	
        // ����ʂŊJ�����Ă���ꍇ�͈ȉ��̗p�ɐ؂�ւ��܂�
        //float developAspect = 1136.0f / 640.0f;
        
        // ���@�̃T�C�Y���擾���āA�c����擾
        float deviceAspect = (float)Screen.width / (float)Screen.height;
		
		// 640x1136�̒[�����c�̔䗦�����������̂͏c�Ńt�B�b�N�X������(�Ȃɂ����Ȃ����̏���)
		if(developAspect > deviceAspect){
			
            // ���@�ƊJ����ʂƂ̑Δ�
            float scale = deviceAspect / developAspect;
			
            // �J�����ɐݒ肵�Ă���orthographicSize�����@�Ƃ̑Δ�ŃX�P�[��
            float deviceSize = camera.orthographicSize;
            
            // �f�o�C�X�T�C�Y�͌Œ�
            deviceSize = 5.0f;
            
            // scale�̋t��
            float deviceScale = 1.0f / scale;
            
            // orthographicSize���v�Z������
            camera.orthographicSize = deviceSize * deviceScale;
            
       	}
        
        // �l��ێ�
    	camera_orthographic_size = camera.orthographicSize;
    	
    	Debug.Log("### camera_orthographic_size..." + camera_orthographic_size);
    	
    	// width�l�����߂邼�[
    	camera_orthographic_size_width = camera_orthographic_size * deviceAspect;
    	
        Debug.Log("### camera_orthographic_size_width..." + camera_orthographic_size_width);
        
    }
    
    ///////////////////////////////////
    // ���݂�orthographicSize(Height)��Ԃ�
    ///////////////////////////////////
    public float getCameraorthographicSizeHeight(){
    	
    	return camera_orthographic_size;
    	
    }
    
    ///////////////////////////////////
    // ���݂�orthographicSize(Width)��Ԃ�
    ///////////////////////////////////
    public float getCameraorthographicSizeWidth(){
    	
    	return camera_orthographic_size_width;
    	
    }
    
    
    
    
    
    
    
    
    
}
