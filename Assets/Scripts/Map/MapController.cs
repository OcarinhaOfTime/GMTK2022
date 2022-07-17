using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapController : MonoBehaviour {
    public static MapController instance;
    public Map<Tile> map;
    public Tile prefab;
    public int width = 8;
    public int height = 8;

    void Awake(){
        instance = this;
    }

    void Start(){
        Generate();
    }

    public void Generate() {
        map = new Map<Tile>(width, height, CreateTile);
        map[1, 0].cost_value = 666;
        map[1, 1].cost_value = 666;
        map[1, 2].cost_value = 666;
        map[0, 2].cost_value = 666;
        map[3, 2].cost_value = 2;
        map[4, 2].cost_value = 2;
    }

    Tile CreateTile(int x, int y){
        var inst = Instantiate(prefab);
        inst.transform.SetParent(transform);
        var min = new Vector2(-width/2, -height/2);
        var p = Map<Tile>.CoordToWorldPoint(x, y, width, height);
        inst.transform.localPosition = p;
        inst.name = $"{x}x{y}";
        inst.gameObject.SetActive(true);
        inst.coord = new Coord(x, y);        
        inst.Deactive();
        return inst;
    }
    public HashSet<(int, int)> selectedTiles;

    public void OnClickTile(int x, int y, int k){
        print($"clicked {x}x{y}");
        var tile = map[x, y];
        ResetSelection();
        selectedTiles = map.Navigate(
            x, y, k, (t, x0, y0) => t.Active(), t => t.const_compound);
    }

    public void ResetSelection(){
        map.MapIter((t, _, __) => t.Deactive());
    }

    // public bool isOver;

    // void Update(){
    //     Vector2 p = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
    //     isOver = map.ContainsWorldPosition(p);
    // }

    public (int, int, bool) EvaluateMouse(){
        Vector2 p = ControlManager.instance.mpos;
        var inside = map.ContainsWorldPosition(p);
        if(!inside) return (0, 0, false);

        var c = map.WorldPointToCoord(p);

        return (c.x, c.y, true);
    }
}
