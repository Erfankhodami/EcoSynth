using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slugAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator slugAnimator;
    [SerializeField] private slugEnemyAI slugAI;

    private void Start()
    {
        slugAnimator = gameObject.GetComponent<Animator>();
        slugAI = gameObject.GetComponent<slugEnemyAI>();
    }

    public void PlayIdleAnimation()
    {
        if (!slugAI.isWalking)
        {
            slugAnimator.SetBool("isWalking", false);
        }
    }

    public void PlayWalkAnimation()
    {
        slugAnimator.SetBool("isWalking",true);
    }
}
