using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTester : MonoBehaviour {
    [Range(1, 20)]
    public int k = 1;
    MapController mapController;
    void Start(){
        ControlManager.instance.onMouseDown.AddListener(OnClick);
        mapController = MapController.instance;
    }

    void OnClick(Vector2 mpos){
        mapController.ResetSelection();
        (var x, var y, var b) = mapController.EvaluateMouse();
        if(!b) return;
        mapController.map[x, y].Active();
        mapController.map.FloodFill(x, y, k, (t, x0, y0) => {
            t.Active();
        });
    }
}
