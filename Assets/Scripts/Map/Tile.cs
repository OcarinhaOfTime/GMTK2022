using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour {
    public Coord coord;
    public int x {
        get {
            return coord.x;
        }
    }
    public int y {
        get {
            return coord.y;
        }
    }
    public int cost = 1;
    public int const_compound => cost + (unit != null ? 999 : 0);
    public int cost_value { set { costt.text = "" + value; cost = value; } }
    [SerializeField] TMP_Text costt;
    private SpriteRenderer spriteRenderer;
    public Color activeColor;
    public Color deactiveColor;
    public Unit unit = null;

    public bool active = false;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Active() {
        spriteRenderer.color = activeColor;
        active = true;
    }

    public void Deactive() {
        spriteRenderer.color = deactiveColor;
        active = true;
    }
}
