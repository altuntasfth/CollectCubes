using Cinemachine.Utility;
using UnityEngine;

namespace _Game.Scripts.AI
{
    public class AIDropState : AIBaseState
    {
        public AIDropState(AIMechanic ai, bool needsExitTime) : base(ai, needsExitTime)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            Debug.Log("Drop State");
        }

        public override void OnLogic()
        {
            base.OnLogic();
            
            Drop();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Drop()
        {
            Vector3 directionToCollector = (ai.collector.transform.position - ai.transform.position).normalized;
            ai.transform.forward = directionToCollector.ProjectOntoPlane(Vector3.up);
            ai.rb.velocity = directionToCollector * ai.velocityMultiplier * Time.fixedDeltaTime;
        }
    }
}