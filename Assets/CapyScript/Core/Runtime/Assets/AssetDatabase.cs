using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CapyScript.Assets
{
    public static class AssetDatabase
    {
        static bool initialized;

        static GameObject[] prefabs;
        static Sprite[] sprites;
        static Material[] materials;
        static ScriptableObject[] scriptableObjects;
        static LoadableScriptableObject[] loadableScriptableObjects;

        static void Init()
        {
            prefabs = Resources.LoadAll<GameObject>("");
            sprites = Resources.LoadAll<Sprite>("");
            materials = Resources.LoadAll<Material>("");
            scriptableObjects = Resources.LoadAll<ScriptableObject>("");

            loadableScriptableObjects = Resources.LoadAll<LoadableScriptableObject>("");

            initialized = true;
        }

        public static void LoadScriptableObjects()
        {
            if (!initialized)
            {
                Init();
            }

            for (int i = 0; i < loadableScriptableObjects.Length; i++)
            {
                loadableScriptableObjects[i].Load();
            }
        }

        public static GameObject GetPrefab(string name)
        {
            string trim = name.Trim();

            if (trim.Length == 0)
            {
                return null;
            }

            if (!initialized)
            {
                Init();
            }

            GameObject[] matching = prefabs.Where(l => l.name == trim).ToArray();

            if (matching.Length > 0)
            {
                return matching[0];
            }
            else
            {
                return null;
            }
        }

        public static Sprite GetSprite(string name)
        {
            string trim = name.Trim();

            if (trim.Length == 0)
            {
                return null;
            }

            if (!initialized)
            {
                Init();
            }

            Sprite[] matching = sprites.Where(l => l.name == trim).ToArray();

            if (matching.Length > 0)
            {
                return matching[0];
            }
            else
            {
                return null;
            }
        }

        public static Material GetMaterial(string name)
        {
            string trim = name.Trim();

            if (trim.Length == 0)
            {
                return null;
            }

            if (!initialized)
            {
                Init();
            }

            Material[] matching = materials.Where(l => l.name == trim).ToArray();

            if (matching.Length > 0)
            {
                return matching[0];
            }
            else
            {
                return null;
            }
        }

        public static ScriptableObject GetScriptableObject(string name)
        {
            string trim = name.Trim();

            if (trim.Length == 0)
            {
                return null;
            }

            if (!initialized)
            {
                Init();
            }

            ScriptableObject[] matching = scriptableObjects.Where(l => l.name == trim).ToArray();

            if (matching.Length > 0)
            {
                return matching[0];
            }
            else
            {
                return null;
            }
        }
    }
}
