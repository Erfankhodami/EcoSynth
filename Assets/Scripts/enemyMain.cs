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

    private void Start()
    {
        logic = gameObject.GetComponent<enemyAILogic>();
    }
    
    public void onDie()
    {   
        logic.enemySource.PlayOneShot(logic.dieSFX);
        Destroy(gameObject);
        SpawnInk();
        Debug.Log("Ded");
    }

    void SpawnInk()
    {
        Instantiate(inkDrop, transform.position, Quaternion.identity);
    }

    
}
