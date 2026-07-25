using System;
using System.Collections;
using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class UIScript : MonoBehaviour
    {
        [SerializeField] private Button _restartGameButton;
        [SerializeField] private GameObject _gameOverPanel;
        [SerializeField] private IrisWipeController _irisWipeController;
        [SerializeField] private LevelsManager _levelsManager;
        
        [SerializeField] private CanvasGroup _openingScreenCanvasGroup;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private AudioMixerGroup _sfxGroup;

        private Coroutine _currentFadeCoroutine;

        private void Awake()
        {
            _restartGameButton.onClick.AddListener(RestartGameClicked);
            _gameOverPanel.SetActive(false);
        }

        private void Start()
        {
            SetSfxVolume(0);
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

        private bool _isGameStarted;
        private void Update()
        {
            if (!_isGameStarted && Keyboard.current.anyKey.wasPressedThisFrame)
            {
                _isGameStarted = true;
                StartGameClicked();
            }
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
            FadeOut();
        }
        
        /// <summary>
        /// Call this from a UI Button OnClick() or via script to trigger the fade out.
        /// </summary>
        public void FadeOut()
        {
            StartFade(0f);
        }

        public void FadeIn()
        {
            StartFade(1f);
        }

        private void StartFade(float targetAlpha)
        {
            // Stop any running fade to prevent overlapping coroutines
            if (_currentFadeCoroutine != null)
            {
                StopCoroutine(_currentFadeCoroutine);
            }

            _currentFadeCoroutine = StartCoroutine(FadeRoutine(targetAlpha));
        }

        private IEnumerator FadeRoutine(float targetAlpha)
        {
            float startAlpha = _openingScreenCanvasGroup.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime; // Use Time.unscaledDeltaTime if game is paused
                var t = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _fadeDuration);
                _openingScreenCanvasGroup.alpha = t;
                SetSfxVolume(1-t);
                yield return null; // Wait for next frame
            }

            // Guarantee exact final value
            _openingScreenCanvasGroup.alpha = targetAlpha;

            // Disable raycasts/clicks when fully transparent to prevent invisible button clicks
            bool isVisible = targetAlpha > 0f;
            _openingScreenCanvasGroup.interactable = isVisible;
            _openingScreenCanvasGroup.blocksRaycasts = isVisible;

            _currentFadeCoroutine = null;
        }
        
        /// <summary>
        /// Sets mixer group volume using a linear slider value (0.0 to 1.0).
        /// </summary>
        /// <param name="sliderValue">Value from 0.0001 to 1.0</param>
        public void SetSfxVolume(float sliderValue)
        {
            // Clamp to prevent Log10(0) which returns -Infinity
            sliderValue = Mathf.Clamp(sliderValue, 0.0001f, 1f);

            // Convert linear 0.0001..1.0 range into decibels (-80dB to 0dB)
            float dB = Mathf.Log10(sliderValue) * 20f;

            // Set the parameter exposed in the Audio Mixer
            _sfxGroup.audioMixer.SetFloat("SFXVolume", dB);
        }
    }
}
