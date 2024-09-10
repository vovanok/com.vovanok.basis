using System.IO;

namespace UnityBasis.Common
{
    public interface ISerializationProvider
    {
        void SerializeToStream<T>(T data, Stream stream);
        T DeserializeFromStream<T>(Stream stream) where T : new();
    }
}
