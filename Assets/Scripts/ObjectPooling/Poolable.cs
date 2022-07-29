using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Poolable : MonoBehaviour {
    public int poolIndex { get; set; }
    public virtual void OnRecycle(){
        
    }
    private Action<int> recycle_fn;
    public virtual void Setup(Action<int> recycle_fn){
        this.recycle_fn = recycle_fn;
    }

    public void Recycle(){
        recycle_fn(poolIndex);
    }
}
