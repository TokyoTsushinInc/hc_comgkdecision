using UnityEngine;

public class CanCode : MonoBehaviour{
	
	// 外部コード参照用
	//public MainProcessAdd0Code mainProcessCode = null;
	
	public const int STATUS_NULL = 0;
	public const int STATUS_THROW = 1000;
	
	// メンバ
	public int status = STATUS_NULL;
	
	////////////////////////////////////////////////////////////
	// 初期化
	////////////////////////////////////////////////////////////
    void Awake(){
		
		// 外部コード参照用
		//mainProcessCode = GameObject.Find("System/MainProcessAdd0Code").GetComponent<MainProcessAdd0Code>();
		
    }
	
	////////////////////////////////////////////////////////////
	// スローオブジェクト
	////////////////////////////////////////////////////////////
	public void throwObject(Vector3 target_pos){
		
		// ワーク変数
		Vector3 target = Vector3.zero;
		float angle = 0.0f;
		Vector3 velocity = Vector3.zero;
		Rigidbody rigidbody = null;
		float speed = 0.0f;
		
		// ターゲット座標
		target = target_pos;
		
		// 発射角度
		angle = 45.0f;
		
		// 射出速度を算出
		velocity = calcGotoTargetVelocity(this.transform.position, target, angle);
		
		// リジッドボディ取得とリセット
		rigidbody = this.GetComponent<Rigidbody>();
		rigidbody.velocity = Vector3.zero;
		rigidbody.angularVelocity = Vector3.zero;
		
		// 射出(rigidbodyのMass==1,Drag==0,AngularDrag==0.05のデフォルト値にしておこう)
		rigidbody.AddForce(velocity * rigidbody.mass, ForceMode.Impulse);
		
		// 回転
		speed = Random.Range(30.0f,50.0f);
		rigidbody.AddTorque(new Vector3(0.0f,Random.Range(-1.0f,1.0f),1.0f) * speed);
		
	}
	
	////////////////////////////////////////////////////////////
	// 標的に命中する射出速度の計算
	////////////////////////////////////////////////////////////
	public Vector3 calcGotoTargetVelocity(Vector3 pointA,Vector3 pointB,float angle){
		
		// ワーク変数
		float rad = 0.0f;
		float x = 0.0f;
		float y = 0.0f;
		float speed = 0.0f;
		
		// 射出角をラジアンに変換
		rad = angle * Mathf.PI / 180.0f;
		
		// 水平方向の距離x
		x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));
		
		// 垂直方向の距離y
		y = pointA.y - pointB.y;
		
		// 斜方投射の公式を初速度について解く
		speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2.0f) / (2.0f * Mathf.Pow(Mathf.Cos(rad), 2.0f) * (x * Mathf.Tan(rad) + y)));
		
		if(float.IsNaN(speed)){
			
			// 条件を満たす初速を算出できなければVector3.zeroを返す
			return Vector3.zero;
			
		}else{
			
			return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
			
		}
		
	}
	
}