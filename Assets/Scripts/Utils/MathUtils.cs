using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils {
    public static float Smoothstep(float a, float b, float x) {
        float t = Mathf.Clamp01((x - a) / (b - a));
        return t * t * (3 - (2 * t));
    }

    public static float Step(float a, float x = .5f){
        return x >= a ? 1 : 0;
    }   

    public static void SmoothArray(float t, float[] weights, float[] refs, float k = .5f){
        for(int i=0; i<refs.Length; i++){
            var w = Mathf.Abs(refs[i] - t);
            weights[i] = Smoothstep(k, 0, w);
        }
    }

    public static void SmoothArray(float t, float[] r, float k = .5f){
        var n = r.Length;
        for(int i=0; i<n; i++){
            var w = Mathf.Abs(i / (n - 1f) - t);
            r[i] = Smoothstep(k, 0, w);
        }
    }
}
