using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour {
    public static HUD instance;
    [SerializeField]GameObject canvas;
    [SerializeField]TMP_Text[] val_txts;
    [SerializeField]TMP_Text title;
    [SerializeField]BlinkSprite target;
    [SerializeField]BlinkSprite atk_target;
    GenericPool<Poolable> targetPool;

    void Awake(){
        instance = this;
        targetPool = new GenericPool<Poolable>();
        targetPool.Setup(8, atk_target.GetComponent<Poolable>(), atk_target.transform.parent);
    }
    void Start(){        
        canvas.SetActive(false);
        ControlManager.instance.onMouseDown.AddListener(OnClick);
    }

    public void ActivateTargets(List<(int, int)> ps){
        foreach((var x, var y) in ps){
            var plb = targetPool.GetPoolable();
            var blink = plb.GetComponent<BlinkSprite>();
            blink.t0 = 0;
            plb.transform.position = MapController.instance.map.CoordToWorldPoint((x, y));
            plb.gameObject.SetActive(true);
        }
    }

    public void DeactivateTargets(){
        targetPool.RecycleAll();
    }

    void OnClick(Vector2 mpos){
        target.t0 = 0;
        target.gameObject.SetActive(true);
        print("Clicked!!!");
        var map = MapController.instance;
        (var x, var y, var b) = map.EvaluateMouse();
        var p = map.map.CoordToWorldPoint((x, y));
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
