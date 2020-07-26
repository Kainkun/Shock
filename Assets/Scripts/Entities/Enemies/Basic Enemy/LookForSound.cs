using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForSound : State
{
    readonly BasicEnemy basicEnemy;
    public LookForSound(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {
        basicEnemy.animator.SetFloat("ForwardSpeed", Mathf.Lerp(basicEnemy.animator.GetFloat("ForwardSpeed"), 0, Time.deltaTime * 5));

        basicEnemy.timeSinceHeard += Time.deltaTime;

        Vector3 direction = basicEnemy.lastHeardSoundPosition - basicEnemy.transform.position;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction, basicEnemy.transform.up);
        Quaternion rotTowards = Quaternion.RotateTowards(basicEnemy.transform.rotation, lookRotation, Time.deltaTime * 90);

        basicEnemy.transform.rotation = rotTowards;
    }
    public void OnEnter()
    {
        
    }
    public void OnExit() { }

}
