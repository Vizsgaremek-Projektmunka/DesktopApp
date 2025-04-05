using UnityEditor;
using UnityEngine;

namespace CapyScript
{
    public abstract class LoadableScriptableObject : ScriptableObject
    {
        private bool loaded = false;
        
        public void Load()
        {
            Application.quitting += Unload;

            #if UNITY_EDITOR
            EditorApplication.quitting += Unload;
            #endif

            if (!loaded)
            {
                OnLoad();
                loaded = true;
            }
        }

        private void Unload()
        {
            if (loaded)
            {
                loaded = false;
            }
        }

        protected abstract void OnLoad();
    }
}
