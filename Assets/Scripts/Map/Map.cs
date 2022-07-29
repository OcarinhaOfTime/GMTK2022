using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Wraps a map structure
/// </summary>
/// <typeparam name="T"></typeparam>
public class Map<T> : IEnumerable<T> {
    public int width { get { return map.GetLength(0); } }
    public int height { get { return map.GetLength(1); } }
    T[,] map;

    public Map(int x, int y) {
        map = new T[x, y];
    }

    public Map(int x, int y, T defaultValue) {
        map = new T[x, y];
        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                map[i, j] = defaultValue;
            }
        }
    }

    public Map(int x, int y, Func<int, int, T> fn) {
        map = new T[x, y];
        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++) {
                map[i, j] = fn(i, j);
            }
        }
    }

    public Map(T[,] map) {
        this.map = map;
    }

    public T this[int x, int y] {
        get {
            return map[x, y];
        }
        set {
            map[x, y] = value;
        }
    }

    public T this[(int, int) c] {
        get {
            return map[c.Item1, c.Item2];
        }
        set {
            map[c.Item1, c.Item2] = value;
        }
    }

    public T this[Vector2Int coord] {
        get {
            return map[coord.x, coord.y];
        }
        set {
            map[coord.x, coord.y] = value;
        }
    }


    public Vector2 CoordToWorldPoint(Vector2Int tile) {
        return new Vector2(-width / 2 + .5f + tile.x, -height / 2 + .5f + tile.y);
    }

    public Vector2 CoordToWorldPoint((int, int) c) {
        (var x, var y) = c;
        return new Vector2(-width / 2 + .5f + x, -height / 2 + .5f + y);
    }

    public static Vector2 CoordToWorldPoint(int x, int y, int w, int h) {
        return new Vector2(-w / 2 + .5f + x, -h / 2 + .5f + y);
    }

    public Vector2Int WorldPointToCoord(Vector2 pos) {
        return new Vector2Int(Mathf.FloorToInt(pos.x + width / 2), Mathf.FloorToInt(pos.y + height / 2));
    }

    public bool ContainsWorldPosition(Vector2 pos){
        return pos.x > -width / 2 && pos.x < width / 2 &&  pos.y > -height / 2 && pos.y < height / 2;
    }

    public IEnumerator<T> GetEnumerator() {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < width; y++) {
                yield return map[x, y];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator() {
        for(int x = 0; x < width; x++) {
            for(int y = 0; y < width; y++) {
                yield return map[x, y];
            }
        }
    }

    public void IterQuad(int x0, int y0, Action<T, int, int> onEach){
        var points = new (int, int)[4]{
            (x0+1, y0),
            (x0, y0+1),
            (x0-1, y0),
            (x0, y0-1)
        };

        foreach((var x, var y) in points){
            if(map.IsInMapRange(x, y)) onEach(map[x, y], x, y);
        }        
    }

    public void IterQuad(Vector2Int v, Action<T, Vector2Int> onEach){
        var points = new Vector2Int[4]{
            v+Vector2Int.right,
            v+Vector2Int.up,
            v+Vector2Int.left,
            v+Vector2Int.down
        };

        foreach(var p in points){
            if(map.IsInMapRange(p)) onEach(this[p], p);
        }        
    }

    public void IterQuad((int , int) c, Action<T, int, int> onEach){
        (var x0, var y0) = c;
        var points = new (int, int)[4]{
            (x0+1, y0),
            (x0, y0+1),
            (x0-1, y0),
            (x0, y0-1)
        };

        foreach((var x, var y) in points){
            if(map.IsInMapRange(x, y)) onEach(map[x, y], x, y);
        }        
    }

    Stack<Vector2Int> ReconstructPath(
        Dict<Vector2Int, Vector2Int> origin, Vector2Int s, Vector2Int e){
        var path = new Stack<Vector2Int>();
        var i = e;
        while(i != s){
            path.Push(i);
            i = origin[i];
        }
        //path.Push(s);
        return path;
    }

    public Stack<Vector2Int> AStar(Vector2Int s, Vector2Int e, Func<T, int> cost_fn){
        PriorityQueue<Vector2Int> openSet = new PriorityQueue<Vector2Int>();
        var closedMap = new Dict<Vector2Int, bool>(false);
        var origin  = new Dict<Vector2Int, Vector2Int>();
        var gScore = new Dict<Vector2Int, float>(Mathf.Infinity);
        var fScore = new Dict<Vector2Int, float>(Mathf.Infinity);

        gScore[s] = 0;
        fScore[s] = s.MDist(e);
        openSet.Enqueue(s, fScore[s]);

        while(!openSet.Empty){
            var c = openSet.Dequeue();
            closedMap[c] = true;

            if(c == e) return ReconstructPath(origin, s, e);

            IterQuad(c, (t, p) => {
                if(closedMap[p]) return;
                float g = gScore[c] + cost_fn(t);
                if(g < gScore[p]){
                    origin[p] = c;
                    gScore[p] = g;
                    fScore[p] = g + p.MDist(e);
                }
                openSet.Enqueue(p, fScore[p]);
            });            
        }

        throw new Exception("No path found");
    }

    
    public HashSet<(int, int)> FloodFill(int x0, int y0, int k, Action<T, int, int> fn, Func<T, int> cost_fn){
        HashSet<(int, int)> mem = new HashSet<(int, int)>();
        Queue<(int, int, int)> q = new Queue<(int, int, int)>();
        mem.Clear();
        mem.Add((x0, y0));
        fn(map[x0, y0], x0, y0);
        IterQuad(x0, y0, (t, x, y) => q.Enqueue((x, y, k-cost_fn(t))));
        while(q.Count > 0){
            (var x, var y, var k0) = q.Dequeue();
            if(mem.Contains((x, y)) || k0 < 0) continue;
            
            mem.Add((x, y));
            fn(map[x, y], x, y);
            IterQuad(x, y, (t, x_, y_) => q.Enqueue((x_, y_, k0-cost_fn(t))));
        }

        return mem;
    }


    public void MapNeighborIter(Vector2Int tile, Action<T, int, int> onEach) {
        for(int x = tile.x - 1; x <= tile.x + 1; x++) {
            for(int y = tile.y - 1; y <= tile.y + 1; y++) {
                if(map.IsInMapRange(x, y) && (y == tile.y || x == tile.x)) {
                    onEach.Invoke(map[x, y], x, y);
                }
            }
        }
    }

    public void MapNeighborIter(Vector2Int tile, Action<T, Vector2Int> onEach) {
        for(int x = tile.x - 1; x <= tile.x + 1; x++) {
            for(int y = tile.y - 1; y <= tile.y + 1; y++) {
                if(map.IsInMapRange(x, y) && (y == tile.y || x == tile.x)) {
                    onEach.Invoke(map[x, y], new Vector2Int(x, y));
                }
            }
        }
    }

    public int ObstacleInNeighborHood(Vector2Int tile, T obstacle) {
        int obstacles = 0;
        for(int x = tile.x - 1; x <= tile.x + 1; x++) {
            for(int y = tile.y - 1; y <= tile.y + 1; y++) {
                if(map.IsInMapRange(x, y)) {
                    obstacles += ((map[x, y].Equals(obstacle)) ? 1 : 0);
                }
            }
        }

        return obstacles;
    }

    public void MapIter(Action<T, int, int> onEach) {
        for(int x = 0; x < map.GetLength(0); x++) {
            for(int y = 0; y < map.GetLength(1); y++) {
                onEach.Invoke(map[x, y], x, y);
            }
        }
    }

    public bool IsInMapRange(int x, int y) {
        return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
    }
}

public static class MapExtention {
    public static bool IsInMapRange<T>(this T[,] map, int x, int y) {
        return x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1);
    }

    public static bool IsInMapRange<T>(this T[,] map, Vector2Int p) {
        return map.IsInMapRange(p.x, p.y);
    }
}