using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerController _playerController;
    [SerializeField] private Vector2 bridgeOffset=new Vector2(3,0);
    public BridgeSpawn _bridgeSpawn;
    public bool isInPrepareMode=true;
    public bool isSelectedBridge;
    private Vector2 offset;
    private void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        _playerController = playerTransform.GetComponent<PlayerController>();
        GetComponent<SpriteRenderer>().color=Color.green;
        offset = bridgeOffset;
    }
    void Update()
    { 
        if (_playerController.movingControll == 1|| _playerController.movingControll==-1)
        {
            offset = bridgeOffset;
            offset.x = _playerController.movingControll*bridgeOffset.x;
        }
        
        if (isInPrepareMode)
        {
            transform.position = playerTransform.position + (Vector3) offset;
        }
        if (Input.GetKeyDown(KeyCode.Tab)&& isSelectedBridge)
        {
            isInPrepareMode = false;
            GetComponent<SpriteRenderer>().color=Color.white;
            isSelectedBridge = false;
            _bridgeSpawn.canInstantiate = true;
        }
    }
}
