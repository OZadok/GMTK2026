using System.Collections;
using System.Collections.Generic;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsManager : MonoBehaviour
{
    [SerializeField] private LevelParameters[] _levelsParameters;

    [SerializeField] private Timer _timer;
    [SerializeField] private Enemies _enemies;
    [SerializeField] private KingProtection _kingProtection;
    [SerializeField] private KingMovement _kingMovement;
    [SerializeField] private UIProgressBar _progressBar;
    [SerializeField] private UiTimer _uiTimer;
    [SerializeField] private Angel _angel;
    
    [SerializeField] private AudioSource _successAudioSource;

    [SerializeField] private GameObject _gameOverGameObject;
    [SerializeField] private IrisWipeController _irisWipeController;

    private Vector3 _lastCheckPointPosition;
    private Coroutine _checkPointCoroutine;
    
    [SerializeField] private List<Castle> _castles = new List<Castle>();

    private float LevelProgress { get; set; }
    
    private void OnEnable()
    {
        Messenger.Default.Subscribe<StartGameEvent>(OnStartGame);
    }

    private void OnDisable()
    {
        Messenger.Default.Unsubscribe<StartGameEvent>(OnStartGame);
    }

    private void OnStartGame(StartGameEvent obj)
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
        
        var castleXPosition = _kingMovement.transform.position.x + levelParameters._levelDistance + 15.2f;
        if (_castles.Count == _lastLevel + 1)
        {
            _castles.Add(Instantiate(levelParameters._castlePrefab).GetComponent<Castle>());
        }
        var castle = _castles[_lastLevel + 1];
        var vector3 = castle.transform.position;
        vector3.x = castleXPosition;
        castle.transform.position = vector3;
        
        _progressBar.SwapBackgroundSprite(levelParameters._progressBarBackground);
    }

    private int _lastLevel;
    private bool _isLevelRunning = false;

    private IEnumerator StartGame()
    {
        _lastLevel = 0;
        while (_lastLevel < _levelsParameters.Length)
        {
            var levelsParameter = _levelsParameters[_lastLevel];
        
            _isLevelRunning = true;
            _checkPointCoroutine = StartCoroutine(RunLevelWrapper(levelsParameter));

            // Wait until the level finishes or is stopped
            while (_isLevelRunning)
            {
                yield return null;
            }

            _lastLevel++;
            if (_lastLevel < _levelsParameters.Length)
            {
                _successAudioSource.Play();
            }
        }

        StartCoroutine(BigCastle());
    }

    private IEnumerator BigCastle()
    {
        var xPosition = _kingMovement.transform.position.x;
        yield return new WaitUntil(() => xPosition + 4.5f <= _kingMovement.transform.position.x);
        _kingMovement.StopWalking();
        yield return new WaitForSeconds(1f);
        var flora = FindAnyObjectByType<Flora>();
        flora.Surprise();
        yield return new WaitForSeconds(0.5f);
        _angel.SetLevel(_lastLevel); //remove flowers from angel
        yield return new WaitForSeconds(1f);
        _gameOverGameObject.SetActive(true);
        _irisWipeController.PlayIrisIn(new Vector2(0.5f, (5.4f +  2.95f)/10.8f));
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator RunLevelWrapper(LevelParameters level)
    {
        yield return RunLevel(level);
        _isLevelRunning = false; // Normal completion
    }

    [ContextMenu("LoadCheckPoint")]
    public void LoadCheckPoint()
    {
        _lastLevel--;

        if (_checkPointCoroutine != null)
        {
            StopCoroutine(_checkPointCoroutine);
            _checkPointCoroutine = null;
        }

        // Tell StartGame that the level has finished/stopped so it moves past the loop!
        _isLevelRunning = false; 

        _kingMovement.transform.position = _lastCheckPointPosition;
        Messenger.Default.Publish(new LoadCheckPointEvent());
    }

    private IEnumerator RunLevel(LevelParameters levelParameters)
    {
        _lastCheckPointPosition = _kingMovement.transform.position;
        _enemies._toSpawn = false;
        _uiTimer.SwapFlowerSprite(levelParameters._flowersSprite);
        _angel.SetLevel(_lastLevel);
        yield return StartCoroutine(WalkCastleOut());
        Messenger.Default.Publish(new EndWalkInCastleEvent());
        SetLevel(levelParameters);
        LevelProgress = 0;
        Messenger.Default.Publish(new LevelProgressEvent(LevelProgress/levelParameters._levelDistance));
        StartCoroutine(SpawnEnemyAfterTime(4));
        while (LevelProgress < levelParameters._levelDistance - 4)
        {
            yield return null;
            LevelProgress += Time.deltaTime * _kingMovement._speed;
            Messenger.Default.Publish(new LevelProgressEvent(LevelProgress/levelParameters._levelDistance));
        }
        _enemies._toSpawn = false;
        while (LevelProgress < levelParameters._levelDistance)
        {
            yield return null;
            LevelProgress += Time.deltaTime * _kingMovement._speed;
            Messenger.Default.Publish(new LevelProgressEvent(LevelProgress/levelParameters._levelDistance));
        }
        
        yield return StartCoroutine(WalkCastleIn());
    }

    private IEnumerator SpawnEnemyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        _enemies._toSpawn = true;
    }

    private IEnumerator WalkCastleOut()
    {
        var xPosition = _kingMovement.transform.position.x;
        _kingMovement.StartWalkingStraight();
        yield return new WaitUntil(() => xPosition + 8.55f <= _kingMovement.transform.position.x);
        _castles[^1].ShowRightFlag();
        _kingMovement.StartWalkingDownStairs();
        yield return new WaitUntil(() => _kingMovement.transform.position.y <= -2);
        var vector3 = _kingMovement.transform.position;
        vector3.y = -2;
        _kingMovement.transform.position = vector3;
        _kingMovement.StartWalkingStraight();
    }
    
    private IEnumerator WalkCastleIn()
    {
        Messenger.Default.Publish(new StartWalkInCastleEvent());
        
        //walk stairs
        _kingMovement.StartWalkingUpStairs();
        yield return new WaitUntil(() => _kingMovement.transform.position.y >= 2.95f);
        var vector3 = _kingMovement.transform.position;
        vector3.y = 2.95f;
        _kingMovement.transform.position = vector3;
        var xPosition = _kingMovement.transform.position.x;
        _kingMovement.StartWalkingStraight();
        _castles[^1].ShowLeftFlag();
        yield return new WaitUntil(() => xPosition + 8.55f <= _kingMovement.transform.position.x);
    }
}
