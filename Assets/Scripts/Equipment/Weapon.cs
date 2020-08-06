using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipment
{
    public float damage;
    public GameObject defaultHitEffectPs;
    public GameObject defaultCritHitEffectPs;

    public void DamageEntity(RaycastHit hit, Ray ray)
    {
        var entity = hit.transform.root.GetComponent<Entity>();
        var crit = hit.transform.tag == "Critical";

        if (hit.transform.tag == "Critical")
        {
            if(entity.critHitPs != null)
                Destroy(Instantiate(entity.critHitPs, hit.point, Quaternion.identity), 5);
            else if(defaultCritHitEffectPs != null)
                Destroy(Instantiate(defaultCritHitEffectPs, hit.point, Quaternion.identity), 5);
        }
        else
        {
            if(entity.hitPs != null)
                Destroy(Instantiate(entity.hitPs, hit.point, Quaternion.identity), 5);
            else if(defaultHitEffectPs != null)
                Destroy(Instantiate(defaultHitEffectPs, hit.point, Quaternion.identity), 5);
        }
            entity.TakeDamage(damage, hit, ray.direction);
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
                Destroy(Instantiate(defaultHitEffectPs, hit.point, Quaternion.identity), 5);
            }
        }



    }

}