using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour {
    public static UI instance;
    [SerializeField]TMP_Text msg_txt;
    [SerializeField]CanvasGroup cg;
    [SerializeField]CanvasGroup endgame_cg;
    void Awake(){
        instance = this;
        endgame_cg.alpha = cg.alpha = 0;
    }

    public async Task ShowMessage(string msg, Color color){
        msg_txt.color = color;
        msg_txt.text = msg;
        await AsyncTweener.Tween(.25f, t => cg.alpha = t);
        await AsyncTweener.Wait(.75f);
        await AsyncTweener.Tween(.15f, t => cg.alpha = 1-t);
    }

    public async Task ShowMessage(string msg){
        await ShowMessage(msg, Color.white);
    }

    public async Task ShowPermMessage(string msg){
        msg_txt.color = Color.white;
        msg_txt.text = msg;
        await AsyncTweener.Tween(.25f, t => cg.alpha = t);
        await AsyncTweener.Wait(.75f);
        //await AsyncTweener.Tween(.25f, t => cg.alpha = 1-t);
    }
    public async Task ToggleEndgame(){
        await AsyncTweener.Tween(.5f, t => endgame_cg.alpha = t);
    }
}
