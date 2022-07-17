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

    public int dist;
    async Task Idle(){
        target = GetClosestUnit();
        dist = coord.TileDist(target.coord);
        if(dist < 5){
            state = AIState.Aggressive;
            await Aggressive();
        }
    }

    Unit GetClosestUnit(){
        var us = controller.main_units;
        (int d, Unit u) = controller.main_units.Aggregate((999, us[0]), (acc, k) => {
            var dist = coord.TileDist(k.coord);
            if(dist < acc.Item1){
                acc.Item1 = dist;
                acc.Item2 = k;
            }

            return acc;
        });
        return u;
    }

    public Unit target;

    async Task<bool> TryToAttack(){
        target = GetClosestUnit();
        if(coord.TileDist(target.coord) <= 1){
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
        map.Navigate(coord.x, coord.y, attributes.move * 2,
        (t, x, y) => {
            var _d = t.coord.TileDist(target.coord);
            if (_d < d) {
                d = _d;
                c = t.coord;
            }
        },
        t => t.const_compound);

        await AsyncTweener.Wait(.5f);
        Move(c);
        await AsyncTweener.Wait(.5f);
        await TryToAttack();
        SetHasMoved(true);
        dist = coord.TileDist(target.coord);
    }
}
