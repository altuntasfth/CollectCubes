using System;
using _Game.Scripts.Level;
using FSM;
using UnityEngine;
using UnityEngine.AI;

namespace _Game.Scripts.AI
{
    public class AIMechanic : CharacterController
    {
        public NavMeshAgent agent;
        public float rotationSpeed = 10f;
        public int needToDropVoxelsAmount = 10;

        public bool isIdle;
        
        public StateMachine mainStateMachine;
        public bool isInitialized;

        [Range(0.5f, 1f)]
        public float challengingFactor = 0.5f;
        
        private int numberOfRay = 15;
        private float angle = 90f;
        private float targetVelocity = 10f;
        private float rayRange = 2f;

        public void Initialize()
        {
            characterType = CharacterType.AI;
            
            mainStateMachine = new StateMachine();
            
            mainStateMachine.AddState("Idle", new AIIdleState(this, false));
            mainStateMachine.AddState("Collect", new AICollectState(this, false));
            mainStateMachine.AddState("Drop", new AIDropState(this, false));
            
            mainStateMachine.AddTransition("Idle", "Collect", transition => 
                levelManager.voxels.Count != 0);
            mainStateMachine.AddTransition("Collect", "Drop", transition => 
                levelManager.voxels.Count < needToDropVoxelsAmount || voxelHolder.heldVoxels.Count >= needToDropVoxelsAmount);
            mainStateMachine.AddTransition("Drop", "Idle", transition => 
                levelManager.voxels.Count == 0 || IsClosestToCollector());
            
            mainStateMachine.SetStartState("Idle");
            mainStateMachine.Init();

            isInitialized = true;
        }

        private bool IsClosestToCollector()
        {
            float distance = Vector3.Distance(collector.transform.position, transform.position);

            return distance < 3f;
        }

        private void Update()
        {
            if (!isInitialized || levelManager == null)
            {
                return;
            }
            
            mainStateMachine.OnLogic();
        }

        private void ObstacleAvoidance()
        {
            var deltaPosition = Vector3.zero;
            for (int i = 0; i < numberOfRay; i++)
            {
                var rotation = transform.rotation;
                var rotationMode =
                    Quaternion.AngleAxis((i / ((float)numberOfRay - 1)) * angle * 2 - angle, transform.up);
                var direction = rotation * rotationMode * Vector3.forward;

                var ray = new Ray(transform.position, direction);
                RaycastHit hitInfo;

                if (Physics.Raycast(ray, out  hitInfo, rayRange))
                {
                    deltaPosition -= (1f / numberOfRay) * targetVelocity * direction;
                }
                else
                {
                    deltaPosition += (1f / numberOfRay) * targetVelocity * direction;
                }
            }
        }
    }
}