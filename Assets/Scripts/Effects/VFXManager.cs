using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour {
    public static VFXManager instance;
    public TileObj health_fx;

    void Awake(){
        instance = this;
    }

    public async void ShowHealFX(Vector2Int p){
        health_fx.pos = p;
        await AsyncTweener.Tween(.25f, t => health_fx.alpha = t);
        await AsyncTweener.Wait(.75f);
        await AsyncTweener.Tween(.25f, t => health_fx.alpha = 1-t);
    }
}
