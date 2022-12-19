using UnityEngine;

namespace _Game.Scripts.AI
{
    public class AIIdleState : AIBaseState
    {
        public AIIdleState(AIMechanic ai, bool needsExitTime) : base(ai, needsExitTime)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            Debug.Log("Idle State");
        }

        public override void OnLogic()
        {
            base.OnLogic();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}