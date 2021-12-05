using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    #region Variables
    public bool grabbed = false;
    public Rigidbody myRigidbody;
    public Transform spawnBullet;

    public WeaponProperties data;

    Vector3 originalPosition;
    Quaternion originalRotation;
    #endregion

    #region  MonoBehviour
    public void Awake()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        myRigidbody = GetComponent<Rigidbody>();
    }
    #endregion

    #region Methods
    public void Reposition()
    {
        this.transform.SetPositionAndRotation(originalPosition, originalRotation);
    }
    #endregion
}