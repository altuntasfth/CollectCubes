using _Game.Scripts.Level;
using TMPro;
using UnityEngine;

namespace _Game.Scripts
{
    public abstract class CharacterController : MonoBehaviour
    {
        public enum CharacterType
        {
            AI,
            PLAYER,
            NULL
        }

        public CharacterType characterType;
        public Vector3 initialPosition;
        public Quaternion initialRotation;
        public LevelManager levelManager;
        public VoxelHolder voxelHolder;
        public CollectorMechanic collector;
        public Rigidbody rb;
        public TextMeshProUGUI collectedVoxelCounterTMP;
        public int collectedVoxelCount;
        public float velocityMultiplier = 1500f;
        public float rotateSpeed = 15f;

        public abstract void Initialize();
        
        private void OnCollisionEnter(Collision other)
        {
            ObstacleEntity obstacle = other.gameObject.GetComponent<ObstacleEntity>();
            if (obstacle)
            {
                transform.position = initialPosition;
                transform.rotation = initialRotation;
                
                for (var i = 0; i < voxelHolder.heldVoxels.Count; i++)
                {
                    voxelHolder.heldVoxels[i].isHeld = false;
                    voxelHolder.heldVoxels[i].gameObject.layer = LayerMask.NameToLayer("Voxel");
                }
            }
        }
    }
}