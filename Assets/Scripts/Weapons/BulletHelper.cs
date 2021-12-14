using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHelper : MonoBehaviour
{

    public Transform target;
    Vector3 temp;

    private void Awake()
    {

        target = FindObjectOfType<CharacterControl>().transform;
    }
    void Update()
    {
        temp = (target.position - this.transform.position).normalized;

        this.transform.rotation = Quaternion.LookRotation(temp);
    }

    public void DisableSelf()
    {
        this.gameObject.SetActive(false);
    }

    public void StopSelf()
    {
        GetComponentInParent<Rigidbody>().velocity = Vector3.zero;
    }
}
