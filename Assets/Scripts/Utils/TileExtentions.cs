using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TileExtentions {
    public static int ManhattanDistance(this (int, int) a, (int, int) b)   {
        (var x0, var y0) = a;
        (var x1, var y1) = b;
        return Mathf.Abs(x0 - x1) + Mathf.Abs(y0 - y1);
    }
}
