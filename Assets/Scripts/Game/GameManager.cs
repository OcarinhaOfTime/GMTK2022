using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public TurnController[] controllers;
    public TurnController currentController => controllers[currentControllerIndex];
    int currentControllerIndex;
    void Start(){
        GameLoop();
    }
    public async void GameLoop(){
        foreach(var c in controllers) c.Setup();
        await UI.instance.ShowMessage("Game Start");
        currentControllerIndex = 0;
        while(Application.isPlaying){
            await UI.instance.ShowMessage($"{currentController.controllerName}'s Turn", currentController.controllerColor);
            await currentController.PerformTurn();
            currentControllerIndex = (currentControllerIndex + 1) % controllers.Length;
            await UI.instance.ShowMessage("Changing Turn");
        }
    }
}
