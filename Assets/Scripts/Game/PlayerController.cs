using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerController : TurnController {
    public enum ControlState {
        Idle,
        UnitSelected,
        UnitEngaged,
    }
    public Unit[] units;
    public bool endTurn;
    public ControlState state = ControlState.Idle;
    MapController mapController;
    Vector2 mpos;
    Unit selectedUnit;
    public override void Setup() {
        units = GetComponentsInChildren<Unit>();
        mapController = MapController.instance;
        foreach (var u in units) u.Setup();
        ControlManager.instance.onMouseDown.AddListener(Process);
        CameraController.instance.FocusImmediate(units[0].transform.position);
    }

    public override async Task PerformTurn() {
        foreach (var u in units) u.StartTurn();
        await CameraController.instance.Focus(units[0].transform.position);
        endTurn = false;
        while (!endTurn) {
            endTurn = units.Aggregate(true, (acc, x) => acc && x.hasMoved);
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
        if (endTurn) return;
        switch (state) {
            case ControlState.Idle:
                Idle();
                break;

            case ControlState.UnitSelected:
                UnitSelected();
                break;

            case ControlState.UnitEngaged:
                UnitEngaged();
                break;
        }
    }

    void Idle() {
        foreach (var u in units) {
            if (u.hasMoved) continue;
            var col = u.GetComponent<BoxCollider2D>();
            if (!col.OverlapPoint(mpos)) continue;

            print("We move");
            state = ControlState.UnitSelected;
            mapController.OnClickTile(u.coord.x, u.coord.y, u.attributes.move);
            selectedUnit = u;
        }
    }

    void UnitSelected() {
        state = ControlState.Idle;
        (var x, var y, var b) = mapController.EvaluateMouse();
        mapController.ResetSelection();

        if (!b) return;
        if (!mapController.selectedTiles.Contains((x, y))) return;
        if (!selectedUnit.Move(new Coord(x, y))) return;
        mapController.map.IterQuad(x, y, (t, x1, y1) => {
            if (t.unit != null && t.unit is EnemyUnit) {
                selectedUnit.SetHasMoved(false);
                state = ControlState.UnitEngaged;
            }
        });
    }

    async void UnitEngaged() {
        state = ControlState.Idle;
        selectedUnit.SetHasMoved(true);

        (var x, var y, var b) = mapController.EvaluateMouse();
        if (!b) return;
        var t = mapController.map[x, y];
        if (t.unit != null && t.unit is EnemyUnit) {
            selectedUnit.SetHasMoved(false);
            await GameManager.instance.Battle(selectedUnit, t.unit);
            selectedUnit.SetHasMoved(true);
        }
    }

    public override bool EvaluateLoseCondition() {
        return !units[0].alive;
    }
}
