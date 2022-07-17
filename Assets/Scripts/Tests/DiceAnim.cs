using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DiceAnim : MonoBehaviour {
    public float speed = 45;
    public float speed_end = 10;
    Material mat;
    public float alpha {
        set => mat.SetFloat("_Alpha", value);
    }

    public Texture2D tex {
        set => mat.SetTexture("_ValueTex", value);
    }

    // void Awake(){
    //     mat = GetComponent<MeshRenderer>().material;
    //     GetComponent<IClickable>().onClick.AddListener(Anim);
    // }

    public void Setup(){
        mat = GetComponent<MeshRenderer>().material;
    }
    
    [ContextMenu("Anim")]
    public async Task Anim(){
        var dir = Random.onUnitSphere;
        var r0 = Quaternion.identity;
        Vector3 s = transform.localScale;
        await AsyncTweener.Tween(1, t => {
            var v = dir * Mathf.Lerp(speed, speed_end, t);
            transform.localRotation *= Quaternion.Euler(v * Time.deltaTime);
            float st = Mathf.Lerp(-1, 1, t);
            float ht = 1 - st*st;
            transform.localScale = Vector3.Lerp(s, s*0.8f, ht);
        });

        await AsyncTweener.Tween(.5f, t => alpha = t); 

        

        await AsyncTweener.Tween(.25f, t => {           
            transform.localRotation = Quaternion.Slerp(transform.localRotation, r0, t);
        });

        await AsyncTweener.Wait(1);
        // await AsyncTweener.Tween(.25f, t => alpha = 1-t);
        // await AsyncTweener.Wait(.25f);
    }

    public void ResetPos(){
        transform.localRotation = Quaternion.identity;
    }
}
