using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAngleTester : MonoBehaviour {
    [SerializeField] UnitAttributes attribs;
    [SerializeField] SpriteRenderer sr;

    void Update(){
        Vector2 v = transform.position;
        sr.sprite = attribs.SpriteFromDir(v);
    }
}
