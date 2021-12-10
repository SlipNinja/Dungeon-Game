using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthComponent : HealthComponent
{

    private InterfaceHandler playerInterface;

    void Start()
    {
        playerInterface = GameObject.Find("UserInterface").GetComponent<InterfaceHandler>();
        playerInterface.SetMaxSliderValue(maxHealthPoints);
    }

    void Update()
    {
        playerInterface.SetSliderValue(curHealthPoints);
    }
}
