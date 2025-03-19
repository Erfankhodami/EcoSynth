using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slugEnemyAI : MonoBehaviour
{
    [SerializeField] private bool isMovingRight = true;
    public Transform groundDetector;
    public float rayLength = 2f;
    public bool isWalking = false;
    public float speed = 5f;
    [SerializeField] private slugAnimatorController SlugAnimatorController;


    private void Start()
    {
        SlugAnimatorController = gameObject.GetComponent<slugAnimatorController>();
    }

    private void Update()
    {
        SlugAnimatorController.PlayWalkAnimation();
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetector.position, Vector2.down, rayLength);

        if (groundInfo.collider == false)
        {
            if (isMovingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                isMovingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                isMovingRight = true;
            }
        }
    }

    public IEnumerator patrolLogic()
    {
        throw new NotImplementedException();
    }
}
