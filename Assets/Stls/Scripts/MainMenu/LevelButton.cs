using System;
using UnityEngine;
using UnityEngine.UI;

namespace Stls
{
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        [SerializeField] private LevelData _levelData;

        public event Action<LevelData> Clicked;

        private void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(OnClicked);
        }

        private void OnDisable()
        {
            GetComponent<Button>().onClick.RemoveListener(OnClicked);
        }

        private void OnClicked()
        {
            Clicked?.Invoke(_levelData);
        }
    }
}
