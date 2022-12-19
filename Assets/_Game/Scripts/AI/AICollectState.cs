using Cinemachine.Utility;
using UnityEngine;

namespace _Game.Scripts.AI
{
    public class AICollectState : AIBaseState
    {
        public AICollectState(AIMechanic ai, bool needsExitTime) : base(ai, needsExitTime)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            Debug.Log("Collect State");
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            CollectVoxels();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void CollectVoxels()
        {
            VoxelEntity closestVoxel = null;
            
            if (Time.frameCount % 60 == 0)
            {
                if (ai.levelManager.voxels.Count != 0)
                {
                    closestVoxel = ai.levelManager.voxels[Random.Range(0, ai.levelManager.voxels.Count - 1)].
                        GetComponent<VoxelEntity>();
                }
            }
            else
            {
                closestVoxel = GetClosestVoxel();
            }

            if (closestVoxel != null)
            {
                Vector3 directionToClosestVoxel = (closestVoxel.transform.position - ai.transform.position).normalized;
                ai.transform.forward = Vector3.Lerp(ai.transform.forward,
                    directionToClosestVoxel.ProjectOntoPlane(Vector3.up), ai.rotationSpeed * Time.deltaTime);
                ai.rb.velocity = directionToClosestVoxel * ai.velocityMultiplier * Time.fixedDeltaTime;
            }
        }

        private VoxelEntity GetClosestVoxel()
        {
            VoxelEntity closestVoxel = null;
            float minDistance = 99999f;
            
            for (var i = 0; i < ai.levelManager.voxels.Count; i++)
            {
                VoxelEntity voxel = ai.levelManager.voxels[i].GetComponent<VoxelEntity>();

                if (!voxel.isHeld)
                {
                    float distanceToVoxel = Vector3.Distance(voxel.transform.position, ai.transform.position);
                    if (distanceToVoxel < minDistance)
                    {
                        minDistance = distanceToVoxel;
                        closestVoxel = voxel;
                    }
                }
            }

            return closestVoxel;
        }
    }
}