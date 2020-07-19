using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    float health;

    void Start()
    {
        health = maxHealth;
    }


    void Update()
    {
        
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }
    public virtual void Heal(float heal)
    {
        health += heal;
        if (health > maxHealth) health = maxHealth;
    }

    public virtual void Die()
    {

    }

}
