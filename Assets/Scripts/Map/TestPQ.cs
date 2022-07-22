using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPQ : MonoBehaviour {
    [ContextMenu("Test")]
    void Test(){
        PriorityQueue<(int, int)> pq = new PriorityQueue<(int, int)>();
        pq.Enqueue((0, 0), 1);
        pq.Enqueue((0, 2), 0);
        pq.Enqueue((0, 3), 5);
        pq.Enqueue((4, 0), 3);

        foreach((var x, var y) in pq){
            print($"{x}x{y}");
        }
    }
}
