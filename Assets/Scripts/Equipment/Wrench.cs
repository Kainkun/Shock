using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : Weapon
{
    public float swingTime = 1;
    Coroutine currentSwingCoroutine;
    bool swinging;

    public override void Interact()
    {
        if (swinging)
            return;

        swinging = true;
        animator.SetTrigger("Swing");
    }

    public void AttemptHit()
    {
        Ray ray;
        RaycastHit hit = GetCrosshairHit(out ray);

        if (hit.transform)
        {
            if (hit.transform.root.GetComponent<Entity>()) //if entity, deal damage
            {
                DamageEntity(hit, ray);
            }
            else
            {
                Destroy(Instantiate(hitEffectPs, hit.point, Quaternion.identity), 5);
            }
        }
    }

    public void SwingDone()
    {
        swinging = false;
    }

}
