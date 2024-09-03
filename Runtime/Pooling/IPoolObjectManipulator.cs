namespace UnityBasis.Pooling
{
    public interface IPoolObjectManipulator<TObject>
    {
        TObject CreateNew();
        void SwitchBusyState(TObject targetObject, PoolItemState state);
        void Destroy(TObject objectForDestroy);
    }
}
