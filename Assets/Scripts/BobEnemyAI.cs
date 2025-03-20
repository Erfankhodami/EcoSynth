using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class BobEnemyAI : EnemyInteligence
{
    
    [SerializeField] private bool movingRight = true;
    public Transform groundDetection;
    public float rayLength = 2f;
    public AudioClip dieSFX;
    public AudioSource enemySource;

    
    private void Update()
    {
        transform.Translate(Vector2.right * movingSpeed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down,rayLength);
        
        if (groundInfo.collider == false)
        {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
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
