using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlayer : MonoBehaviour {
    public static SFXPlayer instance;
    public AudioClip[] clips;
    AudioSource src;

    void Awake(){
        instance = this;
        src = GetComponent<AudioSource>();
    }

    public void Play(int k){
        src.PlayOneShot(clips[k]);
    }
}
