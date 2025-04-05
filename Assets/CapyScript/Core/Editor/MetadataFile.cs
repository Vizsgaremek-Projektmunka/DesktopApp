using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;


#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace CapyScript.Editor
{
#if ODIN_INSPECTOR
    [TypeInfoBox("You definitely shouldn't touch this file if you don't know what you're doing.")]
#endif
    public class MetadataFile : ScriptableObject
    {
        [HideInInspector] public GUID target;
        [ReadOnly] public string metadata;

        public string[] metas
        {
            get
            {
                return metadata.Split(";").ToArray();
            }
            set
            {
                if (value == null || value.Length == 0)
                {
                    metadata = string.Empty;
                }
                else
                {
                    metadata = value[0];

                    for (int i = 1; i < value.Length; i++)
                    {
                        metadata += ";" + value[i];
                    }
                }
            }
        }

#if ODIN_INSPECTOR
        [ShowInInspector][ReadOnly][PropertyOrder(-1)][LabelText("Target")]
#endif
        public string targetPath
        {
            get
            {
                return AssetDatabase.GUIDToAssetPath(target);
            }
            set
            {
                target = AssetDatabase.GUIDFromAssetPath(value);
            }
        }

        public bool HasMeta(string meta)
        {
            return metas.Contains(meta);
        }

        public void AddMeta(string meta)
        {
            if (!HasMeta(meta))
            {
                List<string> metaList = metas.ToList();
                metaList.Add(meta);
                metas = metaList.ToArray();
            }
        }

        public void RemoveMeta(string meta)
        {
            if (HasMeta(meta))
            {
                metas = metas.Where(l => l != meta).ToArray();
            }
        }

        public static void ClearEmptyFiles()
        {
            List<string> GUIDs = AssetDatabase.FindAssets("t:MetadataFile").ToList();

            foreach (string metafileGUID in GUIDs)
            {
                string path = AssetDatabase.GUIDToAssetPath(metafileGUID);
                MetadataFile metafile = (MetadataFile)AssetDatabase.LoadAssetAtPath(path, typeof(MetadataFile));
                if (metafile != null && metafile.metadata.Trim() == string.Empty)
                {
                    AssetDatabase.DeleteAsset(path);
                }
            }
        }

        public static List<MetadataFile> GetAllMetadataFiles()
        {
            return AssetDatabase.FindAssets("t:MetadataFile").ToList().ConvertAll(l => (MetadataFile)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(l), typeof(MetadataFile)));
        }

        public static List<MetadataFile> GetMetadataFilesTargeting(string target)
        {
            return GetAllMetadataFiles().Where(l => l.targetPath == target).ToList();
        }
    }
}
