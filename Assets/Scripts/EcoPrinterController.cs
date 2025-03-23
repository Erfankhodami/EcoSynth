using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class EcoPrinterController : MonoBehaviour
{
    [SerializeField] private float detectionArea = 10;
    [SerializeField] private GameObject wall;
    [SerializeField] private float wallPlacementSpeed;
    /*[SerializeField] private Collider2D bossWeakSpot;
    [SerializeField] private Collider2D boosNormalSpot;*/
    [SerializeField] private float health = 100;
    [SerializeField] private Effects effects;
    [SerializeField] private float movingSpeed=10;
    [SerializeField] private GameObject bossFightUI;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float attackRadious = 3;
    [SerializeField] private AudioClip musicClip;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private GameObject wonMenu;
    [SerializeField] private AudioClip damageTakingClip;
    public int DamagePower;
    private Vector3 wallPlacementPos;
    private LayerMask playerLayerMask;
    private bool isBossFightStarted=false;
    private PlayerController playerGameObject;
    private SpriteRenderer _spriteRenderer;
    private bool isSecondStage=false;
    private float fullHealth;
    private Animator _animator;
    private Camera mainCam;
    private AudioSource themePlayer;
    private AudioSource SFXPlayer;

    [System.Serializable] class  Effects
    {
        public GameObject normalDamageEffect;
        public GameObject weakSpotDamageEffect;
        public GameObject dieEffect;
    }
    private void Start()
    {
        mainCam=Camera.main;
        fullHealth = health;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        playerLayerMask = LayerMask.GetMask("Player");
        playerGameObject = GameObject.Find("Player").GetComponent<PlayerController>();
        wallPlacementPos = transform.position;
        wallPlacementPos.x -= detectionArea+5;
        _animator = GetComponent<Animator>();
        themePlayer = GameObject.Find("LevelTheme").GetComponent<AudioSource>();
        SFXPlayer = GameObject.Find("SoundPlayer").GetComponent<AudioSource>();
    }

    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectionArea,playerLayerMask);
        if (hit is not null&& !isBossFightStarted)
        {
            StartCoroutine(PlaceWall());
            StartCoroutine(ZoomCamera());
            isBossFightStarted = true;
            bossFightUI.SetActive(true);
            _animator.SetBool("isBossFightStarted",true);
            themePlayer.Pause();
            themePlayer.clip = musicClip;
            themePlayer.Play();
        }

        if (isBossFightStarted)
        {
            BossFight();
        }
    }
    void BossFight()
    {
        FollowPlayer();
        Collider2D hit = Physics2D.OverlapCircle(transform.position, attackRadious, playerLayerMask);
        if (hit != null)
        {
            _animator.SetBool("isAttacking",true);
            SFXPlayer.PlayOneShot(damageClip);
        }
        else
        {
            _animator.SetBool("isAttacking",false);
        }
    }

    IEnumerator PlaceWall()
    {
        GameObject gm=Instantiate(wall, wallPlacementPos, wall.transform.rotation);
        var transformPosition = gm.transform.position;
        transformPosition.y += 100;
        gm.transform.position = transformPosition;
        while (gm.transform.position.y>wallPlacementPos.y)
        {
            gm.transform.position = Vector3.Lerp(gm.transform.position, wallPlacementPos, wallPlacementSpeed);
            yield return null;
        }
    }

    void FollowPlayer()
    {
        if (playerGameObject.isDead)
        {
            return;
        }
        Vector2 movingDir;
        if (transform.position.x - playerGameObject.transform.position.x < 0)
        {
            movingDir = Vector2.right;
            _spriteRenderer.flipX = true;
            
            //just for mashroom ahhh
        }
        else
        {
            movingDir = Vector2.left;
            _spriteRenderer.flipX = false;
        }

        
        transform.Translate(movingDir * movingSpeed * Time.deltaTime);
    }
    
    public void DamageBoss(int amount)
    {
        health -= amount;
        GameObject effect = null;
        if (amount >= 10)
        {
            effect = effects.weakSpotDamageEffect;
        }
        else
        {
            effect = effects.normalDamageEffect;
        }

        Instantiate(effect, transform.position, quaternion.identity);
        if (health <= 0)
        {
            if (!isSecondStage)
            {
                SwitchStage();
            }
            else
            {
                Die();
            }
        }
        UpdateSlideBar();
        SFXPlayer.PlayOneShot(damageTakingClip);
    }

    void UpdateSlideBar()
    {
        healthSlider.value = health / fullHealth;
    }

    IEnumerator ZoomCamera()
    {
        while (mainCam.orthographicSize>5)
        {
            mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, 5, .1f);
            yield return null;
        }
    }
    void SwitchStage()
    {
        //cahnge boss sprites
        health = fullHealth;
        Instantiate(effects.dieEffect, transform.position, quaternion.identity);
        _animator.SetBool("isSecondStageStarted",true);
        DamagePower *= 2;
    }
    void Die()
    {
        Instantiate(effects.dieEffect, transform.position, quaternion.identity);
        bossFightUI.SetActive(false);
        wonMenu.SetActive(true);
        isBossFightStarted = false;
        transform.position = new Vector3(10000, 100000, 10000);
    }
}
