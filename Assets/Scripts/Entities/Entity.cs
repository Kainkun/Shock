using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float maxHealth;
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public Vector3 lastHitDirection;
    public float critMultiplier = 2;
    public GameObject hitPs;
    public GameObject critHitPs;

    void Start()
    {
        currentHealth = maxHealth;
    }


    void Update()
    {
        
    }
    public virtual void TakeDamage(float damage, RaycastHit hit, Vector3 hitDirection)
    {
        lastHitDirection = hitDirection;

        if (hit.collider.tag == "Critical")
            TakeDamage(damage * critMultiplier);
        else
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
