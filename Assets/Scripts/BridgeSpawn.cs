using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum bridgeType
{
    small = 0,
    medium = 1,
    large = 2
}

public class BridgeSpawn : MonoBehaviour
{
    [SerializeField] private List<GameObject> bridges;
    private GameObject selectedBridge;
    private GameObject previewBridge;
    public bool canInstantiate = true;
    public GameObject printCanvas;
    [SerializeField] private Camera mainCamera;
    private cameraFollow camFollow;
    private bool isPlacing = false;
    private Vector3 mouseWorldPos;
    public playerMain _playermain;
    public Text errorText;
    public int minForSmall;
    public int minForMedium;
    public int minForLarge;
    public PlayerController _prc;
    
    public float normalZoom = 5f;
    public float printZoom = 8f; // Default zoom-out value
    public float minZoom = 5f; // Minimum zoom level
    public float maxZoom = 12f; // Maximum zoom level
    public float zoomSpeed = 2f; // Scroll zoom speed
    
    public float panSpeed = 0.5f; // How fast the camera moves when dragging
    public Vector2 panLimitMin = new Vector2(-10, -5); // Minimum camera pan limits
    public Vector2 panLimitMax = new Vector2(10, 5); // Maximum camera pan limits

    public Color previewColor = new Color(1f, 1f, 1f, 0.5f);
    private Vector3 playerStartPos;
    private Vector3 originalPreviewScale; // Store the original preview size
    private Vector3 lastMousePosition; // Store last mouse position for dragging

    void Start()
    {
        mainCamera = Camera.main;
        camFollow = mainCamera.GetComponent<cameraFollow>();
        
    }

    public void PrepareSmallBridge()
    {
        if (_playermain.numberofEcoInk >= minForSmall)
        {
            canInstantiate = true;
            SetSelectedBridge(bridgeType.small);
            StartPlacementMode();
        }
        else
        {
            canInstantiate = false;
            errorText.text = "NOT ENOUGH ECOINK! COLLECT MORE";
            
        }
        
    }

    public void PrepareMediumBridge()
    {
        if (_playermain.numberofEcoInk >= minForMedium)
        {
            canInstantiate = true;
            SetSelectedBridge(bridgeType.medium);
            StartPlacementMode();
        }
        else
        {
            canInstantiate = false;
            errorText.text = "NOT ENOUGH ECOINK! COLLECT MORE!";
            
        }
        
    }

    public void PrepareLargeBridge()
    {
        if (_playermain.numberofEcoInk >= minForLarge)
        {
            canInstantiate = true;
            SetSelectedBridge(bridgeType.large);
            StartPlacementMode();
        }
        else
        {   
            canInstantiate = false;
            errorText.text = "NOT ENOUGH ECOINK! COLLECT MORE!";
            
        }
        
    }

    public void SetSelectedBridge(bridgeType type)
    {
        selectedBridge = bridges[(int)type];
    }

    void StartPlacementMode()
    {
        if (!canInstantiate) return;

        printCanvas.SetActive(false);
        isPlacing = true;
        camFollow.canFollow = false; // Stop camera from following

        playerStartPos = camFollow.playerObject.position; // Store player position

        StartCoroutine(ZoomCamera(printZoom, 0.5f));

        // Spawn a faded preview of the bridge
        previewBridge = Instantiate(selectedBridge);
        originalPreviewScale = previewBridge.transform.localScale; // Store original scale
        SetPreviewMode(previewBridge, true);
    }

    void Update()
    {
        if (isPlacing)
        {
            UpdatePreviewPosition();
            HandleScrollZoom();

            if (Input.GetMouseButtonDown(1)) // Right Click to place
            {
                int ecoInkCost = 0;
                if (selectedBridge == bridges[(int) bridgeType.small])
                {
                    ecoInkCost = minForSmall;
                }
                else if (selectedBridge == bridges[(int)bridgeType.medium])
                {
                    ecoInkCost = minForMedium;
                }
                else if(selectedBridge == bridges[(int)bridgeType.large])
                {
                    ecoInkCost = minForLarge;
                }
                PlaceBridge(ecoInkCost);
            }
        }
    }

    void UpdatePreviewPosition()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;

        if (previewBridge != null)
        {
            previewBridge.GetComponent<BoxCollider2D>().isTrigger = true;
            previewBridge.transform.position = mouseWorldPos;
            previewBridge.transform.localScale = originalPreviewScale; // Keep its size the same
        }
    }

    void HandleScrollZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Get scroll input

        if (scroll != 0f)
        {
            float newZoom = mainCamera.orthographicSize - scroll * zoomSpeed; // Zooms in when scrolling up
            mainCamera.orthographicSize = Mathf.Clamp(newZoom, minZoom, maxZoom);
        }
    }

    /*
    void HandleMousePan()
    {
        if (Input.GetMouseButtonDown(2)) // Middle Click starts dragging
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)) // Holding Middle Click drags the camera
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            Vector3 move = new Vector3(-delta.x * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);

            Vector3 newPosition = mainCamera.transform.position + move;
            newPosition.x = Mathf.Clamp(newPosition.x, panLimitMin.x, panLimitMax.x);
            newPosition.y = Mathf.Clamp(newPosition.y, panLimitMin.y, panLimitMax.y);
            mainCamera.transform.position = newPosition;

            lastMousePosition = Input.mousePosition;
        }
    }
    */

    void PlaceBridge(int ecoInkCost)
    {
        GameObject newPlatform = Instantiate(selectedBridge, mouseWorldPos, Quaternion.identity);
        newPlatform.GetComponent<BridgeController>().StartDecay(); // Start health decay after placement
        int newNumberOfEcoInk = _playermain.numberofEcoInk -= ecoInkCost;
        Destroy(previewBridge);
        isPlacing = false;
        StartCoroutine(ZoomCamera(normalZoom, 0.5f));
        camFollow.canFollow = true;
        _playermain.showPlayerCanvas();
        _playermain.ecoIndicator.text = "x" + newNumberOfEcoInk;

    }

    void SetPreviewMode(GameObject obj, bool isPreview)
    {
        if (isPreview)
        {
            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = previewColor;
            }
        }
    }

    IEnumerator ZoomCamera(float targetZoom, float duration)
    {
        float startZoom = mainCamera.orthographicSize;
        float time = 0;

        while (time < duration)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startZoom, targetZoom, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        mainCamera.orthographicSize = targetZoom;
    }
}
