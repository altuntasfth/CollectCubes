using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Game.Scripts.Level
{
    [CreateAssetMenu(fileName = "Level_", menuName = "Level/Create Level Data")]
    public class LevelData : ScriptableObject
    {
        public GameObject voxelPrefab;
        public Texture voxelTexture;
        public float voxelOffset;
    }
}