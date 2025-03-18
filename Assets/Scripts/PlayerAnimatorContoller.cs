using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorContoller : MonoBehaviour
{
    private Animator _animator;
    private PlayerController _playerController;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerController = GetComponent<PlayerController>();
    }
    
    private void Update()
    {
        if (_playerController.movingControll != 0)
        {
            _animator.SetBool("isWalking",true);
        }
        else
        {
            _animator.SetBool("isWalking",false);
        }

        if (_playerController.movingControll > .1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = 1;
            transform.localScale = scale;
        }
        if (_playerController.movingControll < -.1f)
        {
            Vector3 scale = transform.localScale;
            scale.x = -1;
            transform.localScale = scale;
        }
    }

    public void PlayNormalJumpAnimation()
    {
        _animator.SetBool("isJumping",true);
    }
    public void PlayDoubleJumpAnimation()
    {
        _animator.SetBool("isDoubleJumping",true);
    }
    public void PlayLandAnimation()
    {
        _animator.SetBool("isJumping",false);
        _animator.SetBool("isDoubleJumping",false);
        _animator.SetTrigger("land");
    }

    public void PlayDashAnimation()
    {
        _animator.SetBool("isDashing",true);
    }
    public void StopDashAnimation()
    {
        _animator.SetBool("isDashing",false);
    }
}
