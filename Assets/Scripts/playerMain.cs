using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMain : MonoBehaviour
{
    public int numberofEcoInk = 0;
    public Text ecoIndicator;
    public GameObject printCanvas;
    public GameObject playerCanvas;
    public bool printCanvasActive = false;
    public GameObject mainCanvas;
    public MainManager _mainManager;

    private void Start()
    {
        _mainManager = GameObject.Find("MainManager").GetComponent<MainManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))           
        {
            if (printCanvasActive)
            {
                showPlayerCanvas();
            }
            else
            {
                showPrintCanvas();
            }
        }
    }

    //moved ink collision check to playercontroller script
    public void UpdateInkAmount()
    {
        ecoIndicator.text = "x" + numberofEcoInk;
        Debug.Log("Gained Ink to print!");
        _mainManager.numberOfEcoInk = numberofEcoInk;
    }

    //is this needed to be an ienumerator method?
    /*IEnumerator destroy(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(obj);
    }*/

    public void showPrintCanvas()
    {   
        mainCanvas.SetActive(true);
        printCanvasActive = true;
        playerCanvas.SetActive(false);
        printCanvas.SetActive(true);
    }

    public void showPlayerCanvas()
    {   
        mainCanvas.SetActive(true);
        printCanvasActive = false;
        playerCanvas.SetActive(true);
        printCanvas.SetActive(false);
    }
}