﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimatorEvents : MonoBehaviour
{
    BasicEnemy basicEnemy;
    private void Awake()
    {
        basicEnemy = transform.root.GetComponent<BasicEnemy>();
    }

    public void AttemptHit()
    {
        if (basicEnemy.currentHealth <= 0)
            return;

        Player player = Player.instance;

        Vector3 enemyForward = basicEnemy.transform.forward;
        Vector3 directionToPlayer = player.transform.position - basicEnemy.transform.position;
        float angle = Vector3.Angle(enemyForward, directionToPlayer);
        //Debug.DrawRay(basicEnemy.transform.position, enemyForward, Color.red, 2);
        //Debug.DrawRay(basicEnemy.transform.position, directionToPlayer, Color.green, 2);

        if (Vector3.Distance(player.transform.position, transform.root.position) < basicEnemy.attackRange && angle <= basicEnemy.attackWidthArc)
        {
            player.TakeDamage(basicEnemy.damage);
        }
    }
}
