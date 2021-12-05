using System;
using System.Collections.Generic;
using UnityEngine;
public enum GunType
{
    pistol  = 0,
    shotgun = 1,
    semiauto= 2
}

[Serializable]
public struct WeaponData
{
    public GunType myGunType;
    public float speed;
    public int ammoClip;
    public float firingSpeed;
    [Range(0,0.05f)]
    public float noise;
}