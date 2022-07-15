using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkEffect : MonoBehaviour {
    public float freq = 1;
    Graphic gfx;

    void Awake(){
        gfx = GetComponent<Graphic>();
    }
    void Update(){
        var t = Mathf.Sin(Time.time * Mathf.PI * freq);
        var a = MathUtils.Smoothstep(-.5f, .5f, t);
        gfx.color = gfx.color.SetAlpha(a);
    }
}
