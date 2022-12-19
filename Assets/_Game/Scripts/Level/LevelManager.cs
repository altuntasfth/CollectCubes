using System;
using System.Collections.Generic;
using _Game.Scripts.AI;
using _Game.Scripts.Pool;
using Cinemachine.Utility;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game.Scripts.Level
{
    public class LevelManager : MonoBehaviour
    {
        public GameManager gameManager;
        public BaseLevelData baseLevelData;
        public Transform voxelOrigin;

        public List<GameObject> voxels;
        public int voxelCount;

        public float timer = 10f;
        public float voxelSpawnDelayTime = 0.1f;

        public GameObject ai;
        public GameObject aiCollector;

        public void Initialize()
        {
            switch (gameManager.gameMode)
            {
                case GameManager.GameMode.STANDARD:
                    GenerateStandardLevel();
                    break;
                
                case GameManager.GameMode.TIME:
                    GenerateTimerLevel();
                    break;
                
                case GameManager.GameMode.AI:
                    GenerateAILevel();
                    break;
            }
        }

        private void GenerateStandardLevel()
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

            voxelCount = voxels.Count;
        }

        public void GenerateTimerLevel()
        {
            
        }

        private void GenerateAILevel()
        {
            ai.SetActive(true);
            aiCollector.SetActive(true);
            ai.GetComponent<AIMechanic>().Initialize();
        }

        public void StartToSpawnVoxels(int spawnCountOnOneFrame)
        {
            DOVirtual.DelayedCall(voxelSpawnDelayTime, () =>
            {
                int xMultiplier = -8;
                for (int i = 0; i < spawnCountOnOneFrame; i++)
                {
                    if (spawnCountOnOneFrame == 1)
                    {
                        xMultiplier = 0;
                    }
                    GameObject voxel = PoolManager.Instance.Pool.Get();
                    voxelOrigin.transform.localPosition = Vector3.zero;
                    voxel.transform.parent = voxelOrigin;
                    voxel.transform.localPosition =
                        (Random.insideUnitSphere * 2f).ProjectOntoPlane(Vector3.up) +
                        xMultiplier * Vector3.right;

                    voxels.Add(voxel);

                    xMultiplier += 8;
                }
            }).SetLoops(Mathf.RoundToInt(timer / voxelSpawnDelayTime));
        }
    }
}