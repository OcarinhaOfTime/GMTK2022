using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollManager : MonoBehaviour {
    public RollEntry roll_d4;
    public RollEntry roll_d6;
    public RollEntry roll_d8;

    void Awake(){
        roll_d4.onClicked.AddListener(() => roll_d4.text = "" +DiceUtils.d4);
        roll_d6.onClicked.AddListener(() => roll_d6.text = "" +DiceUtils.d6);
        roll_d8.onClicked.AddListener(() => roll_d8.text = "" +DiceUtils.d8);
    }
}
