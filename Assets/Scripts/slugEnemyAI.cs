using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slugEnemyAI : EnemyInteligence
{
    private void Update()
    {
        Collider2D atackCheck = Physics2D.OverlapCircle(transform.position, attackRadious, playerLayerMask);
        if (atackCheck != null)
        {
            _animator.SetBool("isAttacking",true);
        }
        else
        {
            _animator.SetBool("isAttacking",false);
        }
        Behave();
    }
    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health == 0)
        {
            Die(transform.position);
        }
    }
}
