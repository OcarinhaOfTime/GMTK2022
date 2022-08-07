using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceManager : MonoBehaviour {    
    public static DiceManager instance;
    public Texture2D[] sprites;
    public DiceAnim heroPrefab;
    public DiceAnim villainPrefab;
    DiceAnim[] hdices;
    DiceAnim[] vdices;
    public int ndices = 4;
    [SerializeField]TMP_Text result_txt;
    [SerializeField]CanvasGroup result_cg;
    [SerializeField]Sprite[] curtain_sprites;
    [SerializeField]CanvasGroup curtain_cg;
    [SerializeField]Image dice_curtain;
    public float spacing = 3;
    bool working = false;
    bool clicked_out;
    public int cheat_roll = 7;

    void Awake(){
        instance = this;
        hdices = new DiceAnim[ndices];
        vdices = new DiceAnim[ndices];
        for(int i=0; i<ndices; i++){
            hdices[i] = Instantiate(heroPrefab);
            hdices[i].transform.SetParent(transform);
            hdices[i].Setup();
            hdices[i].gameObject.SetActive(false);

            vdices[i] = Instantiate(villainPrefab);
            vdices[i].transform.SetParent(transform);
            vdices[i].Setup();
            vdices[i].gameObject.SetActive(false);
        }
    }

    // void Start(){
    //     ControlManager.instance.mainControl.Player.Click.performed += 
    //     ctx => Test();
    // }

    // [Range(1, 4)]public int test_k = 1;
    // [ContextMenu("Test")]
    // public async void Test(){
    //     await RollD6(test_k, hdices);
    // }

    // public async Task<int> RollD6Hero(int k){
    //     var r = await RollD6(k, hdices);
    //     return r;
    // }

    // public async Task<int> RollD6Villain(int k){
    //     return await RollD6(k, vdices);
    // }

    public async Task<int> RollD6(int k, int team, int typ=0){
        dice_curtain.sprite = curtain_sprites[typ];
        if(team == 0){
            var r = await RollD6(k, hdices);
            return r;
        }

        return await RollD6(k, vdices);
    }

    public async Task<int> RollD6(int k, DiceAnim[] dices){
        if(working) return 0;
        working = true;
        DisableDices();
        result_cg.alpha = 0;
        var rs = DiceUtils.d6x(k);
        var r = rs.Sum();
        result_txt.text = "" + r;
        List<Task> tsks = new List<Task>();
        Vector3 origin = -Vector3.right * spacing * (rs.Length - 1) / 2;

        await AsyncTweener.Tween(.2f, t => curtain_cg.alpha = t);

        for(int i=0; i<rs.Length; i++){
            var d = dices[i];
            d.ResetPos();
            d.gameObject.SetActive(true);
            d.tex = sprites[rs[i]-1];
            d.alpha = 0;
            d.transform.localPosition = origin + Vector3.right * i * spacing;

            tsks.Append(d.Anim());
            //await d.Anim();
        }
        //await Task.all(tsks);
        await Task.WhenAll(tsks);
        await AsyncTweener.Wait(0.75f);
        await AsyncTweener.Tween(.2f, t =>  result_cg.alpha = t);
        await AsyncTweener.Wait(.6f);
        await AsyncTweener.Tween(.2f, t =>  result_cg.alpha = 1-t);
        await AsyncTweener.Tween(.1f, t => curtain_cg.alpha = 1-t);
        DisableDices();
        working = false;
        return r;
    }

    void DisableDices(){
        foreach(var d in hdices){
            d.alpha = 0;
            d.gameObject.SetActive(false);
        } 

        foreach(var d in vdices){
            d.alpha = 0;
            d.gameObject.SetActive(false);
        } 
    }
}
