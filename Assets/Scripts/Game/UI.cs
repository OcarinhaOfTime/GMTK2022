using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public static UI instance;
    [SerializeField]TMP_Text msg_txt;
    [SerializeField]CanvasGroup cg;
    [SerializeField]CanvasGroup endgame_cg;
    [SerializeField]Sprite[] curtains;
    [SerializeField]Image msgPanelSR;
    void Awake(){
        instance = this;
        endgame_cg.alpha = cg.alpha = 0;
    }

    public async Task ShowPanel(int curtainID){
        msgPanelSR.sprite = curtains[curtainID];
        msg_txt.color = Color.clear;
        msgPanelSR.color = Color.white;
        await AsyncTweener.Tween(.25f, t => cg.alpha = t);
        await AsyncTweener.Wait(.25f);
    }

    public async Task ClosePanel(){
        await AsyncTweener.Tween(.2f, t => cg.alpha = 1-t);
    }

    public async Task ShowAnimatedMsg(string msg){
        msg_txt.text = msg;
        var mid = Vector2.zero;
        var right = Vector2.right * 150;
        var left = Vector2.right * -150;
        await AsyncTweener.Tween(.35f, t => {
            msg_txt.color = Color.Lerp(Color.clear, Color.white, Mathf.Sqrt(t));
            msg_txt.rectTransform.localPosition = Vector3.Lerp(right, mid, t);
        });
        await AsyncTweener.Wait(1.25f);
        await AsyncTweener.Tween(.25f, t => {
            msg_txt.color = Color.Lerp(Color.white, Color.clear, t);
            msg_txt.rectTransform.localPosition = Vector3.Lerp(mid, left, t);
        });
    }

    public async Task ShowMessage(string msg, int curtainID){
        msgPanelSR.sprite = curtains[curtainID];
        msg_txt.text = msg;
        msgPanelSR.color = Color.white;
        await AsyncTweener.Tween(.25f, t => cg.alpha = t);
        await AsyncTweener.Wait(1.25f);
        await AsyncTweener.Tween(.15f, t => cg.alpha = 1-t);
    }

    public async Task ShowMessage(string msg){
        msgPanelSR.sprite = null;
        msgPanelSR.color = Color.black;
        msg_txt.text = msg;
        await AsyncTweener.Tween(.25f, t => cg.alpha = t);
        await AsyncTweener.Wait(.75f);
        await AsyncTweener.Tween(.15f, t => cg.alpha = 1-t);
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
