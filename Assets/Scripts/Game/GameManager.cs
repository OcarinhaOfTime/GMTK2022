using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;
    public TurnController[] controllers;
    public TurnController currentController => controllers[currentControllerIndex];
    int currentControllerIndex;
    public bool gameEnded = false;

    void Awake(){
        instance = this;
    }
    void Start(){
        GameLoop();
    }
    public async void GameLoop(){
        gameEnded = false;
        foreach(var c in controllers) c.Setup();
        await UI.instance.ShowMessage("Game Start");
        currentControllerIndex = 0;
        while(Application.isPlaying && !gameEnded){
            await UI.instance.ShowMessage($"{currentController.controllerName}'s Turn", currentController.controllerColor);
            await currentController.PerformTurn();
            currentControllerIndex = (currentControllerIndex + 1) % controllers.Length;
            foreach(var c in controllers) gameEnded |= c.EvaluateLoseCondition();
            if(gameEnded) break;
            await UI.instance.ShowMessage("Changing Turn");

            
        }

        if(controllers[0].EvaluateLoseCondition()){
            await UI.instance.ShowPermMessage("Game Over.");
        }else{
            await UI.instance.ShowPermMessage("Victory!");
        }

        await UI.instance.ToggleEndgame();
    }

    public async Task Battle(Unit a, Unit b){
        print($"{a.team}'s {a.attributes.className} attacked {b.attributes.className}");
        await AsyncTweener.Wait(.25f);
        var dmg = await DiceManager.instance.RollD6(a.attributes.atk, a.teamID);
        b.TakeDamage(dmg);
        await AsyncTweener.Wait(1);
        if(!b.alive) return;
        print($"{b.team}'s {b.attributes.className} attacked {a.attributes.className}");
        await AsyncTweener.Wait(.25f);
        dmg = await DiceManager.instance.RollD6(b.attributes.atk, b.teamID);
        a.TakeDamage(dmg);
        await AsyncTweener.Wait(.5f);
    }
}
