using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapyScript
{
    public class SingletonScriptableObject<T> : LoadableScriptableObject where T : SingletonScriptableObject<T>
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

        protected override void OnLoad()
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
                Debug.LogError("You cannot create more than 1 instance of " + GetType().ToString() + " because it's a SingletonScriptableObject!");
                return;
            }
        }
    }
}
