using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileExtentions {
    public static int MDist(this (int, int) a, (int, int) b)   {
        (var x0, var y0) = a;
        (var x1, var y1) = b;
        return Mathf.Abs(x0 - x1) + Mathf.Abs(y0 - y1);
    }

    public static (int, int) ToTuple(this Vector2Int v){
        return (v.x, v.y);
    }

     public static int MDist(this Vector2Int a, Vector2Int b)   {
        (var x0, var y0) = a.ToTuple();
        (var x1, var y1) = b.ToTuple();
        return Mathf.Abs(x0 - x1) + Mathf.Abs(y0 - y1);
    }

    //public static operator Vector2Int ()
    
}
