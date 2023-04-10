using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    //public event EventHandler OnDead;

    [SerializeField] private int health = 100;
    private int healthMax;

    private void Awake()
    {
        healthMax = health;
    }

    public void Damage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0)
        {
            health = 0;
        }
        if (health == 0)
        {
            
        }

        Debug.Log("HP : " + health);
    }
    /*
    private void Die()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }*/

    public float GetHealthNormalized()
    {
        return (float)health / healthMax;
    }

    public float GetHealth()
    {
        return health;
    }
}