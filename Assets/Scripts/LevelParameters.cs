using UnityEngine;

[CreateAssetMenu(fileName = "LevelParameters", menuName = "Scriptable Objects/LevelParameters")]
public class LevelParameters : ScriptableObject
{
	[Header("References")]
	public GameObject _castlePrefab;
	public Sprite _progressBarBackground;
	public Sprite _flowersSprite;
	
	[Header("Parameters")]
	public EnemyType _enemyType;
	public float _levelTimerTime = 60;
	public float _timeToAddWhenEnemyDestroyed = 5;
	public float _timeToReduceWhenPressedAndEnemyNotDestroyed = 1;
	public float _timeToReduceWhenEnemyReachesKing = 2;
	public float _spawnInterval = 2;
	[Tooltip("it's also level time")] 
	public float _levelDistance = 60;

	
}
