using System.Collections.Generic;
using UnityEngine;

namespace Stls
{
    [CreateAssetMenu(menuName = "New Level Data", fileName = "New Level Data")]
    public class LevelData : ScriptableObject
    {
        public int GridWidth = 10;
        public int GridHeight = 10;
        public List<GridCoordinates> NotWalkableCells;

        public GridCoordinates PlayerPosition;
        public GridDirection PlayerDirection;
        public GridCoordinates ExitPosition;


        [System.Serializable]
        public class EnemyData
        {
            public GridCoordinates Position;
            public GridDirection Direction;
            [Header("First element must equals to Position")]
            public GridCoordinates[] PatrolWaypoints;
        }

        public List<EnemyData> Enemies;
    }
}