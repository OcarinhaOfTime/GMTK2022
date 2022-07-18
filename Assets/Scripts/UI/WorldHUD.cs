using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldHUD : MonoBehaviour {
    public float hp_fill { set => hp_fill_img.fillAmount = value; }
    [SerializeField] Image hp_fill_img;
    public Color hudColor {set => hp_fill_img.color = value;}
}
