using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour {
    [SerializeField]GameObject canvas;
    [SerializeField]TMP_Text[] val_txts;
    [SerializeField]TMP_Text title;

    void Start(){
        ControlManager.instance.onMouseDown.AddListener(OnClick);
    }

    void OnClick(Vector2 mpos){
        print("Clicked!!!");
        var map = MapController.instance;
        (var x, var y, var b) = map.EvaluateMouse();
        if(!b) return;
        var unit = map.map[x, y].unit;
        if(unit == null) return;
        ContextUI(unit);
    }
    public void ContextUI(Unit u){
        print("Contex UI being updated");        
        var atrbs =  u.attributes;
        title.text = atrbs.className;
        var vals = new string[]{
            $"{u.hp}/{u.max_hp}",
            $"{atrbs.atk}",
            $"{atrbs.move}",
            $"{atrbs.range}"
        };

        foreach((var txt, var v) in val_txts.Zip(vals, (a, b) => (a, b))){
            txt.text = v;
        }
    }
}
