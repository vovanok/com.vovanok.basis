using System.Collections.Generic;

namespace UnityBasis.Pooling
{
    public class PoolCluster<TCriteria, TObject> where TObject : class
    {
        private readonly Dictionary<TCriteria, Pool<TObject>> pools;
        private readonly Dictionary<TObject, Pool<TObject>> objectsPools; // Cache-map object -> pool
        private readonly int expandStep;
        private readonly IPoolClusterObjectManipulator<TCriteria, TObject> objectManipulator;

        public PoolCluster(int expandStep, IPoolClusterObjectManipulator<TCriteria, TObject> objectManipulator)
        {
            pools = new Dictionary<TCriteria, Pool<TObject>>();
            objectsPools = new Dictionary<TObject, Pool<TObject>>();

            this.expandStep = expandStep;
            this.objectManipulator = objectManipulator;
        }

        public TObject Spawn(TCriteria criteria)
        {
            if (criteria == null)
                return null;

            Pool<TObject> pool = GetOrCreatePool(criteria);

            TObject targetObject = pool.Spawn();
            if (targetObject == null)
            {
                pool.ExpandBy(expandStep);
                targetObject = pool.Spawn();
            }

            // Save pool for spawned object
            objectsPools[targetObject] = pool;
            return targetObject;
        }

        public bool Despawn(TObject objectForDispose)
        {
            if (objectForDispose == null || !objectsPools.ContainsKey(objectForDispose))
                return false;

            Pool<TObject> pool = objectsPools[objectForDispose];

            if (pool.Despawn(objectForDispose))
            {
                // Delete pool for freed object
                objectsPools.Remove(objectForDispose);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            foreach (KeyValuePair<TCriteria, Pool<TObject>> poolItem in pools)
                poolItem.Value.Clear();

            pools.Clear();
            objectsPools.Clear();
        }

        private Pool<TObject> GetOrCreatePool(TCriteria criteria)
        {
            if (pools.ContainsKey(criteria))
                return pools[criteria];

            // Create pool for criteria
            PoolObjectManipulator<TCriteria, TObject> poolObjectManipulator = 
                new PoolObjectManipulator<TCriteria, TObject>(criteria, objectManipulator);

            Pool<TObject> newPool = new Pool<TObject>(poolObjectManipulator);

            pools[criteria] = newPool;
            return newPool;
        }
    }
}
