using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemies : MonoBehaviour
{
    public static Enemies Instance;
    
    public List<GameObject> _enemies = new List<GameObject>();
    
    [Header("Spawn Settings")]
    [Tooltip("The enemy prefab to spawn.")]
    [SerializeField] private GameObject enemyPrefab;

    [Tooltip("How often (in seconds) to spawn a new enemy.")]
    [SerializeField] private float spawnInterval = 2.0f;

    [Tooltip("Distance BEYOND the camera border to spawn enemies.")]
    [SerializeField] private float spawnBufferDistance = 2.0f;

    [Header("Camera Reference")]
    [Tooltip("Main camera reference. If empty, uses Camera.main.")]
    [SerializeField] private Camera targetCamera;

    private float spawnTimer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemyOutsideCamera();
        }
    }

    private void AddEnemy(GameObject enemy)
    {
        _enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        _enemies.Remove(enemy);
        Destroy(enemy);
    }

    private void SpawnEnemyOutsideCamera()
    {
        if (enemyPrefab == null || targetCamera == null) return;

        var spawnPosition = GetRandomOffscreenPosition();
        var enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        AddEnemy(enemy);
    }

    private Vector3 GetRandomOffscreenPosition()
    {
        // 1. Pick a random edge: 0 = Top, 1 = Bottom, 2 = Left, 3 = Right
        int side = Random.Range(0, 4);

        // Viewport coordinates run from (0,0) [bottom-left] to (1,1) [top-right]
        Vector3 viewportPoint = Vector3.zero;

        switch (side)
        {
            case 0: // Top
                viewportPoint = new Vector3(Random.Range(0f, 1f), 1.1f, 0f);
                break;
            case 1: // Bottom
                viewportPoint = new Vector3(Random.Range(0f, 1f), -0.1f, 0f);
                break;
            case 2: // Left
                viewportPoint = new Vector3(-0.1f, Random.Range(0f, 1f), 0f);
                break;
            case 3: // Right
                viewportPoint = new Vector3(1.1f, Random.Range(0f, 1f), 0f);
                break;
        }

        // 2. Determine distance from camera plane (Z depth)
        // For 2D games, set this to 0. For 3D top-down games, use ground plane distance.
        float distanceToGround = Mathf.Abs(targetCamera.transform.position.y);
        //viewportPoint.z = targetCamera.orthographic ? targetCamera.farClipPlane * 0.5f : distanceToGround;
        viewportPoint.z = targetCamera.orthographic ? 0 : distanceToGround;

        // 3. Convert viewport point to World coordinates
        Vector3 worldPoint = targetCamera.ViewportToWorldPoint(viewportPoint);

        // 4. Offset slightly outward by spawnBufferDistance for extra safety
        Vector3 camToPoint = (worldPoint - targetCamera.transform.position);
        camToPoint.y = 0; // Lock elevation for 3D ground games (remove if 2D)
        worldPoint += camToPoint.normalized * spawnBufferDistance;

        return worldPoint;
    }
}
