using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private float movingControll;
    [SerializeField] private float movingSpeed=20000;
    [SerializeField] private float horizontalDrag = 30;
    [SerializeField] private float jumpingPower=35;
    private int jumpCount = 2;
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        //basic player movement
        movingControll = Input.GetAxis("Horizontal");
        //drag system
        playerRB.AddForce(Vector2.right*movingControll*movingSpeed*Time.deltaTime,ForceMode2D.Force);
        Vector3 hVelocity = playerRB.velocity;
        hVelocity.x *= 1-horizontalDrag*Time.deltaTime;
        playerRB.velocity = hVelocity;
        
        if (Input.GetKeyDown(KeyCode.Space)&& jumpCount!=0)
        {
            //jumping force system
            Vector3 vVelocity = playerRB.velocity;
            vVelocity.y = jumpingPower;
            playerRB.velocity = vVelocity;
            jumpCount--;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jumpCount = 2;
        }
    }
}
