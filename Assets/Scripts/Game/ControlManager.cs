using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ControlManager : MonoBehaviour {
    public static ControlManager instance;
    public ControlMap mainControl;
    bool draggin;
    Camera cam;
    public Vector2 mpos{
        get{
            var pss = Mouse.current.position.ReadValue();
            return cam.ScreenToWorldPoint(pss);
        }
    }
    void Awake(){
        cam = Camera.main;
        instance = this;
        mainControl = new ControlMap();
        mainControl.Enable();
        mainControl.Player.Click.performed += ctx => {            
            onMouseDown.Invoke(mpos);
        };
    }

    public UnityEvent<Vector2> onMouseDown = new UnityEvent<Vector2>();
}
