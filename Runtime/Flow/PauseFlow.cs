using System;
using UnityBasis.Common;

namespace UnityBasis.Flow
{
    public class PauseFlow : Flow
    {
        private readonly ApplicationLifecycle applicationLifecycle;
        private readonly TimeSpan duration;

        public PauseFlow(FlowService flowService, ApplicationLifecycle applicationLifecycle, TimeSpan duration)
            : base(flowService)
        {
            this.applicationLifecycle = applicationLifecycle;
            this.duration = duration;
        }

        protected override void RunCurrent()
        {
            applicationLifecycle.RunAfterDelay(
                duration, 
                () => OnComplete.Invoke());
        }
    }
}
