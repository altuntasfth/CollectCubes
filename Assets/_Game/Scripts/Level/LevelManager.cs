using System;
using System.Collections.Generic;
using _Game.Scripts.Pool;
using UnityEngine;

namespace _Game.Scripts.Level
{
    public class LevelManager : MonoBehaviour
    {
        public GameManager gameManager;
        public BaseLevelData baseLevelData;
        public Transform voxelOrigin;

        public List<GameObject> voxels;

        private void Start()
        {
            GenerateLevel();
        }

        private void GenerateLevel()
        {
            int currentLevelIndex =  gameManager.levelIndex % baseLevelData.levels.Count;
            LevelData currentLevelData = baseLevelData.levels[currentLevelIndex];
            Texture2D texture = (Texture2D)currentLevelData.voxelTexture;
            float x = 0;
            
            for (int i = 0; i < texture.width; i++)
            {
                float z = 0f;
                for (int j = 0; j < texture.height; j++)
                {
                    Color voxelColor = texture.GetPixel(i, j);
                    
                    if (voxelColor.a != 0f)
                    {
                        GameObject voxel = PoolManager.Instance.Pool.Get();
                        voxel.transform.parent = voxelOrigin;
                        voxel.transform.localPosition = new Vector3(x, 0.5f, z);
                        voxel.GetComponent<MeshRenderer>().material.color = voxelColor;
                        
                        voxels.Add(voxel);
                    }
                    
                    z += currentLevelData.voxelOffset;
                }

                x += currentLevelData.voxelOffset;
            }
        }
    }
}