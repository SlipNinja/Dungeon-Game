using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandler : MonoBehaviour
{
    public Text floor;
    public Slider healthSlider;

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
