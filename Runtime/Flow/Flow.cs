using System;
using UnityEngine.Events;

namespace UnityBasis.Flow
{
    public abstract class Flow
    {
        public readonly UnityEvent OnComplete = new();

        private readonly FlowService flowService;
        private Flow previousFlow;
        private Flow nextFlow;

        public Flow(FlowService flowService)
        {
            this.flowService = flowService;
        }
        
        public Flow ThenPause(TimeSpan duration)
        {
            return SetNextFlow(flowService.Pause(duration));
        }

        public Flow ThenDo(Action action)
        {
            return SetNextFlow(flowService.Do(action));
        }

        public Flow ThenTween(TimeSpan duration, Action<float> action)
        {
            return SetNextFlow(flowService.Tween(duration, action));
        }

        public Flow ThenFlow(Flow nextFlow)
        {
            return SetNextFlow(nextFlow);
        }

        public Flow ThenFlow(Func<Flow> getFlow)
        {
            return SetNextFlow(getFlow());
        }

        public Flow ThenDoMultiple(params Flow[] flows)
        {
            return SetNextFlow(flowService.DoMultiple(flows));
        }

        public void Run()
        {
            if (previousFlow != null)
                previousFlow.Run();
            else
                RunCurrent();
        }

        protected abstract void RunCurrent();

        private Flow SetNextFlow(Flow flow)
        {
            flow.previousFlow = this;
            nextFlow = flow;
            OnComplete.AddListener(CompleteHandler);
            return flow;
        }

        private void CompleteHandler()
        {
            OnComplete.RemoveListener(CompleteHandler);
            nextFlow.RunCurrent();
        }
    }
}
