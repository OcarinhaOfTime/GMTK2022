using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DiceUtils {
    public static int d4 => Random.Range(1, 5);
    public static int d6 => Random.Range(1, 7);
    public static int d8 => Random.Range(1, 9);
}
