using UnityEngine;
using System.Collections;

public class AvatarIKCode : MonoBehaviour {
	
	[SerializeField]
	public Transform leftHandAnchor = null;
	[SerializeField]
	public Transform rightHandAnchor = null;
	/*
	[SerializeField]
	public Transform leftFootAnchor = null;
	[SerializeField]
	public Transform rightFootAnchor = null;
	*/
	[SerializeField]
	public Transform headAnchor = null;
	
	private Animator animator;
	
	// fixパラメータ
	public int ignore_fix_param = 0;
	public int fix_param_mode = 1;
	public float fix_param = 0.0f;
	
	void Awake(){
		
		animator = GetComponent<Animator>();
		
	}
	
	void OnAnimatorIK(){
		
		// アニメーターが設定されていない場合は無視だわぃな。
		if(!animator){
			
			return;
		}
		
		if(fix_param_mode == 0){
			
			if(fix_param != 1.0f){
				fix_param += Time.deltaTime * 1.0f;
				if(fix_param >= 1.0f){
					fix_param = 1.0f;
				}
			}
			
		}
		
		if(fix_param_mode == 1){
			
			if(fix_param != 0.0f){
				fix_param -= Time.deltaTime * 1.0f;
				if(fix_param <= 0.0f){
					fix_param = 0.0f;
				}
			}
			
		}
		
		if(ignore_fix_param == 1){
			fix_param = 0.0f;
		}
		
		
		
		// fixパラータの値に従う
		if (leftHandAnchor != null)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, fix_param);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, fix_param);
			animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandAnchor.position);
			animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandAnchor.rotation);
			
		}
		
		if (rightHandAnchor != null)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, fix_param);
			animator.SetIKRotationWeight(AvatarIKGoal.RightHand, fix_param);
			animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandAnchor.position);
			animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandAnchor.rotation);
			
		}
		/*
		if (leftFootAnchor != null)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, fix_param);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, fix_param);
			animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootAnchor.position);
			animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootAnchor.rotation);
		}
		
		if (rightFootAnchor != null)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, fix_param);
			animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, fix_param);
			animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootAnchor.position);
			animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootAnchor.rotation);
		}
		*/
		
		// ヘッドは例外的にやってる。
		if(headAnchor != null){
			
			/*
			// SetLookAtWeight()のパラメータリスト
			weight		全体の重み
			bodyWeight	体を動かす重み
			headWeight	頭を動かす重み
			eyesWeight	目を動かす重み
			clampWeight	モーションの制限量
			*/
			
			//animator.SetLookAtWeight(fix_param / 1.5f, 0.5f, 1.0f, 0.0f, 0.5f);
			animator.SetLookAtWeight(fix_param, 0.5f, 1.0f, 0.0f, 1.0f);
			
			animator.SetLookAtPosition(headAnchor.position);
           	
		}
		
	}
	
	
	public void setLeftHandAnchor(Transform tf){
		
		leftHandAnchor = tf;
		
	}
	
	public void setRightHandAnchor(Transform tf){
		
		rightHandAnchor = tf;
		
	}
	
	public void setHeadAnchor(Transform tf){
		
		headAnchor = tf;
		
	}
	
	public void setHeadAnchorNull(){
		
		headAnchor = null;
		
	}
	
	public void setFixParamMode(int i){
		
		fix_param_mode = i;
		
	}
	
	public void setFixParam(float f){
		
		if(f >= 1.0f){
			f = 1.0f;
		}
		
		fix_param = f;
		
	}
	
	public void setIgnoreFixParam(int i){
		
		ignore_fix_param = i;
		
	}
	
	
	
	
}