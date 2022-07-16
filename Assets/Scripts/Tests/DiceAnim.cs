using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceAnim : MonoBehaviour {
    public float speed = 45;
    public float speed_end = 10;
    public float power = .25f;
    public float laps = 3;
    Material mat;
    float alpha {
        set => mat.SetFloat("_Alpha", value);
    }

    void Awake(){
        mat = GetComponent<MeshRenderer>().material;
        GetComponent<IClickable>().onClick.AddListener(Anim);
    }
    
    [ContextMenu("Anim")]
    async void Anim(){
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

        // await AsyncTweener.Tween(.25f, t => {           
        //     transform.localRotation = Quaternion.Slerp(transform.localRotation, r0, t);
        // });

        await AsyncTweener.Wait(1);
        await AsyncTweener.Tween(.25f, t => alpha = 1-t);
    }
}
