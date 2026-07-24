using System.Collections;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private LevelParameters[] _levelsParameters;

    [SerializeField] private Timer _timer;
    [SerializeField] private Enemies _enemies;
    [SerializeField] private KingProtection _kingProtection;

    private float _levelProgress;

    private void Awake()
    {
        SetLevel(_levelsParameters[0]); //todo
    }

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private void SetLevel(LevelParameters levelParameters)
    {
        _timer.Init(levelParameters._levelTimerTime, 
            levelParameters._timeToAddWhenEnemyDestroyed, 
            levelParameters._timeToReduceWhenPressedAndEnemyNotDestroyed, 
            levelParameters._timeToReduceWhenEnemyReachesKing);
        _enemies.Init(levelParameters._spawnInterval, levelParameters._enemyType);
        _kingProtection.Init(levelParameters._enemyType);
    }

    private IEnumerator StartGame()
    {
        foreach (var levelsParameter in _levelsParameters)
        {
            yield return StartCoroutine(StartLevel(levelsParameter));
        }
    }

    private IEnumerator StartLevel(LevelParameters levelParameters)
    {
        SetLevel(levelParameters);
        _enemies._toSpawn = false;
        yield return new WaitForSeconds(2f);
        _enemies._toSpawn = true;
        
        _levelProgress = 0;
        while (_levelProgress < levelParameters._levelDistance)
        {
            yield return null;
            _levelProgress += Time.deltaTime;
        }
    }
}
