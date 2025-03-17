using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyMain : MonoBehaviour
{
    public GameObject inkDrop;
    public enemyAILogic logic;

    private void Start()
    {
        logic = gameObject.GetComponent<enemyAILogic>();
    }

    private void Update()
    {
        if (logic.isDead)
        {
            StartCoroutine(onDie());
        }
    }

    IEnumerator onDie()
    {
        Destroy(gameObject);
        yield return new WaitForSeconds(2);
        Instantiate(inkDrop, transform.position, Quaternion.identity);
        Debug.Log("Ded");
    }
}
