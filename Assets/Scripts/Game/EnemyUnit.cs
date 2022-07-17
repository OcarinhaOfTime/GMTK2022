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
        await AsyncTweener.Wait(1f);
        SetHasMoved(true);
    }
}
