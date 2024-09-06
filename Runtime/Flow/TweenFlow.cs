using System;
using UnityEngine;
using UnityBasis.Common;

namespace UnityBasis.Flow
{
    public class TweenFlow : Flow
    {
        private readonly ApplicationLifecycle applicationLifecycle;
        private readonly float durationSec;
        private readonly Action<float> onUpdate;

        private float pastTime;
        private float factor;

        public TweenFlow(
                FlowService flowService, 
                ApplicationLifecycle applicationLifecycle, 
                TimeSpan duration, 
                Action<float> onUpdate)
            : base(flowService)
        {
            this.applicationLifecycle = applicationLifecycle;
            
            durationSec = (float)duration.TotalSeconds;
            this.onUpdate = onUpdate;
        }

        protected override void RunCurrent()
        {
            applicationLifecycle.OnUpdate
                .AddListener(DoOnUpdate);
        }

        private void DoOnUpdate(float deltaTimeSec)
        {
            factor = pastTime / durationSec;
            pastTime += Time.deltaTime;
            onUpdate(Mathf.Lerp(0, 1, factor));

            if (factor >= 1f)
            {
                applicationLifecycle.OnUpdate
                    .RemoveListener(DoOnUpdate);
                
                OnComplete.Invoke();
            }
        }
    }
}
