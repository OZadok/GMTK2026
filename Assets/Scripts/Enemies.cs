using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemies : MonoBehaviour
{
	public static Enemies Instance;

	private readonly Dictionary<EnemyType, List<Enemy>> _enemies = new Dictionary<EnemyType, List<Enemy>>();

	[Header("Spawn Settings")] [SerializeField]
	private List<GameObject> _enemyPrefabs;

	[Tooltip("How often (in seconds) to spawn a new enemy.")] [SerializeField]
	private float _spawnInterval = 2.0f;
	
	[SerializeField] private List<Transform> _spawnPoints = new List<Transform>();
/*
	[Tooltip("Distance BEYOND the camera border to spawn enemies.")] [SerializeField]
	private float _spawnBufferDistance = 2.0f;

	[Header("Camera Reference")] [Tooltip("Main camera reference. If empty, uses Camera.main.")] [SerializeField]
	private Camera _targetCamera;
	*/

	private float _spawnTimer;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Start()
	{
		// if (_targetCamera == null)
		// {
		// 	_targetCamera = Camera.main;
		// }

		_enemies.Add(EnemyType.Red, new List<Enemy>());
		_enemies.Add(EnemyType.Green, new List<Enemy>());
		_enemies.Add(EnemyType.Blue, new List<Enemy>());
	}

	private void Update()
	{
		_spawnTimer += Time.deltaTime;
		if (_spawnTimer >= _spawnInterval)
		{
			_spawnTimer = 0f;
			SpawnEnemy();
		}
	}

	private void AddEnemy(Enemy enemy, EnemyType enemyType)
	{
		_enemies[enemyType].Add(enemy);
	}

	public void RemoveEnemy(GameObject enemyGameObject)
	{
		var enemy = enemyGameObject.GetComponent<Enemy>();
		RemoveEnemy(enemy);
	}

	public void RemoveEnemy(Enemy enemy)
	{
		_enemies[enemy._type].Remove(enemy);
		Destroy(enemy.gameObject);
	}

	public List<Enemy> GetEnemies(EnemyType enemyType)
	{
		return _enemies[enemyType];
	}

	private void SpawnEnemy()
	{
		var enemyPrefabToSPawn = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
		var spawnPosition = GetRandomSpawnPosition();
		var enemy = Instantiate(enemyPrefabToSPawn, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
		AddEnemy(enemy, enemy._type);
	}

	private Vector3 GetRandomSpawnPosition()
	{
		return _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
	}

	// private void SpawnEnemyOutsideCamera()
	// {
	// 	if (_enemyPrefabs == null || _targetCamera == null) return;
	// 	var enemyPrefabToSPawn = _enemyPrefabs[Random.Range(0, _enemyPrefabs.Count)];
	//
	// 	var spawnPosition = GetRandomOffscreenPosition();
	// 	var enemy = Instantiate(enemyPrefabToSPawn, spawnPosition, Quaternion.identity).GetComponent<Enemy>();
	// 	AddEnemy(enemy, enemy._type);
	// }
	//
	// private Vector3 GetRandomOffscreenPosition()
	// {
	// 	// 1. Pick a random edge: 0 = Top, 1 = Bottom, 2 = Left, 3 = Right
	// 	int side = Random.Range(0, 4);
	//
	// 	// Viewport coordinates run from (0,0) [bottom-left] to (1,1) [top-right]
	// 	Vector3 viewportPoint = Vector3.zero;
	//
	// 	switch (side)
	// 	{
	// 		case 0: // Top
	// 			viewportPoint = new Vector3(Random.Range(0f, 1f), 1.1f, 0f);
	// 			break;
	// 		case 1: // Bottom
	// 			viewportPoint = new Vector3(Random.Range(0f, 1f), -0.1f, 0f);
	// 			break;
	// 		case 2: // Left
	// 			viewportPoint = new Vector3(-0.1f, Random.Range(0f, 1f), 0f);
	// 			break;
	// 		case 3: // Right
	// 			viewportPoint = new Vector3(1.1f, Random.Range(0f, 1f), 0f);
	// 			break;
	// 	}
	//
	// 	// 2. Determine distance from camera plane (Z depth)
	// 	// For 2D games, set this to 0. For 3D top-down games, use ground plane distance.
	// 	float distanceToGround = Mathf.Abs(_targetCamera.transform.position.y);
	// 	//viewportPoint.z = targetCamera.orthographic ? targetCamera.farClipPlane * 0.5f : distanceToGround;
	// 	viewportPoint.z = _targetCamera.orthographic ? 0 : distanceToGround;
	//
	// 	// 3. Convert viewport point to World coordinates
	// 	Vector3 worldPoint = _targetCamera.ViewportToWorldPoint(viewportPoint);
	//
	// 	// 4. Offset slightly outward by spawnBufferDistance for extra safety
	// 	Vector3 camToPoint = (worldPoint - _targetCamera.transform.position);
	// 	worldPoint += camToPoint.normalized * _spawnBufferDistance;
	// 	worldPoint.z = 0;
	// 	return worldPoint;
	// }
}