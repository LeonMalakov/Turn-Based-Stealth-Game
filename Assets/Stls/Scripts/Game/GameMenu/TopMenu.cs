using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stls
{
    [RequireComponent(typeof(Canvas), typeof(Animator))]
    public class TopMenu : MonoBehaviour
    {
        [SerializeField] private Button _toggleButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;

        private Canvas _canvas;
        private Animator _animator;
        private bool _isExpanded;

        public event Action RestartClicked;
        public event Action ExitClicked;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            _toggleButton.onClick.AddListener(OnToggleClicked);
            _restartButton.onClick.AddListener(OnRestartClicked);
            _exitButton.onClick.AddListener(OnExitClicked);
        }
        private void OnDisable()
        {
            _toggleButton.onClick.RemoveListener(OnToggleClicked);
            _restartButton.onClick.RemoveListener(OnRestartClicked);
            _exitButton.onClick.RemoveListener(OnExitClicked);
        }

        public void Show()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }

        private void OnExitClicked()
        {
            ExitClicked?.Invoke();
        }

        private void OnRestartClicked()
        {
            RestartClicked?.Invoke();
        }

        private void OnToggleClicked()
        {
            Toggle();
        }

        private void Toggle()
        {
            _isExpanded = !_isExpanded;

            if (_isExpanded)
                _animator.Play(Constants.TopMenuAnimatorController.States.Expand);
            else
                _animator.Play(Constants.TopMenuAnimatorController.States.Shrink);
        }
    }
}