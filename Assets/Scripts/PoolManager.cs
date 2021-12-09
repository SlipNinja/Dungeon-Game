using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public List<BulletComponent> bulletPool = new List<BulletComponent>();

    void Awake()
    {
        instance = this;
    }

   
}
