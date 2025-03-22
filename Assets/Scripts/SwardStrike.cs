using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwardStrike : MonoBehaviour
{ 
    private PlayerController _playerController;
    private LayerMask enemyLayerMask;
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField] private float damageRadious=2;
    [SerializeField] private float coolDown=1;
    [SerializeField] private AudioClip swepeSFX;
    void Start()
    {
        _animator = GetComponent<Animator>();
        enemyLayerMask = LayerMask.GetMask("Enemy");
        _playerController = GetComponent<PlayerController>();
        _audioSource = _playerController.audioSource;
    }
    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, damageRadious, enemyLayerMask);
        if (Input.GetMouseButtonDown(0)&& !_playerController.isSwarding&& _playerController.isSwardCollected)
        {
            _playerController.isSwarding = true;
            _animator.SetTrigger("isSwarding");
            _audioSource.PlayOneShot(swepeSFX);
            if (hit != null)
            {
                Debug.Log(hit.gameObject.tag);
                _playerController.EnemyDamage(hit,20);
            }
            StartCoroutine(DisableIsSwarding());
        }
    }

    IEnumerator DisableIsSwarding()
    {
        yield return new WaitForSeconds(coolDown);
        _playerController.isSwarding = false;
    }
}
