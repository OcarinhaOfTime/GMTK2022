using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour {
    Map<Tile> map;
    public Tile prefab;
    public int width = 8;
    public int height = 8;

    // void Start(){
    //     Generate();
    // }

    // public void Generate() {
    //     map = new Map<Tile>(width, height, CreateTile);
    //     map[1, 0].cost_value = 666;
    //     map[1, 1].cost_value = 666;
    //     map[1, 2].cost_value = 666;
    //     map[0, 2].cost_value = 666;
    //     map[3, 2].cost_value = 2;
    //     map[4, 2].cost_value = 2;
    // }

    // Tile CreateTile(int x, int y){
    //     var inst = Instantiate(prefab);
    //     inst.transform.SetParent(transform);
    //     var min = new Vector2(-width/2, -height/2);
    //     var p = min + Vector2.one * new Vector2(x, y);
    //     inst.transform.localPosition = p;
    //     inst.name = $"{x}x{y}";
    //     inst.gameObject.SetActive(true);
    //     inst.coord = new Coord(x, y);        
    //     inst.Deactive();
        
    //     var clickable = inst.GetComponent<IClickable>();
    //     clickable.onClick.AddListener(() => OnClickTitle(x, y));
    //     return inst;
    // }
    // [Range(1, 5)]
    // public int k = 1;

    // void OnClickTitle(int x, int y){
    //     print($"clicked {x}x{y}");
    //     var tile = map[x, y];
    //     map.MapIter((t, _, __) => t.Deactive());
    //     map.Navigate(x, y, k, (t, x0, y0) => t.Active(), t => t.cost);
    // }
}
