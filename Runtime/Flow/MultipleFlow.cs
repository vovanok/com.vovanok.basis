namespace UnityBasis.Flow
{
    public class MultipleFlow : Flow
    {
        private readonly Flow[] flows;

        public MultipleFlow(FlowService flowService, params Flow[] flows)
            : base(flowService)
        {
            this.flows = flows;

            if (flows.Length != 0)
            {
                flows[flows.Length - 1].OnComplete.AddListener(LastFlowCompleted);
            }
        }

        protected override void RunCurrent()
        {
            foreach (Flow flow in flows)
                flow.Run();
        }

        private void LastFlowCompleted()
        {
            if (flows.Length != 0)
                flows[flows.Length - 1].OnComplete.RemoveListener(LastFlowCompleted);
            
            OnComplete.Invoke();
        }
    }
}
