using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyUnit : Unit {
    public enum AIState{
        Idle,
        Patrol,
        Aggressive
    }

    public async Task Evaluate(){
        var map = MapController.instance.map;
        map.Navigate(coord.x, coord.y, attributes.move, 
        (t, x, y) => {}, 
        t => t.const_compound);
        await AsyncTweener.Wait(1f);
        SetHasMoved(true);
    }
}
