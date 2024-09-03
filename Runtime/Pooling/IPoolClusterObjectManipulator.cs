namespace UnityBasis.Pooling
{
    public interface IPoolClusterObjectManipulator<TCriteria, TObject>
    {
        TObject CreateNew(TCriteria criteria);
        void SwitchBusyState(TObject targetObject, PoolItemState state);
        void Destroy(TObject objectForDestroy);
    }
}
