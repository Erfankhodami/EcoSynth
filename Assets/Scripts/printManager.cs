using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class printManager : MonoBehaviour
{
    public GameObject testPlatform;
    public Camera MainCamera;
    public bool isPlacing = false;
    public GameObject printerCanvas;

    private void Start()
    {
        MainCamera = Camera.main;
    }

    public void Update()
    {
        if (isPlacing && Input.GetMouseButtonDown(0))
        {
            place();
        }
    }

    public void place()
    {
        StartCoroutine(PlacePlatform());
        printerCanvas.SetActive(false);
        isPlacing = true;
    }

    IEnumerator PlacePlatform()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = MainCamera.ScreenToWorldPoint(mousePos);
        mousePos.z = 0;

        yield return Instantiate(testPlatform, mousePos, Quaternion.identity);
        isPlacing = false;
    }
}
