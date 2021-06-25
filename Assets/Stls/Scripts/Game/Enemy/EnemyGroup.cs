using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Stls
{
    public class EnemyGroup : MonoBehaviour
    {
        private List<Enemy> _enemies;
        public IReadOnlyList<Enemy> Enemies => _enemies;
        public bool IsAlarmed { get; private set; }

        public void Setup(List<Enemy> enemies)
        {
            _enemies = enemies;

            OnEnable();
        }

        private void OnEnable()
        {
            if (_enemies == null)
                return;

            foreach(Enemy enemy in _enemies)
                enemy.Alarmed += OnEnemyAlarmed;
        }

        private void OnDisable()
        {
            foreach (Enemy enemy in _enemies)
                enemy.Alarmed -= OnEnemyAlarmed;
        }

        public void PlayTurn()
        {
            foreach (Enemy enemy in _enemies)
                enemy.PlayTurn();
        }

        private void Alarm()
        {
            IsAlarmed = true;
        }

        private void OnEnemyAlarmed()
        {
            Alarm();
        }
    }
}