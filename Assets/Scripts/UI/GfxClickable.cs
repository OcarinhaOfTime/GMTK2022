using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GfxClickable : MonoBehaviour, IPointerClickHandler {
    public UnityEvent onClick;

    public void OnPointerClick(PointerEventData eventData) {
        onClick.Invoke();
    }
}
