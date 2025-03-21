using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Playables;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private PlayerAnimatorContoller _playerAnimatorContoller;
    private int jumpCount = 2;
    private SpriteRenderer _spriteRenderer;
    private playerMain _playerMain;
    private bool isInfected;
    
    [SerializeField] private float movingSpeed = 7000;
    [SerializeField] private float horizontalDrag = 8;
    [SerializeField] private float jumpingPower = 35;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float damageForce = 10;
    [SerializeField] private float damageEffectTime = .2f;
    [SerializeField] private int enemyDamageAmount=10;
    [SerializeField] private int spikesDamageAmount=5;
    [SerializeField] private int infectionDamageAmount=3;
    [SerializeField] private GameObject healEatEffect;
    [SerializeField] private GameObject infectionEffect;
    
    public int maxHealth = 100;
    public int health;
    public Slider healthBar; // ✅ Health Bar
    public Slider dashCooldownBar; // ✅ Dash Cooldown Bar
    public float movingControll;
    public float dashCoolDown = 2f; // Cooldown time in seconds
    public float timeToDash = 1.5f;
    public float dashForce = 10000f;
    public GameObject jumpLand;
    public Transform feet;
    public float effectOffset = 0.5f;
    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioSource audioSource;
    public GameObject ecoInkEffect;
    public AudioClip playerTakeDamage;
    public AudioClip playerDie;
    public GameObject playerDeathEffect;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerAnimatorContoller = GetComponent<PlayerAnimatorContoller>();
        playerRB = GetComponent<Rigidbody2D>();
        _playerMain = GetComponent<playerMain>();

        // ✅ Initialize Health Bar
        health = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = health;

        // ✅ Initialize Dash Cooldown Bar
        dashCooldownBar.maxValue = dashCoolDown;
        dashCooldownBar.value = dashCoolDown; // Full at start (dash is ready)
    }

    void Update()
    {
        movingControll = Input.GetAxis("Horizontal");

        playerRB.AddForce(Vector2.right * movingControll * movingSpeed * Time.deltaTime, ForceMode2D.Force);
        Vector3 hVelocity = playerRB.velocity;
        hVelocity.x *= 1 - horizontalDrag * Time.deltaTime;
        playerRB.velocity = hVelocity;

        if (Input.GetKeyDown(KeyCode.Space) && jumpCount != 0)
        {
            Vector3 vVelocity = playerRB.velocity;
            vVelocity.y = jumpingPower;
            playerRB.velocity = vVelocity;
            audioSource.PlayOneShot(jumpSound);
            if (jumpCount == 2) _playerAnimatorContoller.PlayNormalJumpAnimation();
            if (jumpCount == 1) _playerAnimatorContoller.PlayDoubleJumpAnimation();
            jumpCount--;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && movingControll != 0)
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
        
        Vector3 spawnPosition = new Vector3(feet.position.x, feet.position.y + effectOffset, feet.position.z);
        Instantiate(jumpLand, spawnPosition, Quaternion.identity);

        

        if (collision.gameObject.tag == "spikes")
        {
            StartCoroutine(Damage(Vector3.up,spikesDamageAmount,damageForce,false));
        }
        
        if (collision.gameObject.CompareTag("ecoInk"))
        {
            _playerMain.UpdateInkAmount();
            Instantiate(ecoInkEffect, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Heart"))
        {
            Heal(10);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (isDashing)
        {
            if (col.CompareTag("Bob"))
            {
                col.gameObject.GetComponent<BobEnemyAI>().TakeDamage(10);
            }
            if (col.CompareTag("Slug"))
            {
                col.gameObject.GetComponent<slugEnemyAI>().TakeDamage(10);
            }
            if (col.CompareTag("Tree"))
            {
                col.gameObject.GetComponent<TreeEnemyAI>().TakeDamage(10);
            }
            if (col.CompareTag("Mashroom"))
            {
                col.gameObject.GetComponent<MashroomEnemyAI>().TakeDamage(10);
            }
        }

        if (col.CompareTag("killArea"))
        {
            health -= 100;
            healthBar.value = health;
            Instantiate(playerDeathEffect, transform.position, quaternion.identity);
            Destroy(gameObject,1f);
            Debug.Log("noob died");
        }

        string tag = col.gameObject.tag;
        if (!isDashing)
        {
            if (tag == "Bob" || tag == "Slug" || tag == "Tree")
            {
                Vector3 dir = transform.position - col.transform.position;
                StartCoroutine(Damage(dir, enemyDamageAmount, damageForce,false));
            }

            if (tag == "Mashroom"&&!isInfected)
            {
                Vector3 dir = transform.position - col.transform.position;
                StartCoroutine(Damage(dir, enemyDamageAmount, damageForce,true));
                StartCoroutine(Infect());
            }
        }
    }

    IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        _playerAnimatorContoller.PlayDashAnimation();

        float originalGravity = playerRB.gravityScale;
        playerRB.gravityScale = 0;

        Vector2 dashDirection = movingControll != 0 ? new Vector2(movingControll, 0).normalized : new Vector2(transform.localScale.x, 0);
        float dashStartTime = Time.time;

        while (Time.time < dashStartTime + timeToDash)
        {
            playerRB.velocity = dashDirection * dashForce;
            yield return null;
        }

        isDashing = false;
        _playerAnimatorContoller.StopDashAnimation();
        playerRB.gravityScale = originalGravity;
        playerRB.velocity = Vector2.zero;

        // ✅ Start Dash Cooldown
        StartCoroutine(DashCooldown());

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }

    // ✅ Dash Cooldown Bar Logic
    IEnumerator DashCooldown()
    {
        dashCooldownBar.value = 0; // Empty bar when dash starts

        float elapsedTime = 0;
        while (elapsedTime < dashCoolDown)
        {
            elapsedTime += Time.deltaTime;
            dashCooldownBar.value = elapsedTime; // Update bar over time
            yield return null;
        }

        dashCooldownBar.value = dashCoolDown; // Refill bar when cooldown ends
    }

    // Health Bar Logic (No Changes)
    IEnumerator Damage(Vector3 dir,int amount,float force,bool infectMode)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthBar.value = health;

        int fliper = 1;
        if (dir.x < 0)
        {
            fliper = -1;
        }
        playerRB.AddForce(new Vector3(fliper, .5f, 0) * force, ForceMode2D.Impulse);
        _spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(damageEffectTime);
        audioSource.PlayOneShot(playerTakeDamage);
        if (!infectMode)
        {
            _spriteRenderer.color = Color.white;
        }
        else
        {
            _spriteRenderer.color=Color.green;
        }

        if (health <= 0)
        {
            Die();
        }
    }

    IEnumerator Infect()
    {
        infectionEffect.SetActive(true);
        isInfected = true;
        for (int i = 0; i < 8; i++)
        {
            _spriteRenderer.color=Color.green; 
            StartCoroutine(Damage(Vector3.up, infectionDamageAmount, 0,true));
            yield return new WaitForSeconds(2);
        }
        _spriteRenderer.color=Color.white;
        isInfected = false;
        infectionEffect.SetActive(false);
    }

    void Heal(int amount)
    {
        health += amount;
        healthBar.value = health;
        Instantiate(healEatEffect, transform.position, quaternion.identity);
    }
    public void Die()
    {
        health = 0;
        healthBar.value = health;
        audioSource.PlayOneShot(playerDie);
        Instantiate(playerDeathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject, 2f);
    }
}
