using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public float damage;
    public float critMultiplier = 2;
    public GameObject hitEffectPs;
    public GameObject critHitEffectPs;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void DamageEntity(RaycastHit hit, Ray ray)
    {
        var entity = hit.transform.root.GetComponent<Entity>();
        var crit = hit.transform.tag == "Critical";

        if (hit.transform.tag == "Critical")
        {
            Destroy(Instantiate(critHitEffectPs, hit.point, Quaternion.identity), 5);
            entity.TakeDamage(damage * critMultiplier, ray.direction);
        }
        else
        {
            Destroy(Instantiate(hitEffectPs, hit.point, Quaternion.identity), 5);
            entity.TakeDamage(damage, ray.direction);
        }
    }

    public override void Interact()
    {
        Ray ray;
        RaycastHit hit = GetCrosshairHit(out ray);

        if (hit.transform)
        {
            if (hit.transform.root.GetComponent<Entity>()) //if entity deal damage
            {
                DamageEntity(hit, ray);
            }
            else
            {
                Destroy(Instantiate(hitEffectPs, hit.point, Quaternion.identity), 5);
            }
        }



    }

}