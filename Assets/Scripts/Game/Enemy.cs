using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public enum AIState{
        Idle,
        Patrol,
        Aggressive
    }

    Unit unit;

    public void Setup(){
		unit = GetComponent<Unit>();
        unit.Setup();        
	}

    public void StartTurn(){
        unit.StartTurn();
    }

    public async Task Evaluate(){
        await AsyncTweener.Wait(1f);
        unit.SetHasMoved(true);
    }

    public void EndTurn(){
        unit.EndTurn();
    }
}
