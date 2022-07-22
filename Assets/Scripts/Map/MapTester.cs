using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTester : MonoBehaviour {
    [Range(1, 20)]
    public int k = 1;
    MapController mapController;
    void Start(){
        
        mapController = MapController.instance;
        ControlManager.instance.mainControl.Player.Click.performed += 
        ctx => OnClick();
        ControlManager.instance.mainControl.Player.RightClick.performed += 
        ctx => OnRightClick();
    }
    (int, int) selected;
    void OnClick(){
        path = new Stack<(int, int)>();
        mapController.ResetSelection();
        (var x, var y, var b) = mapController.EvaluateMouse();
        if(!b) return;
        selected = (x, y);
        mapController.map[x, y].Active();
        mapController.map.FloodFill(x, y, k, (t, x0, y0) => {
            t.Active();
        }, t=> t.const_compound);

        // mapController.map.MapNeighborIter(new Coord(x, y), (t, x0, y0) => {
        //     t.Active();
        // });
    }

    Stack<(int, int)> path = new Stack<(int, int)>();

    void OnRightClick(){
        (var x, var y, var b) = mapController.EvaluateMouse();
        if(!b || selected == (x, y)) return;
        IterPath(t => t.Active());
        path = mapController.map.AStar(selected, (x, y), 
        t => t.const_compound);
        IterPath(t => t.Highlight());
    }

    void IterPath(Action<Tile> fn){
        foreach(var c in path){
            fn(mapController.map[c]);
        }
    }
}
