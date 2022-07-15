using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RollEntry : MonoBehaviour {
    [SerializeField] Button button;
    [SerializeField] TMP_Text txt;

    public UnityEvent onClicked => button.onClick;
    public string text {set => txt.text = value;}
}
