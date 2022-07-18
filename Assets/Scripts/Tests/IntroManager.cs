using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {
    ControlMap control;
    void Start(){
        control = new ControlMap();
        control.Enable();
        control.Player.Any.performed += ctx => SceneManager.LoadScene(1);
    }

    void OnDestroy(){
        control.Dispose();
    }
}
