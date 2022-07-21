using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Clickable2D : MonoBehaviour, IClickable {
    Collider2D col;
    Camera cam;
    public UnityEvent onClick;

    UnityEvent IClickable.onClick => onClick;

    void Start(){
        cam = Camera.main;
        col = GetComponent<Collider2D>();
        var control = ControlManager.instance.mainControl;
        control.Player.Click.started += ctx => Evaluate();
        onClick.AddListener(() => print(name));
    }
    void Evaluate(){
        var pss = Mouse.current.position.ReadValue();
        var p = cam.ScreenToWorldPoint(pss);
        var hit = col.OverlapPoint(p);
        p.z = 0;
        
        if(hit) onClick.Invoke();
    }
}
