using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Level
{
    [CreateAssetMenu(order = 0, fileName = "BaseLevelData", menuName = "Level/Create Base Level Data")]
    public class BaseLevelData : ScriptableObject
    {
        public List<LevelData> levels;
    }
}