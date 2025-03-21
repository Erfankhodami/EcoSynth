using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SwardSpawner : MonoBehaviour
{
    [SerializeField] private GameObject swardPrefab;
    private void OnDestroy()
    {
        Instantiate(swardPrefab, transform.position, quaternion.identity);
    }
}
