﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tile : MonoBehaviour {
    public HideFlags customHideFlags;
    public Vector2Int coord;
    public int x => coord.x;
    public int y => coord.y;
    public int cost = 1;
    public int const_compound => cost + (unit != null ? 999 : 0);
    public int cost_value { set { costt.text = "" + value; cost = value; } }
    [SerializeField] TMP_Text costt;
    //private SpriteRenderer spriteRenderer;
    public Unit unit = null;

    //public bool active = false;
    [SerializeField]SpriteRenderer base_rdr;
    [SerializeField]SpriteRenderer detail_rdr;
    [SerializeField]SpriteRenderer overlay_rdr;

    [SerializeField]Sprite base_sprite;
    [SerializeField]Sprite detail_sprite;
    MapController controller;

    void Awake() {
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Setup(MapController controller){
        this.controller = controller;
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
        overlay_rdr.color = controller.activeColor;
    }

    public void Highlight() {
        overlay_rdr.color = controller.hColor;
    }

    public void Deactive() {
        overlay_rdr.color = controller.deactiveColor;
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
