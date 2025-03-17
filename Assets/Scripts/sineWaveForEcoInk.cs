using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineWaveForEcoInk : MonoBehaviour
{
    public float floatspeed = 1f;
    public float flatAmount = 0.2f;

    private Vector3 startPos;
    [SerializeField] private BoxCollider2D _boxCollider2D;

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
        _boxCollider2D.isTrigger = true;
        yield return new WaitForSeconds(1);
        _boxCollider2D.isTrigger = false;
    }
}
