using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyController : TurnController {
    public enum ControlState {
        Idle,
        UnitSelected
    }
    public EnemyUnit[] units;
    public bool endTurn;
    public ControlState state = ControlState.Idle;
    MapController mapController;
    Vector2 mpos;
    Unit selectedUnit;
    public override void Setup() {
        units = GetComponentsInChildren<EnemyUnit>();
        mapController = MapController.instance;
        foreach (var u in units) u.Setup();
        //ControlManager.instance.onMouseDown.AddListener(Process);
    }

    public override async Task PerformTurn() {
        foreach (var u in units) u.StartTurn();
        endTurn = false;
        // while (!endTurn) {
        //     endTurn = units.Aggregate(true, (acc, x) => acc && x.hasMoved);
        //     await Task.Yield();
        // }
        foreach(var u in units){
            await u.Evaluate();
        }

        await AsyncTweener.Wait(.5f);

        foreach (var u in units) u.EndTurn();
        endTurn = true;
    }

    public override bool EvaluateLoseCondition() {
        return units.Aggregate(true, (acc, u) => acc && !u.alive);
    }
}
