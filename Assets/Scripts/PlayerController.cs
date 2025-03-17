using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private float movingControll;
    [SerializeField] private float movingSpeed=7000;
    [SerializeField] private float horizontalDrag = 8;
    [SerializeField] private float jumpingPower=35;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    public float dashCoolDown = 2f;
    public float timeToDash = 1.5f;
    public float dashForce = 10000f;
    public GameObject jumpLand;
    public Transform feet;
    public float effectOffset = 0.5f;
    public AudioClip jumpSound;
    public AudioSource audioSource;
    
        
    
    
    
    private int jumpCount = 2;
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        //

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
            audioSource.PlayOneShot(jumpSound);

        }
        
        //dash system
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform")
        {
            jumpCount = 2;
        }
        Vector3 spawnPosition = new Vector3(feet.position.x, feet.position.y + effectOffset , feet.position.z);
        Instantiate(jumpLand, spawnPosition, Quaternion.identity);
        
        if (collision.collider.CompareTag("Enemy") && isDashing)
        {
            collision.gameObject.GetComponent<enemyAILogic>().takeDamage(10);
            StartCoroutine(collision.gameObject.GetComponent<enemyMain>().onDie());
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        float originalGravity = playerRB.gravityScale;
        playerRB.gravityScale = 0; // Disable gravity for a smooth dash

        Vector2 dashDirection = movingControll != 0 ? new Vector2(movingControll, 0).normalized : new Vector2(transform.localScale.x, 0);
        float dashStartTime = Time.time;

        while (Time.time < dashStartTime + timeToDash)
        {
            playerRB.velocity = dashDirection * dashForce;
            yield return null; // Wait for next frame
        }

        // **STOP DASHING PROPERLY**
        isDashing = false;
        playerRB.gravityScale = originalGravity; // Restore gravity
        playerRB.velocity = Vector2.zero; // **Stop movement after dash**

        yield return new WaitForSeconds(dashCoolDown); // Wait before allowing another dash
        canDash = true;
    }

}
