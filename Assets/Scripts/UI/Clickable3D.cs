using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Clickable3D : MonoBehaviour, IClickable {
    Collider col;
    Camera cam;
    public UnityEvent onClick;

    UnityEvent IClickable.onClick => onClick;

    
    void Start(){
        cam = Camera.main;
        col = GetComponent<Collider>();
        var control = ControlManager.instance.mainControl;
        control.Player.Click.started += ctx => Evaluate();
        onClick.AddListener(() => print(name));
    }
    void Evaluate(){
        var pss = Mouse.current.position.ReadValue();
        var p = cam.ScreenToWorldPoint(pss);
        var ray = cam.ScreenPointToRay(pss);
        var hit = col.Raycast(ray, out var _, 999);
        p.z = 0;
        
        //if(col.bounds.Contains(p)) onClick.Invoke();
        if(hit) onClick.Invoke();
    }
}
