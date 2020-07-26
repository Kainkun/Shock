using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    readonly BasicEnemy basicEnemy;
    float timeSinceLooking;
    float randomTime;

    public Idle(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {
        timeSinceLooking += Time.deltaTime;
        if(timeSinceLooking > randomTime)
        {
            if(Random.Range(0,2) == 0)
                basicEnemy.animator.SetTrigger("LookLeft");
            else
                basicEnemy.animator.SetTrigger("LookRight");
            
            timeSinceLooking = 0;
            randomTime = Random.Range(4f, 6f);
        }
    }
    public void OnEnter()
    {
        timeSinceLooking = 0;
        randomTime = Random.Range(4f, 6f);
        basicEnemy.animator.SetFloat("ForwardSpeed", 0);
    }
    public void OnExit() { }

}
