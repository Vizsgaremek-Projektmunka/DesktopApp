using UnityEngine;

namespace CapyScript
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        public static bool Exists
        {
            get
            {
                return instance != null;
            }
        }

        public static bool ExistsInEditMode
        {
            get
            {
                return FindObjectsOfType<T>().Length > 0;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null || instance == (T)this)
            {
                if (this is not T)
                {
                    Debug.LogError(GetType().ToString() + " is defined incorrectly!");
                    Destroy(this);
                    return;
                }
                else
                {
                    instance = (T)this;
                }
            }
            else
            {
                Debug.LogError("You cannot create more than 1 instance of " + GetType().ToString() + " because it's a SingletonMonoBehaviour!");
                Destroy(this);
                return;
            }
        }
    }
}
