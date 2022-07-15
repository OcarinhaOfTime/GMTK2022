using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseDragger : MonoBehaviour {
    public bool draggin = true;
    BoxCollider2D col;
    Camera cam;
    Vector3 last_mpos;
    void Start(){
        cam = Camera.main;
        col = GetComponent<BoxCollider2D>();
        var control = ControlManager.instance.mainControl;
        control.Player.Click.started += ctx => OnMouseDown();
        control.Player.Click.canceled += ctx => draggin = false;
    }
    void OnMouseDown(){
        var pss = Mouse.current.position.ReadValue();
        var p = cam.ScreenToWorldPoint(pss);
        last_mpos = p;
        p.z = 0;
        
        draggin = col.bounds.Contains(p);
    }

    void Update(){
        if(draggin){
            var pss = Mouse.current.position.ReadValue();
            var p = cam.ScreenToWorldPoint(pss);
            var delta = p - last_mpos;
            transform.position += delta;

            last_mpos = p;
        }
    }
}
