using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour {
    [SerializeField]GameObject canvas;
    [SerializeField]TMP_Text[] val_txts;
    [SerializeField]TMP_Text title;
    [SerializeField]BlinkSprite target;

    void Start(){
        canvas.SetActive(false);
        ControlManager.instance.onMouseDown.AddListener(OnClick);
    }

    void OnClick(Vector2 mpos){
        target.t0 = 0;
        target.gameObject.SetActive(true);
        print("Clicked!!!");
        var map = MapController.instance;
        (var x, var y, var b) = map.EvaluateMouse();
        var p = map.map.CoordToWorldPoint(new Coord(x, y));
        target.transform.position = p;
        canvas.SetActive(false);
        if(!b) return;
        var unit = map.map[x, y].unit;
        if(unit == null) return;
        canvas.SetActive(true);
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
