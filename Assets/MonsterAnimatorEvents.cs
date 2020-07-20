using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAnimatorEvents : MonoBehaviour
{
    public void AttemptHit()
    {
        BasicEnemy basicEnemy = transform.root.GetComponent<BasicEnemy>();
        if(Vector3.Distance(Player.instance.transform.position, transform.root.position) < basicEnemy.attackRange)
        {
            Player.instance.TakeDamage(basicEnemy.damage);
        }
    }
}
