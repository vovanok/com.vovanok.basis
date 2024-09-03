namespace UnityBasis.Pooling
{
    public class PoolObjectManipulator<TCriteria, TObject> : IPoolObjectManipulator<TObject>
    {
        private readonly TCriteria criteria;
        private readonly IPoolClusterObjectManipulator<TCriteria, TObject> objectManipulator;

        public PoolObjectManipulator(TCriteria criteria, IPoolClusterObjectManipulator<TCriteria, TObject> objectManipulator)
        {
            this.criteria = criteria;
            this.objectManipulator = objectManipulator;
        }

        public TObject CreateNew()
        {
            return objectManipulator.CreateNew(criteria);
        }

        public void SwitchBusyState(TObject targetObject, PoolItemState state)
        {
            objectManipulator.SwitchBusyState(targetObject, state);
        }

        public void Destroy(TObject objectForDestroy)
        {
            objectManipulator.Destroy(objectForDestroy);
        }
    }
}
