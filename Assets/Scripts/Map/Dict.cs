using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dict<T0, T1> {
    public T1 defaultVal;
    Dictionary<T0, T1> d = new Dictionary<T0, T1>();
    public Dict(){
        this.defaultVal = default;
    }
    public Dict(T1 defaultVal){
        this.defaultVal = defaultVal;
    }
    public T1 this[T0 i]{
        get{
            if(!d.ContainsKey(i)) d.Add(i, defaultVal);
            
            return d[i];
        }set => d[i] = value;
    }
}
