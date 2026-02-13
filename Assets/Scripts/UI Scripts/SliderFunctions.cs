using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderFunctions : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] UnityEvent onSliderFull, onSliderZero;

    private void Start() {
        slider = GetComponent<Slider>();
    }

    public void SetSliderValue(float value) {
        slider.value = value;
        CheckSlider();
    }

    public void AddToSlider(float value) {
        slider.value += value;
        CheckSlider();
    }

    void CheckSlider() {
        if (slider.value == 0f) {
            onSliderZero.Invoke();
        } else if (slider.maxValue == slider.value) {
            onSliderFull.Invoke();
        }
    }
}
