using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPlayer : MonoBehaviour {
    public enum Anims {
        Idle = 0,
        Victory = 1,
        Item = 2,
        Dead = 3,
        Attack = 4,
        Walk = 5,
    }
    public enum Dirs {
        Down = 0,
        Right = 1,
        Left = 2,
        Up = 3,
    }
    public Anims state = Anims.Idle;
    [SerializeField] Sprite[] spriteSheet;
    [SerializeField] Sprite[] spriteSheetWalk;
    public int fps = 12;
    float frame_length = 1 / 12;
    SpriteRenderer sr;
    float t = 0;
    int frame_tick;
    Action onFrameUpdate;
    Action[] anim_fns;
    public Dirs facing = Dirs.Down;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        onFrameUpdate = Idle;
        anim_fns = new Action[6]{
            Idle,
            Victory,
            Item,
            Dead,
            Attack,
            Walk,
        };
    }

    void Update() {
        frame_length = 1f / fps;
        t += Time.deltaTime;
        if (t > frame_length) {
            t = 0;
            onFrameUpdate();
            frame_tick++;
        }
    }

    void OnValidate() {
        if(!Application.isPlaying || onFrameUpdate == null) return;
        onFrameUpdate = anim_fns[(int)state];
        frame_tick = 0;
        t = 666;
    }

    void Idle() {
        sr.sprite = spriteSheet[frame_tick % 2];
    }

    void Victory() {
        sr.sprite = spriteSheet[2 + frame_tick % 2];
    }

    void Item() {
        sr.sprite = spriteSheet[4 + frame_tick % 2];
    }

    void Dead() {
        sr.sprite = spriteSheet[Mathf.Min(6 + frame_tick, 8)];
    }

    void Attack() {
        sr.sprite = spriteSheet[9 + frame_tick % 2];
    }

    void Walk(){
        sr.sprite = spriteSheetWalk[(int)facing*3 + frame_tick %2];
    }
}
