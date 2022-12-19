using _Game.Scripts.Level;
using TMPro;
using UnityEngine;

namespace _Game.Scripts
{
    public class CharacterController : MonoBehaviour
    {
        public LevelManager levelManager;
        public VoxelHolder voxelHolder;
        public CollectorMechanic collector;
        public Rigidbody rb;
        public TextMeshProUGUI collectedVoxelCounterTMP;
        public int collectedVoxelCount;
        public float velocityMultiplier = 1500f;
        public float rotateSpeed = 15f;
        public bool isPlayer;
    }
}