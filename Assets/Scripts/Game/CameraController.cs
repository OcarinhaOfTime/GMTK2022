using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public static CameraController instance;
    Camera cam;
    public float speed = 2;
    public Vector2 lb;
    public Vector2 ut;
    //Vector3 cameraDelta;
    Vector2 deltaMov;
    public bool controlling = true;
    public Vector2 pos {
        get => transform.position;
        set {
            var p = transform.position;
            p.x = value.x;
            p.y = value.y;
            transform.position = p;
        }
    }

    void Awake() {
        instance = this;
        cam = GetComponent<Camera>();
    }
    void Start() {
        var control = ControlManager.instance.mainControl.Player;
        control.Move.started +=
        ctx => deltaMov = ctx.ReadValue<Vector2>();

        control.Move.performed +=
        ctx => deltaMov = ctx.ReadValue<Vector2>();

        control.Move.canceled +=
        ctx => deltaMov = Vector2.zero;

        control.Zoomin.performed += async ctx => await Zoom(1);
        control.Zoomout.performed += async ctx => await Zoom(-1);
    }

    Vector2 ClampToMap(Vector2 p) {
        var map = MapController.instance.map;
        float h = cam.orthographicSize;
        float w = h * Screen.width / (float)Screen.height;
        lb = new Vector2(w - map.width / 2f, h - map.height / 2f);
        ut = new Vector2(map.width / 2f - w, map.height / 2f - h);
        lb.x = Mathf.Min(0, lb.x);
        ut.x = Mathf.Max(0, ut.x);

        p.x = Mathf.Clamp(p.x, lb.x, ut.x);
        p.y = Mathf.Clamp(p.y, lb.y, ut.y);

        return p;
    }
    void ClampToMap() {
        var p = pos;
        p = ClampToMap(p);
        pos = p;
    }
    public Vector2 v;
    void MoveCamera(Vector2 delta) {
        v = delta * Time.deltaTime * speed;
        pos += v;
        ClampToMap();
    }
    void Update() {
        if (!controlling) return;
        MoveCamera(deltaMov);
    }

    public void FocusImmediate(Vector2 p) {
        p = ClampToMap(p);
        pos = p;
    }

    public async Task Focus(Vector2 p) {
        p = ClampToMap(p);
        float d = Vector2.Distance(p, transform.position);
        float dur = d / 10f;
        await AsyncTweener.Tween(dur, t => pos = Vector2.Lerp(pos, p, t * t));
    }

    public int zoom = 0;
    public async Task Zoom(int dir) {
        if (!controlling) return;
        var newZoom = Mathf.Clamp(zoom + dir, 0, 2);
        if (newZoom == zoom) return;
        var os = cam.orthographicSize;
        var oss = new float[3] { 5, 7, 9 };
        var nos = oss[newZoom];
        zoom = newZoom;
        controlling = false;
        await AsyncTweener.Tween(.25f, t => {
            cam.orthographicSize = Mathf.Lerp(os, nos, t * t);
            ClampToMap();
        });

        controlling = true;
    }
}
