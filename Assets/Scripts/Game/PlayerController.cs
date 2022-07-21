using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : TurnController {
    public enum ControlState {
        Idle,
        UnitMoving,
        UnitEngaged,
    }
    public Unit[] units;
    public bool endTurn;
    public bool waiting = false;
    public int option = -1;
    public ControlState state = ControlState.Idle;
    MapController mapController;
    Vector2 mpos;
    Unit selectedUnit;
    Unit selectedEnemy;

    public override Unit[] main_units => units.Where(u => u.alive).ToArray();

    public override void Setup() {
        endTurn = true;
        units = GetComponentsInChildren<Unit>();
        mapController = MapController.instance;
        foreach (var u in units) u.Setup();
        var cm = ControlManager.instance;
        cm.onMouseDown.AddListener(Process);
        cm.mainControl.Player.PassTurn.performed += ctx => endTurn = true;
        CameraController.instance.FocusImmediate(units[0].transform.position);
    }

    public override async Task PerformTurn() {
        foreach (var u in units) u.StartTurn();
        await CameraController.instance.Focus(units[0].transform.position);
        endTurn = false;
        while (!endTurn) {
            endTurn = units.Aggregate(true, (acc, x) => acc && (x.hasMoved || !x.alive));
            await Task.Yield();
        }

        foreach (var u in units) u.EndTurn();
        endTurn = true;
    }

    void ProcessTest(Vector2 mpo) {
        (var x, var y, var b) = mapController.EvaluateMouse();
        print($"{x}x{y} {b}");
    }

    void Process(Vector2 mpos) {
        this.mpos = mpos;
        if (endTurn || waiting) return;
        switch (state) {
            case ControlState.Idle:
                Idle();
                break;

            case ControlState.UnitMoving:
                UnitSelected();
                break;

            case ControlState.UnitEngaged:
                UnitEngaged();
                break;
        }
    }

    async void Idle() {
        await ProcessMap();
    }

    async Task ProcessMap(){
        WorldUI.instance.Close();
        (var x, var y, var b) = mapController.EvaluateMouse();
        if(!b) return;
        var u = mapController.map[x, y].unit;
        selectedUnit = u;
        if(u == null || u.hasMoved || u is EnemyUnit){
            return;
        }

        
        option = await WorldUI.instance.Evaluate(u.coord, 0, 2);           

        if(option == 0){
            state = ControlState.UnitMoving;
            var m = await DiceManager.instance.RollD6(u.attributes.move, 0);
            mapController.OnClickTile(u.coord.x, u.coord.y, m);
        } 
    }

    void UnitSelected() {
        (var x, var y, var b) = mapController.EvaluateMouse();
        if (!b) return;        
        if (!mapController.selectedTiles.Contains((x, y))) return;

        mapController.ResetSelection();
        
        state = ControlState.Idle;
        selectedUnit.SetHasMoved(true);
        if (!selectedUnit.Move(new Coord(x, y))) return;

        mapController.map.Navigate(x, y, selectedUnit.attributes.range,
         (t, x1, y1) => {
            if (t.unit != null && t.unit is EnemyUnit) {
                selectedEnemy = t.unit;
                selectedUnit.SetHasMoved(false);
                Battle();
            }
        }, t => 1);
    }

    async void Battle(){
        waiting = true;
        option = await WorldUI.instance.Evaluate(selectedUnit.coord, 1, 2);
        waiting = false;
        if(option == 0){
            selectedUnit.SetHasMoved(false);
            await GameManager.instance.Battle(selectedUnit, selectedEnemy);
            selectedUnit.SetHasMoved(true);
        }else{
            selectedUnit.SetHasMoved(true);
        }
    }

    async void UnitEngaged() {
        state = ControlState.Idle;
        selectedUnit.SetHasMoved(true);

        // (var x, var y, var b) = mapController.EvaluateMouse();
        // if (!b) return;

        
        
    //     var t = mapController.map[x, y];
    //     if (t.unit != null && t.unit is EnemyUnit) {
    //         selectedUnit.SetHasMoved(false);
    //         await GameManager.instance.Battle(selectedUnit, selectedEnemy);
    //         selectedUnit.SetHasMoved(true);
    //     }
    }

    public override bool EvaluateLoseCondition() {
        return !units[0].alive;
    }

   protected override void OnUnitDeath(Unit u) {
        if(EvaluateLoseCondition()){
            endTurn = true;
        }
    }
}
