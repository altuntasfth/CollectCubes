using System;
using _Game.Scripts.Level;
using DG.Tweening;
using UnityEngine;

namespace _Game.Scripts
{
    public class CollectorMechanic : MonoBehaviour
    {
        public GameManager gameManager;
        public LevelManager levelManager;
        public Color color;
        public UIManager uiManager;
        public PlayerMechanic player;
        public float forceToOrigin = 3f;
        
        public SphereCollider sphereCollider;

        private void OnTriggerEnter(Collider other)
        {
            VoxelEntity voxel = other.gameObject.GetComponent<VoxelEntity>();
            if (voxel && !voxel.isCollected)
            {
                voxel.isCollected = true;
                voxel.gameObject.layer = LayerMask.NameToLayer("CollectedVoxel");
                voxel.meshRenderer.sharedMaterial.color = color;
                
                
                voxel.rb.AddForce((transform.position - voxel.transform.position) * forceToOrigin, ForceMode.Impulse);
                
                levelManager.voxels.Remove(voxel.gameObject);
                player.collectedVoxelCount++;
                uiManager.gameplayMoneyBarText.text = 
                    (uiManager.totalMoneyAmount + player.collectedVoxelCount).ToString();

                if (levelManager.voxels.Count == 0)
                {
                    gameManager.HandleLevelComplete();
                }
            }
        }
    }
}