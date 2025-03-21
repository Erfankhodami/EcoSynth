using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class MashroomEnemyAI : EnemyInteligence
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
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        if (health <= 0)
        {
            Die(transform.position);
            Instantiate(heart, transform.position, quaternion.identity);
        }
    }
}