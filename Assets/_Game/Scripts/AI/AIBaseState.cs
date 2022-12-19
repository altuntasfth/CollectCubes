using System;
using FSM;

namespace _Game.Scripts.AI
{
    public class AIBaseState : StateBase
    {
        protected ITimer timer;
        protected AIMechanic ai;
        protected Func<StateBase, bool> canExit;
        
        public AIBaseState(AIMechanic ai, bool needsExitTime) : base(needsExitTime)
        {
            this.timer = new Timer();
            this.ai = ai;
        }
        
        public override void OnEnter()
        {
            base.OnEnter();

            timer.Reset();
        }
        
        public override void OnLogic()
        {
            base.OnLogic();
        }
        
        public override void OnExitRequest()
        {
            base.OnExitRequest();
            if (!needsExitTime || canExit != null && canExit(this))
            {
                fsm.StateCanExit();
            }
        }
    }
}