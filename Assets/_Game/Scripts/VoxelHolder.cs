using System;
using System.Collections;
using System.Collections.Generic;
using _Game.Scripts;
using UnityEngine;
using CharacterController = _Game.Scripts.CharacterController;

public class VoxelHolder : MonoBehaviour
{
    public CharacterController character;
    [SerializeField] private float gravityMultiplier = 200f;
    [SerializeField] private BoxCollider holderCollider;

    public List<VoxelEntity> heldVoxels;

    private void OnTriggerEnter(Collider other)
    {
        VoxelEntity voxel = other.gameObject.GetComponent<VoxelEntity>();
        if (voxel)
        {
            if (!voxel.isHeld && !voxel.isCollected)
            {
                voxel.isHeld = true;
                voxel.heldType = character.characterType;
            
                voxel.gameObject.layer = character.characterType == CharacterController.CharacterType.PLAYER
                    ? LayerMask.NameToLayer("HeldVoxelByPlayer")
                    : LayerMask.NameToLayer("HeldVoxelByAI");
            
                heldVoxels.Add(voxel);
            }
        }

        ObstacleEntity obstacle = other.gameObject.GetComponent<ObstacleEntity>();
        if (obstacle)
        {
            character.transform.position = character.initialPosition;
            character.transform.rotation = character.initialRotation;
            
            for (var i = 0; i < heldVoxels.Count; i++)
            {
                heldVoxels[i].isHeld = false;
                heldVoxels[i].gameObject.layer = LayerMask.NameToLayer("Voxel");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        VoxelEntity voxel = other.gameObject.GetComponent<VoxelEntity>();
        if (voxel)
        {
            if (voxel.isHeld && !voxel.isCollected)
            {
                float gravityIntensity = Vector3.Distance(transform.position, voxel.transform.position) / holderCollider.size.x / 2f;
                other.attachedRigidbody.AddForce((transform.position - voxel.transform.position) * gravityIntensity * gravityMultiplier * Time.fixedDeltaTime);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        VoxelEntity voxel = other.gameObject.GetComponent<VoxelEntity>();
        if (voxel)
        {
            if (voxel.isHeld)
            {
                voxel.isHeld = false;
                voxel.heldType = CharacterController.CharacterType.NULL;
            
                if (!voxel.isCollected)
                {
                    voxel.gameObject.layer = LayerMask.NameToLayer("Voxel");
                }
                else
                {
                    voxel.gameObject.layer = LayerMask.NameToLayer("CollectedVoxel");
                }
            
                heldVoxels.Remove(voxel);
            }
        }
    }
}
