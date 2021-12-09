using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorPlatform : MonoBehaviour
{
    public bool playerOnPlatform = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            playerOnPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            playerOnPlatform = false;
        }
    }
}
