using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GMTK/UnitData")]
public class UnitAttributes : ScriptableObject {
    public string className;
    public int hp = 10;
    public int atk = 2;
    public int move = 2;
    public int range = 1;
    public Sprite sprite;
    public Sprite[] sprite_dirs;
    public int IdxFromDir(Vector2 v){
        var a = Mathf.Atan2(v.y, v.x) / Mathf.PI + Mathf.PI * .25f;
        a = Mathf.Floor(a * 2);
        int d = (int)Mathf.Repeat(a, 4);
        if(d > 1){
            d = d == 2 ? 3 : 2;
        }

        return d;
    }
    public Sprite SpriteFromDir(Vector2 v){
        if(sprite_dirs.Length < 1) return sprite;
        var a = Mathf.Atan2(v.y, v.x) / Mathf.PI + Mathf.PI * .25f;
        a = Mathf.Floor(a * 2);
        int d = (int)Mathf.Repeat(a, 4);
        if(d > 1){
            d = d == 2 ? 3 : 2;
        }

        return sprite_dirs[d];
    }
}
