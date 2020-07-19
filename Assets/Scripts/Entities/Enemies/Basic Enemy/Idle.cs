using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    readonly BasicEnemy basicEnemy;

    public Idle(BasicEnemy basicEnemy)
    {
        this.basicEnemy = basicEnemy;
    }

    public void Tick()
    {

    }
    public void OnEnter()
    {
        basicEnemy.animator.SetFloat("ForwardSpeed", 0);
    }
    public void OnExit() { }

}
