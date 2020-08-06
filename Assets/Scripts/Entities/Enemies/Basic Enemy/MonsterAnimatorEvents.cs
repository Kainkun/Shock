using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimatorEvents : MonoBehaviour
{
    public BasicEnemy ParentbasicEnemy;

    public void AttemptHit()
    {
        if (ParentbasicEnemy.currentHealth <= 0)
            return;

        Player player = Player.instance;

        Vector3 enemyForward = ParentbasicEnemy.transform.forward;
        Vector3 directionToPlayer = player.transform.position - ParentbasicEnemy.transform.position;
        float angle = Vector3.Angle(enemyForward, directionToPlayer);
        //Debug.DrawRay(basicEnemy.transform.position, enemyForward, Color.red, 2);
        //Debug.DrawRay(basicEnemy.transform.position, directionToPlayer, Color.green, 2);

        if (Vector3.Distance(player.transform.position, transform.root.position) < ParentbasicEnemy.attackRange && angle <= ParentbasicEnemy.attackWidthArc)
        {
            player.TakeDamage(ParentbasicEnemy.damage);
        }
    }
}
