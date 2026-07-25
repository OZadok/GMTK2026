using System;
using Events;
using SuperMaxim.Messaging;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIScript : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private IrisWipeController _irisWipeController;
        [SerializeField] private LevelsManager _levelsManager;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(StartGameClicked);
            _restartGameButton.onClick.AddListener(RestartGameClicked);
            _gameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            Messenger.Default.Subscribe<GameOverEvent>(OnGameOver);
            Messenger.Default.Subscribe<LoadCheckPointEvent>(OnLoadCheckPoint);
        }

        private void OnDisable()
        {
            Messenger.Default.Unsubscribe<GameOverEvent>(OnGameOver);
            Messenger.Default.Unsubscribe<LoadCheckPointEvent>(OnLoadCheckPoint);
        }

        private void OnLoadCheckPoint(LoadCheckPointEvent obj)
        {
            //_gameOverPanel.SetActive(false);
        }

        private void OnGameOver(GameOverEvent obj)
        {
            _gameOverPanel.SetActive(true);
            
            _irisWipeController.PlayIrisOut(new Vector2(0.5f, (5.4f -  2f)/10.8f), LoadCheckPoint);
        }

        private void LoadCheckPoint()
        {
            _levelsManager.LoadCheckPoint();
            _irisWipeController.PlayIrisIn(new Vector2(0.5f, (5.4f +  2.95f)/10.8f));
        }

        private void RestartGameClicked()
        {
            SceneManager.LoadScene(0);
        }

        private void StartGameClicked()
        {
            Messenger.Default.Publish(new StartGameEvent());
        }
    }
}
