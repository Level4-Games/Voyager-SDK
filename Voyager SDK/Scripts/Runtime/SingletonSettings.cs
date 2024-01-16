using UnityEngine;

namespace Voyager_SDK
{
    public abstract class SingletonSettings<T> : ScriptableObject where T : SingletonSettings<T>
    {
        private static volatile T _instance;

        private static object LockObj;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (LockObj ??= new object())
                {
                    if (_instance != null)
                        return _instance;

                    _instance = VoyagerResources.LoadFromResources<T>();
                }

                return _instance;
            }
        }
    }
}