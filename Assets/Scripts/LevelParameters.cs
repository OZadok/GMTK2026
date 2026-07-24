using UnityEngine;

[CreateAssetMenu(fileName = "LevelParameters", menuName = "Scriptable Objects/LevelParameters")]
public class LevelParameters : ScriptableObject
{
	public EnemyType _enemyType;
	public float _levelTimerTime = 60;
	public float _timeToAddWhenEnemyDestroyed = 5;
	public float _timeToReduceWhenPressedAndEnemyNotDestroyed = 1;
	public float _timeToReduceWhenEnemyReachesKing = 2;
	public float _spawnInterval = 2;
}
