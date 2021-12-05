using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDetector : MonoBehaviour
{
    public List<WeaponComponent> gunList = new List<WeaponComponent>();
   /// <summary>
   /// Checks if there is an available weapon to pickup
   /// </summary>
   /// <param name="other"></param>
    public void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody)
            if (other.attachedRigidbody.gameObject.layer == 10)
            {
                WeaponComponent temp = other.attachedRigidbody.GetComponent<WeaponComponent>();
                if (!temp.grabbed)
                    if (!gunList.Contains(temp))
                        gunList.Add(temp);
            }
    }
    /// <summary>
    /// removes guns from list of posible being picked up
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.attachedRigidbody)
            if (other.attachedRigidbody.gameObject.layer == 10)
            {
                WeaponComponent temp = other.attachedRigidbody.GetComponent<WeaponComponent>();
                if (gunList.Contains(temp))
                    gunList.Remove(temp);
            }

    }
}