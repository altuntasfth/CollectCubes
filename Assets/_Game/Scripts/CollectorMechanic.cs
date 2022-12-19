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
        public SphereCollider sphereCollider;
        public Color color;
        public UIManager uiManager;
        public CharacterController character;
        public float forceToOrigin = 3f;

        private void Update()
        {
            for (var i = 0; i < character.voxelHolder.heldVoxels.Count; i++)
            {
                VoxelEntity voxel = character.voxelHolder.heldVoxels[i];
                if (voxel.heldType == character.characterType)
                {
                    float distance = Vector3.Distance(voxel.transform.position, transform.position);
                    if (distance <= sphereCollider.radius + 1f)
                    {
                        Collect(voxel);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            VoxelEntity voxel = other.gameObject.GetComponent<VoxelEntity>();
            if (voxel && !voxel.isCollected)
            {
                if (voxel.heldType == character.characterType)
                {
                    Collect(voxel);
                }
            }
        }

        private void Collect(VoxelEntity voxel)
        {
            voxel.isCollected = true;
            voxel.gameObject.layer = LayerMask.NameToLayer("CollectedVoxel");
            voxel.meshRenderer.material.color = color;
                
                
            voxel.rb.AddForce((transform.position - voxel.transform.position) * forceToOrigin, ForceMode.Impulse);
                
            levelManager.voxels.Remove(voxel.gameObject);
            character.collectedVoxelCount++;
            character.collectedVoxelCounterTMP.text = (character.collectedVoxelCount).ToString();

            if (levelManager.voxels.Count == 0)
            {
                gameManager.HandleLevelComplete();
            }
        }
    }
}