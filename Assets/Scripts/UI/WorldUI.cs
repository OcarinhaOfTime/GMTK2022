using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class WorldUI : MonoBehaviour {
    public static WorldUI instance;
    public Sprite[] sprites;
    public Button[] btns;
    [SerializeField]GameObject canvas;
    bool waiting = false;
    int clickedButton;

    void Awake(){
        canvas = transform.GetChild(0).gameObject;
        instance = this;
        for(int i=0; i<btns.Length; i++){
            var index = i;
            btns[i].onClick.AddListener(() => OnButtonClicked(index));
            btns[i].gameObject.SetActive(false);
        }
    }

    void ResetButtons(){
        for(int i=0; i<btns.Length; i++){
            btns[i].gameObject.SetActive(false);
        }
    }

    void OnButtonClicked(int i){
        print("We clicked " + i);
        clickedButton = i;
        waiting = false;
    }

    public async Task<int> Evaluate(Vector2Int c, params int[] sprite_is){
        var p = MapController.instance.map.CoordToWorldPoint(c);
        canvas.transform.position = p + Vector2.up * 1.2f;
        canvas.gameObject.SetActive(true);
        waiting = true;
        ResetButtons();
        for(int i=0; i<sprite_is.Length; i++){
            btns[i].gameObject.SetActive(true);
            var im = btns[i].transform.GetChild(0).GetComponent<Image>();
            im.sprite = sprites[sprite_is[i]];
        }
        while(waiting && Application.isPlaying){
            await Task.Yield();
        }
        canvas.gameObject.SetActive(false);
        return clickedButton;
    }

    public async Task Close(){
        clickedButton = -1;
        waiting = false;

        await Task.Yield();
    }
}
