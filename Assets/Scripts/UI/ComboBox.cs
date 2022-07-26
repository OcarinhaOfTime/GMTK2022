using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ComboBox : MonoBehaviour {
    [SerializeField] Sprite selectedSprite;
    [SerializeField] Sprite unselectedSprite;

    [SerializeField] Button[] btns;
    public int selectedIdx;
    UnityEvent<int> onBtnClicked = new UnityEvent<int>();

    void Awake(){
        for(int i=0; i<btns.Length; i++){
            var idx = i;
            btns[i].onClick.AddListener(() => OnBtnClicked(idx));
        }

        HandleOptns();
    }

    void OnBtnClicked(int i){
        if(i == selectedIdx) return;
        selectedIdx = i;
        HandleOptns();
        onBtnClicked.Invoke(i);
    }

    void HandleOptns(){
        for(int i=0; i<btns.Length; i++){
            var im = btns[i].targetGraphic as Image;
            im.sprite = i == selectedIdx ? selectedSprite : unselectedSprite;
        }
    }
}
