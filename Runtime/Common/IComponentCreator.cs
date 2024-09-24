using UnityEngine;

namespace UnityBasis.Common
{
    public interface IComponentCreator
    {
        T Instantiate<T>() where T : Component;
        void Destroy<T>(T component) where T : Component;
    }
}
