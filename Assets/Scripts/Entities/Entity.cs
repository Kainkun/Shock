using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    protected float currentHealth;
    [HideInInspector]
    public Vector3 lastHitDirection;

    void Start()
    {
        currentHealth = maxHealth;
    }


    void Update()
    {
        
    }
    public virtual void TakeDamage(float damage, Vector3 hitDirection)
    {
        lastHitDirection = hitDirection;
        TakeDamage(damage);
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }




    public virtual void Heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public virtual void Die()
    {

    }

}
