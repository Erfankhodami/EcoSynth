using System.Collections;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    private Transform playerTransform;
    private PlayerController _playerController;
    [SerializeField] private Vector2 bridgeOffset = new Vector2(3, 0);
    public BridgeSpawn _bridgeSpawn;
    public bool isInPrepareMode = true;
    public bool isSelectedBridge;
    private Vector2 offset;
    public Camera mainCamera;
    public int health = 100;
    //[SerializeField]private int decayRate = 10;
    public float destroyDelay = 1f;
    private bool isPlaced = false; // Ensure decay starts only after placement

    private SpriteRenderer bridgeSprite; // Reference to the bridge's sprite
    private Color originalColor; // Store original bridge color
    private float targetHealth; // For smooth Lerp effect

    private void Start()
    {
        mainCamera = Camera.main;
        cameraFollow cmf = mainCamera.GetComponent<cameraFollow>();

        // Get the bridge's sprite renderer
        bridgeSprite = GetComponent<SpriteRenderer>();

        if (bridgeSprite != null)
        {
            originalColor = bridgeSprite.color; // Save the original color
        }

        targetHealth = health; // Set initial target health
    }

    public void StartDecay() // Call this when platform is placed
    {
        if (!isPlaced)
        {
            isPlaced = true;
            InvokeRepeating(nameof(decreaseHealth), 1f, 1f); // Start health decay every second
        }
    }

    public void decreaseHealth()
    {
        health -= 10;
        targetHealth = health; // Update target health for smooth transition

        if (health <= 0)
        {
            CancelInvoke(nameof(decreaseHealth)); // Stop further health loss
            Destroy(gameObject, destroyDelay);
        }
    }

    private void Update()
    {
        if (bridgeSprite != null)
        {
            // Smooth Lerp effect for color change
            float healthPercentage = (float)health / 100f;

            // Change color from original to red based on health percentage
            bridgeSprite.color = Color.Lerp(Color.red, originalColor, healthPercentage);

            // Optional: Make it fade when health is low
            if (health <= 30)
            {
                bridgeSprite.color = new Color(bridgeSprite.color.r, bridgeSprite.color.g, bridgeSprite.color.b, Mathf.PingPong(Time.time, 1f)); // Flickering effect
            }
        }
    }
}
