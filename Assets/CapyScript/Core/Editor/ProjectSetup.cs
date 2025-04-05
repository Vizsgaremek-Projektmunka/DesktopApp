using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CapyScript.Editor
{
    public static class ProjectSetup
    {
        const string RootMeta = "RootFolder";

        [MenuItem("Assets/Setup Project", false, 9998)]
        private static void Setup()
        {
            string root = GetRoot();
            
            EnsureOnlyRoot(root);

            CreateFolder(root);
            CreateMetadataFile(root);
            CreateFolder(root + "/Scripts");
            CreateFolder(root + "/Scripts/Editor");
            CreateFolder(root + "/Scripts/Runtime");
            CreateFolder(root + "/Scenes");
            CreateFolder(root + "/Prefabs");
            CreateFolder(root + "/Graphics");
            CreateFolder(root + "/Localization");
            CreateFolder(root + "/Scriptable Objects");

            MetadataFile.ClearEmptyFiles();

            Debug.Log("Project setup complete");
        }

        [MenuItem("Assets/Mark folder as root", false, 9999)]
        private static void MarkRoot()
        {
            string root = AssetDatabase.GetAssetPath(Selection.activeObject);

            EnsureOnlyRoot(root);
            CreateMetadataFile(root);
            MetadataFile.ClearEmptyFiles();

            Debug.Log("Folder marked as root");
        }

        [MenuItem("Assets/Mark folder as root", true, 9999)]
        private static bool MarkRootValidate()
        {
            string root = AssetDatabase.GetAssetPath(Selection.activeObject);
            return root.StartsWith("Assets") && !root.Contains('.');
        }

        static void EnsureOnlyRoot(string root)
        {
            List<MetadataFile> otherMetafiles = MetadataFile.GetAllMetadataFiles().Where(l => l.targetPath != root).ToList();
            otherMetafiles.ForEach(l => l.RemoveMeta(RootMeta));
        }

        static string GetRoot()
        {
            List<MetadataFile> metaFiles = MetadataFile.GetAllMetadataFiles();

            string root = "Assets/Game";
            bool rootFound = false;

            for (int i = 0; i < metaFiles.Count && !rootFound; i++)
            {
                MetadataFile metadata = metaFiles[i];

                if (metadata.HasMeta(RootMeta))
                {
                    root = metadata.targetPath;
                    rootFound = true;
                }
            }

            return root;
        }

        static void CreateMetadataFile(string path)
        {
            List<MetadataFile> metafiles = MetadataFile.GetMetadataFilesTargeting(path);
            MetadataFile metafile = null;

            if (metafiles.Count > 0)
            {
                metafile = metafiles.Where(l => l.HasMeta(RootMeta)).First();

                if (metafile == null)
                {
                    metafile = metafiles[0];
                }
            }

            if (metafile == null)
            {
                MetadataFile newFile = new MetadataFile();
                newFile.metadata = RootMeta;
                newFile.targetPath = path;
                AssetDatabase.CreateAsset(newFile, path + "/metadata.asset");
            }
            else
            {
                metafile.AddMeta(RootMeta);
            }
        }

        static bool CreateFolder(string fullPath)
        {
            if (!fullPath.StartsWith("Assets"))
            {
                return false;
            }

            string[] split = fullPath.Split("/");

            if (split.Length >= 2)
            {
                string path = split[0];
                string name = split[split.Length - 1];

                for (int i = 1; i < split.Length - 1; i++)
                {
                    path += "/" + split[i];
                }

                if (!AssetDatabase.IsValidFolder(path) && !CreateFolder(path))
                {
                    return false;
                }

                if (!AssetDatabase.IsValidFolder(path + "/" + name))
                {
                    AssetDatabase.CreateFolder(path, name);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
