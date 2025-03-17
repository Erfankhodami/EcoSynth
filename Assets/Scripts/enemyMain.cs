using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

public class enemyMain : MonoBehaviour
{
    public GameObject inkDrop;
    public enemyAILogic logic;
    public float timeBeforeSpawn = 0.5f;

    private void Start()
    {
        logic = gameObject.GetComponent<enemyAILogic>();
    }

    

    public IEnumerator onDie()
    {
        yield return StartCoroutine(spawn());
        Destroy(gameObject);
        Debug.Log("Ded");
    }

    IEnumerator spawn()
    {
        Instantiate(inkDrop, transform.position, Quaternion.identity);
        Collider2D inkCollider = inkDrop.GetComponent<BoxCollider2D>();
        inkCollider.isTrigger = true;
        yield return new WaitForSeconds(0.1f);
        inkCollider.isTrigger = false;
    }

    
}
