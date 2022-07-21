using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkSprite : MonoBehaviour {
    public float freq = 1;
    SpriteRenderer gfx;
    public float t0;

    void Awake(){
        gfx = GetComponent<SpriteRenderer>();
    }
    void Update(){
        t0 += Time.deltaTime;
        var t = Mathf.Sin(t0 * Mathf.PI * freq);
        var a = MathUtils.Smoothstep(-1.0f, .9f, t);
        gfx.color = gfx.color.SetAlpha(Mathf.Pow(a, .25f));
    }
}
