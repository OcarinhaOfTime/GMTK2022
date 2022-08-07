using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileObj : MonoBehaviour {
    SpriteRenderer sr;
    public Vector2Int pos{
        get => MapController.instance.map.WorldPointToCoord(transform.position);
        set => transform.position = MapController.instance.map.CoordToWorldPoint(value);
    }

    public Color color{
        get => sr.color;
        set => sr.color = value;
    }

    public float alpha{
        get => sr.color.a;
        set => sr.color = sr.color.SetAlpha(value);
    }
    void Awake(){
        sr = GetComponent<SpriteRenderer>();
    }
}
