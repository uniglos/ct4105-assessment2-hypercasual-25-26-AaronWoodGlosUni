using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UpdateText : MonoBehaviour {

    TextMeshProUGUI text;
    public string format;
    private void Awake() {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void UpdateTextintFormated(int num)
    {
        text.text = $"{format}{num.ToString()}";
    }
    public void UpdateTextStringformated(string str)
    {
        text.text = $"{format}{str.ToString()}";
    }

    public void UpdateTextint(int num) {
        text.text = num.ToString();
    }
    public void UpdateTextString(string str) {
        text.text = str.ToString();
    }
}