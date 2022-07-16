using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
	public int width = 5;
	public int height = 10;
	public Transform top_left;
	public Transform bottom_right;
	public GameObject prefab;
	public Map<Tile> map;
	private Unit currentPlayer;
	private List<Tile> tiles = new List<Tile>();

	public List<Unit> players;

	void Awake () {
		map = new Map<Tile>(width, height);
		for(int i=0; i<width; i++){
			for(int j=0; j<height; j++){
				var x = Mathf.Lerp(top_left.position.x, bottom_right.position.x, i / (float)(width - 1));
				var y = Mathf.Lerp(top_left.position.y, bottom_right.position.y, j / (float)(height - 1));
				var inst = Instantiate(prefab);
				inst.transform.position = new Vector2(x, y);
				inst.transform.SetParent(transform);
				var tile = inst.GetComponent<Tile>();
				tile.coord = new Coord(i, j);
				//tile.mapManager = this;

				inst.SetActive(true);
				map[i, j] = tile;
			}
		}
	}

	// public void OnTileSelected(Tile tile){
	// 	if(tile.active){
	// 		if(tile.player != null && tile.player != currentPlayer){
	// 			players.Remove(tile.player);
	// 			tile.player.Die();

	// 			foreach(var t in tiles){
	// 				t.Deactive();
	// 			}
	// 			currentPlayer.active = false;
	// 			tiles.Clear();
	// 			return;
	// 		}
	// 		map[currentPlayer.coord].player = null;
	// 		currentPlayer.coord = tile.coord;
	// 		currentPlayer.transform.position = tile.transform.position + Vector3.forward * -1;
	// 		currentPlayer.active = false;
	// 		tile.player = currentPlayer;
	// 		foreach(var t in tiles){
	// 			t.Deactive();
	// 		}

	// 		tiles.Clear();
	// 		foreach(var p in players)
	// 			p.SetCollider(true);
	// 	}
	// }

	void GetNeighbors(Tile tile){
		foreach(var t in tiles){
			t.Deactive();
		}

		tiles.Clear();

		tiles.Add(tile);

		if(map.IsInMapRange(tile.x+1, tile.y)){
			tiles.Add(map[tile.x+1, tile.y]);
		}
		if(map.IsInMapRange(tile.x-1, tile.y)){
			tiles.Add(map[tile.x-1, tile.y]);
		}
		if(map.IsInMapRange(tile.x, tile.y+1)){
			tiles.Add(map[tile.x, tile.y+1]);
		}
		if(map.IsInMapRange(tile.x, tile.y-1)){
			tiles.Add(map[tile.x, tile.y-1]);
		}
	}

	public void OnPlayerSelected(Unit player){
		currentPlayer = player;
		if(!player.active){
			player.active = true;
			GetNeighbors(map[player.coord]);

			foreach(var tile in tiles){
				tile.Active();
			}
			foreach(var p in players)
				p.SetCollider(false);
		}else{
			player.active = false;
			foreach(var tile in tiles){
				tile.Deactive();
			}
		}
	}
}
