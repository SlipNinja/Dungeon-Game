using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : HealthComponent
{

    void Start()
    {
        if (InterfaceHandler.instance == null)
        {
            this.enabled = false;
            return;
        }
        InterfaceHandler.instance.SetMaxSliderValue(maxHealthPoints);
    }

    void Update()
    {
        InterfaceHandler.instance.SetSliderValue(curHealthPoints);
    }
}
