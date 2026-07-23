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

        private void Awake()
        {
            _startGameButton.onClick.AddListener(StartGameClicked);
            _restartGameButton.onClick.AddListener(RestartGameClicked);
            _gameOverPanel.SetActive(false);
        }

        private void OnEnable()
        {
            Messenger.Default.Subscribe<GameOverEvent>(OnGameOver);
        }

        private void OnDisable()
        {
            Messenger.Default.Unsubscribe<GameOverEvent>(OnGameOver);
        }

        private void OnGameOver(GameOverEvent obj)
        {
            _gameOverPanel.SetActive(true);
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
