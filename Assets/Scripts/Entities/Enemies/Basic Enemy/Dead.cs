using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : State
{
    readonly BasicEnemy basicEnemy;
    Collider playerMovementCollider;

    public Dead(BasicEnemy basicEnemy, Collider playerMovementCollider)
    {
        this.basicEnemy = basicEnemy;
        this.playerMovementCollider = playerMovementCollider;
    }

    public void Tick()
    {

    }
    public void OnEnter()
    {
        playerMovementCollider.enabled = false;

        var yPlaneHD = basicEnemy.lastHitDirection;
        basicEnemy.lastHitDirection.y = 0;

        var yPlaneF = basicEnemy.transform.forward;
        yPlaneF.y = 0;

        if (Vector3.Dot(yPlaneHD, yPlaneF) <= 0)
            basicEnemy.animator.SetTrigger("DeathFront");
        else
            basicEnemy.animator.SetTrigger("DeathBack");
    }
    public void OnExit() { }

}
