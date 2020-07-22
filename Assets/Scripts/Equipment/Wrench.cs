using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wrench : Weapon
{
    public float swingTime = 1;
    Coroutine currentSwingCoroutine;

    public override void Interact()
    {
        if(currentSwingCoroutine == null)
            currentSwingCoroutine = StartCoroutine(Swing());

    }

    void AttemptHit()
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

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(swingTime);
        AttemptHit();

        currentSwingCoroutine = null;
    }
}
