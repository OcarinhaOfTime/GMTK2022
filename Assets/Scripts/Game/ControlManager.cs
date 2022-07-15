using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour {
    public static ControlManager instance;
    public ControlMap mainControl;
    bool draggin;
    void Awake(){
        instance = this;
        mainControl = new ControlMap();
        mainControl.Enable();
    }
}
