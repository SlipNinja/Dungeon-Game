using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthComponent : MonoBehaviour
{
    public int maxHealthPoints;
    public int curHealthPoints;
    public UnityEvent deathEvent;
    public UnityEvent damageEvent;

    public void Awake()
    {
        curHealthPoints = maxHealthPoints;
    }

    public void ReceiveDamage(int value = 1)
    {
        curHealthPoints -= value;

        DamageEffect();
    }

    public virtual void DamageEffect()
    {
        damageEvent.Invoke();
        //Foo

        if (curHealthPoints < 0)
        {
            deathEvent.Invoke();
        }
    }
    public void RestoreHealth()
    {
        curHealthPoints = maxHealthPoints;
    }

    public void DisableSelf()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
}