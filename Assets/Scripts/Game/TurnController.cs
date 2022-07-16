using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public abstract class TurnController : MonoBehaviour {
    public string controllerName = "Player";
    public Color controllerColor = Color.white;
    public virtual void Setup(){
        
    }
    public virtual async Task PerformTurn(){
        await Task.Yield();
    }
}
