
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class UIUtils {  
    public static Color SetAlpha(this Color color, float a){
        var c = color;
        c.a = a;
        return c;
    }

    public static void SetAlpha(this Graphic g, float a){
        var c = g.color;
        c.a = a;
        g.color = c;
    }
}
