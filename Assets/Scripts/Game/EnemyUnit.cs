using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class EnemyUnit : Unit {
    public enum AIState {
        Idle,
        Patrol,
        Aggressive
    }
    public AIState state = AIState.Idle;
    PlayerController controller;

    public int dist;
    public Unit target;

    public async Task Evaluate(TurnController targetController) {
        controller = (PlayerController)targetController;
        switch(state){
            case AIState.Idle:
            await Idle();
            break;

            case AIState.Patrol:
            break;

            case AIState.Aggressive:
            await Aggressive();
            break;
        }
    }
    async Task Idle(){
        target = GetClosestUnit();
        dist = coord.MDist(target.coord);
        if(dist < 10){
            state = AIState.Aggressive;
            await Aggressive();
        }
    }

    Unit GetClosestUnit(){
        var us = controller.main_units;
        (int d, Unit u) = controller.main_units.Aggregate((999, us[0]), (acc, k) => {
            var dist = coord.MDist(k.coord);
            if(dist < acc.Item1){
                acc.Item1 = dist;
                acc.Item2 = k;
            }

            return acc;
        });
        return u;
    }


    async Task<bool> TryToAttack(){
        target = GetClosestUnit();
        if(coord.MDist(target.coord) <= attributes.range){
            print("We attack");
            await GameManager.instance.Battle(this, target);
            return true;
        }

        return false;
    }

    async Task Aggressive(){
        var target = controller.units[0];
        var b = await TryToAttack();
        if(b) return;

        var map = MapController.instance.map;
        var d = 999f;
        var c = coord;
        map.FloodFill(coord.x, coord.y, attributes.move * 2,
        (t, x, y) => {
            var _d = t.coord.MDist(target.coord);
            if (_d < d) {
                d = _d;
                c = t.coord;
            }
        },
        t => t.const_compound);

        await AsyncTweener.Wait(.5f);
        await Move(c);
        await AsyncTweener.Wait(.5f);
        await TryToAttack();
        SetHasMoved(true);
        dist = coord.MDist(target.coord);
    }
}
