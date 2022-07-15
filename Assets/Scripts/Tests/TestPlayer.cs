using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour {
    ControlMap controlMap;
    public float speed = 10;
    void Awake(){
        controlMap = new ControlMap();
        controlMap.Enable();

        controlMap.Player.Shoot.performed += ctx => print("Shooot");
    }

    void Update(){
        var m = controlMap.Player.Move.ReadValue<Vector2>();
        Vector3 d = m * Time.deltaTime * speed;
        transform.position += d;
    }
}
