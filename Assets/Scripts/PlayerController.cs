using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private PlayerAnimatorContoller _playerAnimatorContoller;
    [SerializeField] private float movingSpeed=7000;
    [SerializeField] private float horizontalDrag = 8;
    [SerializeField] private float jumpingPower=35;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float damageForce=10;
    [SerializeField] private float damageEffectTime = .5f;
    public int health = 100;
    
    public float movingControll;
    public float dashCoolDown = 2f;
    public float timeToDash = 1.5f;
    public float dashForce = 10000f;
    public GameObject jumpLand;
    public Transform feet;
    public float effectOffset = 0.5f;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioSource audioSource;
    private int jumpCount = 2;
    private playerMain _playerMain;
    public GameObject ecoInkEffect;
    private SpriteRenderer _spriteRenderer;
    public AudioClip playerTakeDamage;
    public AudioClip playerDie;
    public GameObject playerDeathEffect;
    
    
    
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimatorContoller = GetComponent<PlayerAnimatorContoller>();
        playerRB = GetComponent<Rigidbody2D>();
        _playerMain = GetComponent<playerMain>();
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
            audioSource.PlayOneShot(jumpSound);
            if (jumpCount == 2)
            {
                    _playerAnimatorContoller.PlayNormalJumpAnimation();
            }
            if (jumpCount == 1)
            {
                _playerAnimatorContoller.PlayDoubleJumpAnimation();
            }
            jumpCount--;
        }
        
        //dash system
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash&& movingControll!=0)
        {
            StartCoroutine(Dash());
            audioSource.PlayOneShot(dashSound);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            jumpCount = 2;
            _playerAnimatorContoller.PlayLandAnimation();
        }
        Vector3 spawnPosition = new Vector3(feet.position.x, feet.position.y + effectOffset , feet.position.z);
        Instantiate(jumpLand, spawnPosition, Quaternion.identity);
        
        if (collision.collider.CompareTag("Enemy") && isDashing)
        {
            collision.gameObject.GetComponent<enemyAILogic>().takeDamage(10);
            collision.gameObject.GetComponent<enemyMain>().onDie();
        }
        if (collision.gameObject.CompareTag("ecoInk"))
        {
            _playerMain.UpdateInkAmount();
            Instantiate(ecoInkEffect, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Enemy"&&!isDashing)
        {
            Vector3 dir = transform.position-collision.transform.position  ;
            StartCoroutine(Damage(dir));
        }

        if (collision.gameObject.tag == "spikes")
        {
            Die();
        }
        
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        _playerAnimatorContoller.PlayDashAnimation();
        
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
        _playerAnimatorContoller.StopDashAnimation();
        playerRB.gravityScale = originalGravity; // Restore gravity
        playerRB.velocity = Vector2.zero; // **Stop movement after dash**

        yield return new WaitForSeconds(dashCoolDown); // Wait before allowing another dash
        canDash = true;
    }

    //this method handles the damage effect
    IEnumerator Damage(Vector3 dir)
    {
        health -= 10;
        int fliper = 1;
        if (dir.x < 0)
        {
            fliper = -1;
        }
        playerRB.AddForce(new Vector3(fliper,.5f,0)*damageForce,ForceMode2D.Impulse);
        _spriteRenderer.color=Color.red;
        yield return new WaitForSeconds(damageEffectTime);
        audioSource.PlayOneShot(playerTakeDamage);
        _spriteRenderer.color = Color.white;
    }

    public void Die()
    {
        audioSource.PlayOneShot(playerDie);
        Instantiate(playerDeathEffect, transform.position, Quaternion.identity);
        Debug.Log("dead!!!");
        Destroy(gameObject,2f);
    }
    

}
