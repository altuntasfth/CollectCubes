using System;
using _Game.Scripts.Level;
using FSM;
using UnityEngine;

namespace _Game.Scripts.AI
{
    public class AIMechanic : CharacterController
    {
        public float rotationSpeed = 10f;
        public int needToDropVoxelsAmount = 10;

        public bool isIdle;
        
        public StateMachine mainStateMachine;
        public bool isInitialized;

        public void Initialize()
        {
            isPlayer = false;
            
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
    }
}