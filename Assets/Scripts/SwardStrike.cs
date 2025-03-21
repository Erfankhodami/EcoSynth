using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwardStrike : MonoBehaviour
{ 
    private PlayerController _playerController;
    private LayerMask enemyLayerMask;
    private Animator _animator;
    [SerializeField] private float damageRadious=2;
    [SerializeField] private float coolDown=1;
    void Start()
    {
        _animator = GetComponent<Animator>();
        enemyLayerMask = LayerMask.GetMask("Enemy");
        _playerController = GetComponent<PlayerController>();
    }
    void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, damageRadious, enemyLayerMask);
        if (Input.GetKeyDown(KeyCode.Tab)&& !_playerController.isSwarding&& _playerController.isSwardCollected)
        {
            _playerController.isSwarding = true;
            _animator.SetTrigger("isSwarding");
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
