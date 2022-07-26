using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public AudioSource music;
    public AudioClip mainClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    public static GameManager instance;
    public TurnController[] controllers;
    public TurnController currentController => controllers[currentControllerIndex];
    int currentControllerIndex;
    public bool gameEnded = false;

    void Awake(){
        music.loop = true;
        music.clip = mainClip;
        instance = this;
    }
    void Start(){
        music.Play();
        GameLoop();
    }
    public async void GameLoop(){
        gameEnded = false;
        foreach(var c in controllers) c.Setup();
        var ui = UI.instance;
        await ui.ShowPanel(0);
        await ui.ShowAnimatedMsg("Game Start");
        currentControllerIndex = 0;
        while(Application.isPlaying && !gameEnded){
            await ui.ShowAnimatedMsg($"{currentController.controllerName}'s Turn");
            await ui.ClosePanel();
            await currentController.PerformTurn();
            currentControllerIndex = (currentControllerIndex + 1) % controllers.Length;
            foreach(var c in controllers) gameEnded |= c.EvaluateLoseCondition();
            if(gameEnded) break;
            await ui.ShowPanel(currentController.teamID);
            await ui.ShowAnimatedMsg("Changing Turn");            
        }
        
        await ui.ClosePanel();
        music.Stop();
        music.loop = false;
        
        if(controllers[0].EvaluateLoseCondition()){
            //SFXPlayer.instance.Play(6);
            music.clip = loseClip;
            music.Play();
            await UI.instance.ShowPermMessage("Game Over.");
        }else{
            //SFXPlayer.instance.Play(5);
            music.clip = winClip;
            music.Play();
            await UI.instance.ShowPermMessage("Victory!");
        }

        await UI.instance.ToggleEndgame();
    }

    public async Task Battle(Unit a, Unit b){
        var dist = a.coord.TileDist(b.coord);
        print($"{a.team}'s {a.attributes.className} attacked {b.attributes.className}");
        await AsyncTweener.Wait(.25f);
        if(a.attributes.range >= dist){
            
            var dmg = await DiceManager.instance.RollD6(a.attributes.atk, a.teamID);
            SFXPlayer.instance.Play(0);
            b.TakeDamage(dmg);
            
            await AsyncTweener.Wait(1);
            if(!b.alive) return;
        }

        if(b.attributes.range >= dist){
            print($"{b.team}'s {b.attributes.className} attacked {a.attributes.className}");
            await AsyncTweener.Wait(.25f);
            var dmg = await DiceManager.instance.RollD6(b.attributes.atk, b.teamID);
            
            SFXPlayer.instance.Play(0);
            a.TakeDamage(dmg);
            await AsyncTweener.Wait(.5f);
        }  
    }
}
