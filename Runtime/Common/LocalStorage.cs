using System;
using System.IO;
using UnityEngine;

namespace UnityBasis.Common
{
    public abstract class LocalStorage
    { 
        protected T Load<T>(string filepath, ISerializationProvider serializationProvider) where T : new()
        {
            if (!File.Exists(filepath))
                return default(T);

            try
            {
                using FileStream input = File.OpenRead(filepath);
                return serializationProvider.DeserializeFromStream<T>(input);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error read data from {filepath}\n{ex.Message}");
                return default(T);
            }
        }

        protected T LoadOrCreate<T>(string filepath, ISerializationProvider serializationProvider) where T : new()
        {
            var data = Load<T>(filepath, serializationProvider);
            if (data == null)
            {
                data = new T();
                Save(data, filepath, serializationProvider);
            }

            return data;
        }
        
        protected void Save<T>(T data, string filepath, ISerializationProvider serializationProvider) where T: new()
        {
            try
            {
                using FileStream output = File.Create(filepath);
                serializationProvider.SerializeToStream(data, output);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
            }
        }
        
        protected string GetFullPathFolder(string relationFolder)
        {
#if UNITY_IOS && !UNITY_EDITOR
            string persistentLevelsPath = Path.Combine(Application.persistentDataPath, relationFolder);
            string streamingLevelsPath = Path.Combine(Application.streamingAssetsPath, relationFolder);
            if (!Directory.Exists(persistentLevelsPath))
            {
                Directory.CreateDirectory(persistentLevelsPath);
                foreach (string filepath in Directory.GetFiles(streamingLevelsPath))
                {
                    File.Copy(filepath, Path.Combine(persistentLevelsPath, Path.GetFileName(filepath)));
                }
            }

            return Path.Combine(Application.persistentDataPath, relationFolder);
#else
            return Path.Combine(Application.streamingAssetsPath, relationFolder);
#endif
        }
    }
}