using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    readonly BasicEnemy basicEnemy;
    public float time;

    public Attack(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {
        time += Time.deltaTime;
    }
    public void OnEnter()
    {
        time = 0;
        basicEnemy.animator.SetTrigger("Attack");
    }
    public void OnExit()
    {
        time = 0;
    }

}
