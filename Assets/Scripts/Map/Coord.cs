using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// [System.Serializable]
// public class Coord {
//     public int x;
//     public int y;

//     public Coord() {
//     }

//     public Coord(int x, int y) {
//         this.x = x;
//         this.y = y;
//     }

//     public float DistSqrt(Coord other) {
//         return Mathf.Pow(x - other.x, 2) + Mathf.Pow(y - other.y, 2);
//     }

//     public float Dist(Coord other) {
//         return Mathf.Sqrt(DistSqrt(other));
//     }

//     public int TileDist(Coord other) {
//         return Mathf.Abs(x - other.x) + Mathf.Abs(y - other.y);
//     }

//     public bool Equals(Coord other) {
//         return x == other.x && y == other.y;
//     }

//     public override string ToString() {
//         return "{ " + x + ", " + y + " }";
//     }

//     public static implicit operator (int, int)(Coord c){
//         return (c.x, c.y);
//     }

//     public static implicit operator Coord((int, int) c){
//         return new Coord(c.Item1, c.Item2);
//     }
// }