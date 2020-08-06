using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : Entity
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
