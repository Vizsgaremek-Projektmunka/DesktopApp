using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace CapyScript.Editor
{
    public static class CreationUtility
    {
        public class GameObjectDescription
        {
            public string name;
            public Type[] components;
            public int layer;
            public string tag;
            public GameObjectDescription[] childs;

            public GameObjectDescription(string name, Type[] components, GameObjectDescription[] childs = null, int layer = 0, string tag = "")
            {
                this.name = name;

                if (components != null)
                {
                    this.components = components;
                }
                else
                {
                    this.components = new Type[0];
                }

                if (childs != null)
                {
                    this.childs = childs;
                }
                else
                {
                    this.childs = new GameObjectDescription[0];
                }

                this.layer = layer;
                this.tag = tag;
            }

            public GameObjectDescription(string name, params Type[] components)
            {
                this.name = name;

                if (components != null)
                {
                    this.components = components;
                }
                else
                {
                    this.components = new Type[0];
                }

                this.childs = new GameObjectDescription[0];
                this.layer = 0;
                this.tag = "";
            }
        }

        public static GameObject CreateGameObject(GameObjectDescription description)
        {
            Transform parent = Selection.activeTransform;

            GameObject newGameObject = Create(description);

            SceneView sceneView = SceneView.lastActiveSceneView;
            newGameObject.transform.position = sceneView ? sceneView.pivot : Vector3.zero;

            StageUtility.PlaceGameObjectInCurrentStage(newGameObject);

            if (parent != null)
            {
                newGameObject.transform.parent = parent;
            }

            GameObjectUtility.EnsureUniqueNameForSibling(newGameObject);

            Undo.RegisterCreatedObjectUndo(newGameObject, "Create Object: " + newGameObject.name);
            Selection.activeGameObject = newGameObject;

            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

            return newGameObject;
        }

        static GameObject Create(GameObjectDescription description)
        {
            GameObject newGameObject = ObjectFactory.CreateGameObject(description.name, description.components);
            GameObjectUtility.EnsureUniqueNameForSibling(newGameObject);
            newGameObject.layer = description.layer;

            if (description.tag != "")
            {
                newGameObject.tag = description.tag;
            }

            for (int i = 0; i < description.childs.Length; i++)
            {
                GameObject child = Create(description.childs[i]);
                child.transform.parent = newGameObject.transform;
            }

            return newGameObject;
        }
    }
}
