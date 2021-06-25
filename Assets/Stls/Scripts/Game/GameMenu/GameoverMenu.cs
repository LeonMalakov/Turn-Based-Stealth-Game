using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stls
{
    [RequireComponent(typeof(Canvas), typeof(Animator))]
    public class GameoverMenu : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private Animator _animator;

        public event Action RestartClicked;
        public event Action ExitClicked;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(OnRestartClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(OnRestartClicked);
            _exitButton.onClick.RemoveListener(OnExitClicked);
        }

        public void ShowWin()
        {
            _animator.Play(Constants.GameoverMenuAnimatorController.States.ShowWin);
        }

        public void ShowLoose()
        {
            _animator.Play(Constants.GameoverMenuAnimatorController.States.ShowLoose);
        }

        private void OnExitClicked()
        {
            ExitClicked?.Invoke();
        }

        private void OnRestartClicked()
        {
            RestartClicked?.Invoke();
        }
    }
}