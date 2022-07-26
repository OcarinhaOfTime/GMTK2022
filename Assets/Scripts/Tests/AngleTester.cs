using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleTester : MonoBehaviour {
    public float a;
    void Update(){
        var v = new Vector2(transform.position.x, transform.position.y);        
        a = Mathf.Atan2(v.y, v.x) / Mathf.PI + Mathf.PI * .25f;
        a = Mathf.Floor(a * 2);
        a = Mathf.Repeat(a, 4);
    }
}
