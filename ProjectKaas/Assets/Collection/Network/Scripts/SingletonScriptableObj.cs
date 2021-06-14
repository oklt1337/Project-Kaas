using UnityEngine;

namespace Collection.Network.Scripts
{
    public abstract class SingletonScriptableObj<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance = null;
        
        public static T Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                var results = Resources.FindObjectsOfTypeAll<T>();
                if (results.Length == 0)
                {
                    Debug.LogError("SingletonScriptableObj -> Instance -> result length is 0 for type " + typeof(T) + ".");
                    return null;
                }

                if (results.Length > 1)
                {
                    Debug.LogError("SingletonScriptableObj -> Instance -> result length is greater than 1 for type " + typeof(T) + ".");
                    return null;
                }

                _instance = results[0];
                return _instance;
            }
        }
    }
}
