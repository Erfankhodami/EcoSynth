using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAILogic : MonoBehaviour
{
    public float speed;
    [SerializeField] private bool movingRight = true;
    public Transform groundDetection;
    public float rayLength = 2f;
    public float health = 10f;
    public GameObject dieEffect;
    public bool isDead = false;
    
    private void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down,rayLength);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
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

    public void takeDamage(int damage)
    {
        health -= damage;
        if (health == 0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        
    }
}
