using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInteligence : MonoBehaviour
{
    public float movingSpeed = 3;
    [SerializeField] private float overlapCircleRadious = 10;
    [SerializeField] private GameObject inkDrop;
    public float attackRadious = 1;
    public LayerMask playerLayerMask;
    public LayerMask defaultLayerMask;
    private int flipper = 1;
    private SpriteRenderer _spriteRenderer;
    private float checkerOffset=1;
    private float checkerRadious=.3f;
    private bool checkForPlayer = true;
    public int health;
    public bool isDead;
    public GameObject dieEffect;
    public AudioClip dieSFX;
    public AudioSource enemySource;
    public Animator _animator;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerLayerMask = LayerMask.GetMask("Player");
        defaultLayerMask = LayerMask.GetMask("Default");
        _animator = GetComponent<Animator>();
    }
    
    public void Behave()
    {
        Vector3 checker = new Vector3(checkerOffset, 0, 0) * flipper;
        Collider2D front = Physics2D.OverlapCircle(transform.position + checker, checkerRadious, defaultLayerMask);
        Collider2D hit = Physics2D.OverlapCircle(transform.position, overlapCircleRadious, playerLayerMask);
        
        
        if (checkForPlayer)
        {
            if (hit != null)
            {
                FollowPlayer(hit);
            }
        }
        if(hit==null||!checkForPlayer)
        {
            RoamAround(front);
        }
        
    }
    
    void RoamAround(Collider2D hit)
    {
        // Flip movement if there's a wall
        if (hit != null)
        {
            Flip();
        }

        // Move in the current direction
        transform.Translate(movingSpeed * transform.right * flipper * Time.deltaTime);
    }

    void FollowPlayer(Collider2D hit)
    {
        // Check for wall during follow
        Vector3 checker = new Vector3(checkerOffset, 0, 0) * flipper;
        Collider2D wallCheck = Physics2D.OverlapCircle(transform.position + checker, checkerRadious, defaultLayerMask);
        
        if (wallCheck != null)
        {
            RoamAround(hit);
            checkForPlayer = false;
            StartCoroutine(Delay());
        }
        
        Vector2 movingDir;
        if (transform.position.x - hit.transform.position.x < 0)
        {
            movingDir = Vector2.right;
            flipper = 1;
            _spriteRenderer.flipX = true;
        }
        else
        {
            movingDir = Vector2.left;
            flipper = -1;
            _spriteRenderer.flipX = false;
        }

        
        transform.Translate(movingDir * movingSpeed * Time.deltaTime);
    }

    void Flip()
    {
        flipper *= -1;
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        checkForPlayer = true;
    }

    public virtual void TakeDamage(int damage)
    {
    }

    public void Die(Vector3 position)
    {
        isDead = true;
        Instantiate(dieEffect, transform.position, Quaternion.identity);
        Instantiate(inkDrop, position, Quaternion.identity);
        enemySource.PlayOneShot(dieSFX);
        Destroy(gameObject);
    }

}