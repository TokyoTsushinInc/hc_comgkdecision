using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaCode : MonoBehaviour{
    
    // ゲームオブジェクト
    public GameObject[] objectSkin = new GameObject[8];
    
    // スキンメッシュ
    public SkinnedMeshRenderer[] smr = new SkinnedMeshRenderer[8];
    
    // エフェクト
    public ParticleSystem[] effectAngry = new ParticleSystem[4];
    public ParticleSystem effectSweat = null;
    
    // メンバ
    public int skin = 0;
    public int shape_index = -1;
    public float shape_param = 0.0f;
    public float shape_target_param = 0.0f;
    public bool talk_face = false;
    
    // Start is called before the first frame update
    void Start(){
        
        
        
    }
    
    // Update is called once per frame
    void Update(){
        
        updateFace();
        
    }
    
    ////////////////////////////////////////////////////////
    // スキンの設定
    ////////////////////////////////////////////////////////
    public void setSkin(int index){
        
        // ワーク変数
        int i = 0;
        
        // スキンidを設定
        skin = index;
        shape_index = -1;
        shape_param = 0.0f;
        shape_target_param = 0;
        talk_face = false;
        
        // スキン表示
        for(i = 0;i < 8;i++){
            
            if(objectSkin[i] != null){
                
                objectSkin[i].SetActive(false);
                
                if(skin == i){
                    
                    objectSkin[i].SetActive(true);
                    
                }
                
            }
            
        }
        
    }
    
    ////////////////////////////////////////////////////////
    // スキンのオブジェクトを返す
    ////////////////////////////////////////////////////////
    public GameObject getSkinObject(){
        
        // ワーク変数
        int i = 0;
        GameObject obj = null;
        
        for(i = 0;i < 8;i++){
            
            if(i == skin){
                
                obj = objectSkin[i];
                
                break;
                
            }
            
        }
        
        return obj;
        
    }
    
    ////////////////////////////////////////////////////////
    // フェイスの設定
    ////////////////////////////////////////////////////////
    public void setFace(int index,bool flag,float param,float target_param){
        
        shape_index = index;
        if(flag){
            shape_param = shape_param;
        }else{
            shape_param = param;
        }
        shape_target_param = target_param;
        
        smr[skin].SetBlendShapeWeight(shape_index,shape_param);
        
    }
    
    ////////////////////////////////////////////////////////
    // トークフェイスの設定
    ////////////////////////////////////////////////////////
    public void setTalkFace(bool flag){
        
        talk_face = flag;
        
    }
    
    ////////////////////////////////////////////////////////
    // フェイスの更新
    ////////////////////////////////////////////////////////
    public void updateFace(){
        
        // ワーク変数
        bool fix = false;
        float speed = 200.0f;
        
        if(shape_index != -1){
            
            // up
            if(shape_param < shape_target_param){
                
                shape_param += Time.deltaTime * speed;
                if(shape_param > shape_target_param){
                    shape_param = shape_target_param;
                    fix = true;
                }
                
                smr[skin].SetBlendShapeWeight(shape_index,shape_param);
                
                if(fix){
                    shape_index = -1;
                }
                
            // down
            }else{
                
                shape_param -= Time.deltaTime * speed;
                if(shape_param < shape_target_param){
                    shape_param = shape_target_param;
                    fix = true;
                }
                
                smr[skin].SetBlendShapeWeight(shape_index,shape_param);
                
                if(fix){
                    shape_index = -1;
                }
                
            }
            
        }
        
        if(shape_index == -1){
            
            if(talk_face){
                
                // フェイスの設定
                if(skin == 0){
                    setFace(0,true,shape_param,(float)UnityEngine.Random.Range(0,101));
                }
                if(skin == 1){
                    setFace(1,true,shape_param,(float)UnityEngine.Random.Range(0,101));
                }
                if(skin == 2){
                    setFace(0,true,shape_param,(float)UnityEngine.Random.Range(0,101));
                }
            }
            
        }
        
    }
    
    ////////////////////////////////////////////////////////
    // エフェクトの再生
    ////////////////////////////////////////////////////////
    public void playEffectAngry(){
        
        // ワーク変数
        int i = 0;
        
        for(i = 0;i < 4;i++){
            
            effectAngry[i].GetComponent<ParticleSystem>().Play();
            
        }
        
    }
    
    ////////////////////////////////////////////////////////
    // エフェクトの再生
    ////////////////////////////////////////////////////////
    public void playEffectSweat(){
        
        effectSweat.GetComponent<ParticleSystem>().Play();
        
    }
    
}
