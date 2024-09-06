using System;

namespace UnityBasis.Flow
{
    public class DoFlow : Flow
    {
        private readonly Action action;

        public DoFlow(FlowService flowService, Action action)
            : base(flowService)
        {
            this.action = action;
        }

        protected override void RunCurrent()
        {
            action();
            OnComplete.Invoke();
        }
    }
}
