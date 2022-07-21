using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ControlManager : MonoBehaviour {
    public static ControlManager instance;
    public ControlMap mainControl;
    [SerializeField]GfxClickable mainRaycaster;
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
        mainRaycaster.onClick.AddListener(() => onMouseDown.Invoke(mpos));
        mainControl.Player.Exit.performed += ctx => Application.Quit();
        mainControl.Player.Restart.performed += 
        ctx => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public UnityEvent<Vector2> onMouseDown = new UnityEvent<Vector2>();

    void OnDestroy(){
        print("Disposing this crap");
        mainControl.Dispose();
    }
}
