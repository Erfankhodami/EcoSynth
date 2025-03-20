using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemyAI : EnemyInteligence
{
    private bool didPlayerNotice = false;
    [SerializeField] private float noticeErea=5;
    void Update()
    {
        if (!didPlayerNotice)
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, noticeErea, playerLayerMask);
            if (hit != null)
            {
                didPlayerNotice = true;
            }
        }
        else
        {
            Behave();
        }
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die(transform.position);
            Destroy(gameObject);
        }
    }
}
