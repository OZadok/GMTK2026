using System.Collections;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private LevelParameters[] _levelsParameters;

    [SerializeField] private Timer _timer;
    [SerializeField] private Enemies _enemies;
    [SerializeField] private KingProtection _kingProtection;
    [SerializeField] private KingMovement _kingMovement;
    
    [SerializeField] private GameObject _castle;

    private float _levelProgress;

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
        
        var castleXPosition = _kingMovement.transform.position.x + levelParameters._levelDistance + 19.2f;
        var castleGameObject = Instantiate(_castle);
        var vector3 = castleGameObject.transform.position;
        vector3.x = castleXPosition;
        castleGameObject.transform.position = vector3;
    }

    private IEnumerator StartGame()
    {
        foreach (var levelsParameter in _levelsParameters)
        {
            yield return StartCoroutine(RunLevel(levelsParameter));
        }
    }

    private IEnumerator RunLevel(LevelParameters levelParameters)
    {
        _enemies._toSpawn = false;
        yield return StartCoroutine(WalkCastleOut());
        Messenger.Default.Publish(new EndWalkInCastleEvent());
        SetLevel(levelParameters);
        yield return new WaitForSeconds(4f);
        _enemies._toSpawn = true;
        
        _levelProgress = 0;
        while (_levelProgress < levelParameters._levelDistance - 4)
        {
            yield return null;
            _levelProgress += Time.deltaTime;
        }
        _enemies._toSpawn = false;
        
        yield return new WaitForSeconds(4f);
        
        yield return StartCoroutine(WalkCastleIn());
    }

    private IEnumerator WalkCastleOut()
    {
        var xPosition = _kingMovement.transform.position.x;
        _kingMovement._walkingState = KingMovement.WalkingState.Straight;
        yield return new WaitUntil(() => xPosition + 8.55f <= _kingMovement.transform.position.x);
        _kingMovement._walkingState = KingMovement.WalkingState.Down;
        yield return new WaitUntil(() => _kingMovement.transform.position.y <= -2);
        var vector3 = _kingMovement.transform.position;
        vector3.y = -2;
        _kingMovement.transform.position = vector3;
        _kingMovement._walkingState = KingMovement.WalkingState.Straight;
    }
    
    private IEnumerator WalkCastleIn()
    {
        Messenger.Default.Publish(new StartWalkInCastleEvent());
        
        //walk stairs
        _kingMovement._walkingState = KingMovement.WalkingState.Up;
        yield return new WaitUntil(() => _kingMovement.transform.position.y >= 2.95f);
        var vector3 = _kingMovement.transform.position;
        vector3.y = 2.95f;
        _kingMovement.transform.position = vector3;
        var xPosition = _kingMovement.transform.position.x;
        _kingMovement._walkingState = KingMovement.WalkingState.Straight;
        yield return new WaitUntil(() => xPosition + 8.55f <= _kingMovement.transform.position.x);
    }
}
