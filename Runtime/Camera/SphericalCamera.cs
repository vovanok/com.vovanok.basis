using System;
using UnityEngine;
using UnityBasis.Data;

namespace UnityBasis.Camera
{
    public class SphericalCamera : MonoBehaviour
    {
        private SphrericalVector position;
        private SphrericalVector minPosition;
        private SphrericalVector maxPosition;
        private Vector3 observedPoint;
        private bool isNeedReposition;

        public SphrericalVector Position
        {
            get => position;
            set
            {
                ExecuteWithReposition(() => position = 
                    new SphrericalVector(
                        r: Mathf.Clamp(value.R, minPosition.R, maxPosition.R),
                        teta: Mathf.Clamp(value.Teta, minPosition.Teta, maxPosition.Teta),
                        fi: Mathf.Repeat(value.Fi, 360f)));
            }
        }
        
        public SphrericalVector MinPosition
        {
            get => minPosition;
            set
            {
                minPosition = value;
                Position = Position;
            }
        }
        
        public SphrericalVector MaxPosition
        {
            get => maxPosition;
            set
            {
                maxPosition = value;
                Position = Position;
            }
        }
        
        public Vector3 ObservedPoint
        {
            get => observedPoint;
            set
            {
                ExecuteWithReposition(() => observedPoint = value);
            }
        }

        public void ChangePosition(SphrericalVector deltaPosition)
        {
            Position += deltaPosition;
        }

        protected virtual void FixedUpdateInternal()
        {
            if (!isNeedReposition)
                return;

            isNeedReposition = false;

            transform.localPosition = ObservedPoint + Position.ToDecard();
            transform.LookAt(ObservedPoint);
        }

        private void FixedUpdate()
        {
            FixedUpdateInternal();
        }

        private void ExecuteWithReposition(Action executor)
        {
            executor();
            isNeedReposition = true;
        }
    }
}
