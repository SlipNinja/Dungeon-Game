using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandler : MonoBehaviour
{
    public static InterfaceHandler instance;
    public Text floor;
    public Slider healthSlider;
    private void Awake()
    {
        instance = this;
    }
    public void SetSliderValue(int newValue)
    {
        healthSlider.value = newValue;
    }

    public void SetMaxSliderValue(int newMaxValue)
    {
        healthSlider.maxValue = newMaxValue;
    }

    public void SetFloor(int currentfloor)
    {
        floor.text = "Floor " + currentfloor;
    }

}
