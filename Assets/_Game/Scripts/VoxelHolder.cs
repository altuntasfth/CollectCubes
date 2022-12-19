using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CharacterController = _Game.Scripts.CharacterController;

public class VoxelHolder : MonoBehaviour
{
    public CharacterController character;
    [SerializeField] private float gravityMultiplier = 200f;
    [SerializeField] private SphereCollider holderCollider;

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
    }

    private void OnTriggerStay(Collider other)
    {
        VoxelEntity voxel = other.gameObject.GetComponent<VoxelEntity>();
        if (voxel)
        {
            if (voxel.isHeld && !voxel.isCollected)
            {
                float gravityIntensity = Vector3.Distance(transform.position, voxel.transform.position) / holderCollider.radius;
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
                    voxel.gameObject.layer = LayerMask.NameToLayer("Default");
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
