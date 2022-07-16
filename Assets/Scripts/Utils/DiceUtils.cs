using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiceUtils {
    public static int d4 => Random.Range(1, 5);
    public static int d6 => Random.Range(1, 7);
    public static int d8 => Random.Range(1, 9);

    public static int[] d6x(this int k){
        var r = new int[k];
        for(int i=0; i<k; i++) r[i] = d6;
        return r;
    }

    public static int sum(this int[] r){
        var s = 0;
        for(int i=0; i<r.Length; i++) s += r[i];
        return s;
    }
}
