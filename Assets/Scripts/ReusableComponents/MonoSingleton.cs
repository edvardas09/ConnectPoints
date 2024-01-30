using UnityEngine;

namespace ConnectPoints
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; }

        protected virtual void Awake()
        {
            if (Instance == this)
            {
                Debug.LogError("MonoSingleton: trying to delete main singleton!");
            }
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this as T;

            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);
        }
    }

    public class Singleton<T> where T : class, new()
    {
        private static T instance;


        protected Singleton()
        {
        }

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();

                }
                return instance;
            }
        }

    }
}