using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GMTK/UnitData")]
public class UnitAttributes : ScriptableObject {
    public string className;
    public int hp = 10;
    public int atk = 2;
    public int move = 2;
    public Sprite sprite;
}
