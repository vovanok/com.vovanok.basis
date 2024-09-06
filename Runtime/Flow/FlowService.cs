using System;
using UnityBasis.Common;

namespace UnityBasis.Flow
{
    public class FlowService
    {
        private readonly ApplicationLifecycle applicationLifecycle;

        public FlowService(ApplicationLifecycle applicationLifecycle)
        {
            this.applicationLifecycle = applicationLifecycle;
        }

        public Flow Pause(TimeSpan duration)
        {
            return new PauseFlow(this, applicationLifecycle, duration);
        }

        public Flow Do(Action action)
        {
            return new DoFlow(this, action);
        }

        public Flow Tween(TimeSpan duration, Action<float> action)
        {
            return new TweenFlow(this, applicationLifecycle, duration, action);
        }

        public Flow DoMultiple(params Flow[] flows)
        {
            return new MultipleFlow(this, flows);
        }
    }
}
