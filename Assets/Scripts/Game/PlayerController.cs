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
    public List<Vector2Int> reachableTiles = new List<Vector2Int>();

    public override Unit[] main_units => units.Where(u => u.alive).ToArray();

    public override void Setup() {
        endTurn = true;
        units = GetComponentsInChildren<Unit>();
        mapController = MapController.instance;
        foreach (var u in units) u.Setup();
        var cm = ControlManager.instance;
        cm.onMouseDown.AddListener(async (k) => await Process(k));
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

    async Task Process(Vector2 mpos) {
        this.mpos = mpos;
        if (endTurn || waiting) return;
        switch (state) {
            case ControlState.Idle:
                Idle();
                break;

            case ControlState.UnitMoving:
                await UnitSelected();
                break;

            case ControlState.UnitEngaged:
                UnitEngaged();
                break;
        }
    }

    async void Idle() {
        await ProcessMap();
    }

    public HashSet<(int, int)> selectedTiles;

    public void SelectUnitInMap(int x, int y, int k, Unit u){
        print($"clicked {x}x{y}");
        var map = mapController.map;
        var tile = map[x, y];
        mapController.ResetSelection();
        selectedTiles = map.FloodFillAtk(
            x, y, k, (t, x0, y0) => t.Active(), t => t.const_compound,
            u.attributes.range, (t, x, y) => {});
    }

    async Task ProcessMap(){
        await WorldUI.instance.Close();
        (var x, var y, var b) = mapController.EvaluateMouse();
        if(!b) return;
        var u = mapController.map[x, y].unit;
        selectedUnit = u;
        if(u == null || u.hasMoved || u is EnemyUnit){
            return;
        }
        
        option = await WorldUI.instance.Evaluate(u.coord, 0, 2);           

        if(option == 0){
            print("We moving " + u.attributes.className);
            state = ControlState.UnitMoving;
            var m = await DiceManager.instance.RollD6(u.attributes.move, 0, 1);
            SelectUnitInMap(u.coord.x, u.coord.y, m, u);
        } 
    }

    async Task UnitSelected() {
        (var x, var y, var b) = mapController.EvaluateMouse();
        if (!b) return;        
        if (!selectedTiles.Contains((x, y))) return;

        mapController.ResetSelection();
        
        state = ControlState.Idle;
        selectedUnit.SetHasMoved(true);
        var moved = await selectedUnit.Move(new Vector2Int(x, y));
        if (!moved) return;

        reachableTiles.Clear();
        if(selectedUnit.attributes.className != "Cleric"){
            reachableTiles = GetReachableTiles(x, y);
            if(reachableTiles.Count <= 0) return;
            HUD.instance.ActivateTargets(reachableTiles);
        }else{
            reachableTiles = GetReachableTilesCleric(x, y);
            if(reachableTiles.Count <= 0) return;
            HUD.instance.ActivateTargets(reachableTiles, Color.green);
        }
        

        
        state = ControlState.UnitEngaged;

        //selectedEnemy = t.unit;
        //selectedUnit.SetHasMoved(false);
        //Battle();
    }    

    List<Vector2Int> GetReachableTiles(int x, int y){
        var rt = new List<Vector2Int>();
        mapController.map.FloodFill(x, y, selectedUnit.attributes.range,
         (t, x1, y1) => {
            if (t.unit != null && t.unit is EnemyUnit) {
                rt.Add(t.unit.coord);                
            }
        }, t => 1);

        return rt;
    }

    List<Vector2Int> GetReachableTilesCleric(int x, int y){
        var rt = new List<Vector2Int>();
        mapController.map.FloodFill(x, y, selectedUnit.attributes.range,
         (t, x1, y1) => {
            if (t.unit != null && t.unit is PlayerUnit) {
                rt.Add(t.unit.coord);                
            }
        }, t => 1);

        return rt;
    }

    async void UnitEngaged() {
        HUD.instance.DeactivateTargets();
        state = ControlState.Idle;
        selectedUnit.SetHasMoved(true);

        (var x, var y, var b) = mapController.EvaluateMouse();
        if (!b) return;
        if(!reachableTiles.Contains(new Vector2Int(x, y))) return;

        if(selectedUnit.attributes.className != "Cleric"){
            selectedEnemy = mapController.map[x, y].unit;
            waiting = true;
            selectedUnit.SetHasMoved(false);
            await GameManager.instance.Battle(selectedUnit, selectedEnemy);
        }else{
            var sunit = mapController.map[x, y].unit;
            waiting = true;
            var h = await DiceManager.instance.RollD6(1, 0, 1);
            sunit.Heal(h);
            selectedUnit.SetHasMoved(false);
        }

        
        selectedUnit.SetHasMoved(true);
        waiting = false;
    }

    public override bool EvaluateLoseCondition() {
        return !units[0].alive;
    }

   protected override void OnUnitDeath(Unit u) {
        if(EvaluateLoseCondition()){
            endTurn = true;
        }
    }

    // async void Battle(){
    //     waiting = true;
    //     option = await WorldUI.instance.Evaluate(selectedUnit.coord, 1, 2);
    //     waiting = false;
    //     if(option == 0){
    //         selectedUnit.SetHasMoved(false);
    //         await GameManager.instance.Battle(selectedUnit, selectedEnemy);
    //         selectedUnit.SetHasMoved(true);
    //     }else{
    //         selectedUnit.SetHasMoved(true);
    //     }
    // }
}
