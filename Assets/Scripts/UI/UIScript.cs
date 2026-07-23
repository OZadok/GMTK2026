using Events;
using SuperMaxim.Messaging;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIScript : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(StartGameClicked);
        }

        private void StartGameClicked()
        {
            Messenger.Default.Publish(new StartGameEvent());
        }
    }
}
