using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour {
    public HideFlags customHideFlags;
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
    //private SpriteRenderer spriteRenderer;
    public Color activeColor;
    public Color deactiveColor;
    public Unit unit = null;

    public bool active = false;
    [SerializeField]SpriteRenderer base_rdr;
    [SerializeField]SpriteRenderer detail_rdr;
    [SerializeField]SpriteRenderer overlay_rdr;

    [SerializeField]Sprite base_sprite;
    [SerializeField]Sprite detail_sprite;

    void Awake() {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }
    [ContextMenu("Hide Sprites")]
    public void HideSprites(){
        base_rdr.gameObject.hideFlags = customHideFlags;
        detail_rdr.gameObject.hideFlags = customHideFlags;
        overlay_rdr.gameObject.hideFlags = customHideFlags;
    }

    [ContextMenu("Reset Editor")]
    public void ResetEditor(){
        base_rdr.gameObject.hideFlags = 0;
        detail_rdr.gameObject.hideFlags = 0;
        overlay_rdr.gameObject.hideFlags = 0;
    }

    public void Active() {
        overlay_rdr.color = activeColor;
        active = true;
    }

    public void Deactive() {
        overlay_rdr.color = deactiveColor;
        active = true;
    }

    [ContextMenu("Apply Sprites")]
    public void ApplySprites(){
        base_rdr.sprite = base_sprite;
        detail_rdr.sprite = detail_sprite;
        detail_rdr.enabled = detail_sprite != null;
    }

    void OnValidate(){
        ApplySprites();
    }
}
