using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public enum bridgeType
{
    small=0,
    medium=1,
    large=2
}
public class BridgeSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> bridges;
    private GameObject selectedBridge;
    public bool canInstantiate=true;

    public void PrepareSmallBridge()
    {
        SetSelectedBridge(bridgeType.small);
        InstantiateBridge();
    }
    
    public void PrepareMediumBridge()
    {
        SetSelectedBridge(bridgeType.medium);
        InstantiateBridge();
    }
    public void PrepareLargeBridge()
    {
        SetSelectedBridge(bridgeType.large);
        InstantiateBridge();
    }
    public void SetSelectedBridge(bridgeType type)
    {
        selectedBridge = bridges[(int)type];
    }

    void InstantiateBridge()
    {
        if (!canInstantiate)
        {
            return;
        }
        BridgeController br=Instantiate(selectedBridge).GetComponent<BridgeController>();
        br.isInPrepareMode = true;
        br.isSelectedBridge = true;
        br._bridgeSpawn = this;
        canInstantiate = false;
    }
}
