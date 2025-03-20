using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slugEnemyAI : EnemyInteligence
{
    private void Update()
    {
        Behave();
    }
    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health == 0)
        {
            Die(transform.position);
            Destroy(gameObject);
        }
    }
}
