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
        Debug.Log("Idle");
    }
    public void OnEnter() { }
    public void OnExit() { }

}
