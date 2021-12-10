using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandler : MonoBehaviour
{
    private static Text floor;
    private static Slider healthSlider;
    private Transform backimage;

    void Start()
    {
        backimage = gameObject.transform.Find("backimage");

        floor = backimage.Find("floor").GetComponent<Text>();
        healthSlider = backimage.Find("LifeBar").GetComponent<Slider>();
    }
    
    public static void SetSliderValue(int newValue)
    {
        healthSlider.value = newValue;
    }

    public static void SetMaxSliderValue(int newMaxValue)
    {
        healthSlider.maxValue = newMaxValue;
    }

    public static void SetFloor(int currentfloor)
    {
        floor.text = "Floor " + currentfloor;
    }

}
