using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DiceAnim : MonoBehaviour {
    public float speed = 45;
    public float speed_end = 10;
    public float duration = .5f;
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
        await AsyncTweener.Tween(duration * 1.2f, t => {
            var v = dir * Mathf.Lerp(speed, speed_end, t);
            transform.localRotation *= Quaternion.Euler(v * Time.deltaTime);
            float st = Mathf.Lerp(-1, 1, t);
            float ht = 1 - st*st;
            transform.localScale = Vector3.Lerp(s, s*0.8f, ht);
        });

        await AsyncTweener.Tween(duration*.3f, t => alpha = t);         

        await AsyncTweener.Tween(duration * .85f, t => {           
            transform.localRotation = Quaternion.Slerp(transform.localRotation, r0, t);
        });

        await AsyncTweener.Wait(duration * .3f);
    }

    public void ResetPos(){
        transform.localRotation = Quaternion.identity;
    }
}
