using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GMTK/Character")]
public class CharacterAttributes : ScriptableObject {
    public string className;
    public int hp = 10;
    public int atk = 2;
    public int move = 2;
}
