using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class playerMain : MonoBehaviour
{
    public int numberofEcoInk = 0;
    public int health = 100;
    public Text ecoIndicator;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("ecoInk"))
        {
            numberofEcoInk += 1;
            ecoIndicator.text = "x" + numberofEcoInk;
            Destroy(other.gameObject);
            Debug.Log("Gained Ink to print!");
        }
    }
}
