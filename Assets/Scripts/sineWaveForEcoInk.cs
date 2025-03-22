using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineWaveForEcoInk : MonoBehaviour
{
    public float floatspeed = 1f;
    public float flatAmount = 0.2f;

    private Vector3 startPos;
    public bool isCollactable = false;
    private void Start()
    {
        startPos = transform.position;
        StartCoroutine(EnableTrigger());
    }

    private void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatspeed) * flatAmount;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }

    IEnumerator EnableTrigger()
    {
        yield return new WaitForSeconds(1);
        isCollactable = true;
    }
}
