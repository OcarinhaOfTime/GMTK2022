using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class TurnController : MonoBehaviour {
    public string controllerName = "Player";
    public Color controllerColor = Color.white;
    public int teamID = 0;
    public virtual void Setup(){
        
    }
    public virtual async Task PerformTurn(){
        await Task.Yield();
    }

    public virtual bool EvaluateLoseCondition(){
        return false;
    }
}
