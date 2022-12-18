using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelHolder : MonoBehaviour
{
    [SerializeField] private float gravityMultiplier = 200f;
    [SerializeField] private SphereCollider holderCollider;
    
    private void OnTriggerStay(Collider other)
    {
        VoxelEntity voxel = other.gameObject.GetComponent<VoxelEntity>();
        if (voxel)
        {
            float gravityIntensity = Vector3.Distance(transform.position, voxel.transform.position) / holderCollider.radius;
            other.attachedRigidbody.AddForce((transform.position - voxel.transform.position) * gravityIntensity * gravityMultiplier * Time.fixedDeltaTime);
        }
    }
}
