using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapyScript
{
    public class Singleton<T> where T : Singleton<T>
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

        public Singleton()
        {
            if (instance == null || instance == (T)this)
            {
                if (this is not T)
                {
                    Debug.LogError(GetType().ToString() + " is defined incorrectly!");
                    return;
                }
                else
                {
                    instance = (T)this;
                }
            }
            else
            {
                Debug.LogError("You cannot create more than 1 instance of " + GetType().ToString() + " because it's a Singleton!");
                return;
            }
        }

        ~Singleton()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }
}
