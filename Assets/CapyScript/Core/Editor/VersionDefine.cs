using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

namespace CapyScript.Editor
{
    [CreateAssetMenu(fileName = "New VersionDefine", menuName = "CapyScript/Version Define")]
    public class VersionDefine : ScriptableObject
    {
        public class DefineModification
        {
            public string[] toAdd;
            public string[] toRemove;

            public DefineModification(string[] toAdd, string[] toRemove)
            {
                this.toAdd = toAdd;
                this.toRemove = toRemove;
            }

            public DefineModification()
            {
                this.toAdd = new string[0];
                this.toRemove = new string[0];
            }

            public DefineModification(DefineModification[] modifications)
            {
                List<string> toAdd = new List<string>();
                List<string> toRemove = new List<string>();

                for (int i = 0; i < modifications.Length; i++)
                {
                    toAdd.AddRange(modifications[i].toAdd);
                    toRemove.AddRange(modifications[i].toRemove);
                }

                this.toAdd = toAdd.Distinct().ToArray();
                this.toRemove = toRemove.Distinct().ToArray();
            }
        }
        
        static NamedBuildTarget[] targetGroups = { NamedBuildTarget.Android, NamedBuildTarget.EmbeddedLinux, NamedBuildTarget.NintendoSwitch, 
            NamedBuildTarget.iOS, NamedBuildTarget.Standalone, NamedBuildTarget.Server, NamedBuildTarget.XboxOne, NamedBuildTarget.WindowsStoreApps, NamedBuildTarget.WebGL,
            NamedBuildTarget.VisionOS, NamedBuildTarget.tvOS, NamedBuildTarget.QNX, NamedBuildTarget.PS4, NamedBuildTarget.LinuxHeadlessSimulation };

        public string define;
        public bool createVersions;
        public int majorVersion;
        public int minorVersion;

        [InitializeOnLoadMethod]
        public static void InitializeOnLoad() //A Unity az Editor betöltésekor és a Define Symbolok változásakor is meghívja
        {
            VersionDefine[] instances = GetAllInstances();

            DefineModification[] modifications = new DefineModification[instances.Length];

            for (int i = 0; i < instances.Length; i++)
            {
                modifications[i] = instances[i].Init();
            }

            DefineModification finalModification = new DefineModification(modifications);

            foreach (NamedBuildTarget grp in targetGroups)
            {
                PlayerSettings.GetScriptingDefineSymbols(grp, out string[] existingDefines);

                List<string> newDefineList = new List<string>();

                for (int i = 0; i < existingDefines.Length; i++)
                {
                    if (!existingDefines[i].StartsWith("CAPYSCRIPT") && !finalModification.toRemove.Contains(existingDefines[i]))
                    {
                        newDefineList.Add(existingDefines[i]);
                    }
                }

                for (int i = 0; i < finalModification.toAdd.Length; i++)
                {
                    newDefineList.Add(finalModification.toAdd[i]);
                }

                PlayerSettings.SetScriptingDefineSymbols(grp, newDefineList.ToArray());
            }
        }
        
        public static VersionDefine[] GetAllInstances()
        {
            string[] guids = AssetDatabase.FindAssets("t:VersionDefine");
            VersionDefine[] instances = new VersionDefine[guids.Length];
            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                instances[i] = AssetDatabase.LoadAssetAtPath<VersionDefine>(path);
            }

            return instances;
        }

        public DefineModification Init()
        {
            if (define.Trim() == "")
            {
                return new DefineModification();
            }
            
            string[] defines;

            string correctDefine = define.ToUpper().Replace(" ", "");

            if (createVersions)
            {
                defines = new string[] { correctDefine, (correctDefine + "_" + majorVersion), (correctDefine + "_" + majorVersion + "_" + minorVersion) };
            }
            else
            {
                defines = new string[] { correctDefine };
            }

            List<string> toRemove = new List<string>();

            foreach (NamedBuildTarget grp in targetGroups)
            {
                PlayerSettings.GetScriptingDefineSymbols(grp, out string[] existingDefines);

                for (int i = 0; i < existingDefines.Length; i++)
                {
                    if (existingDefines[i].StartsWith(correctDefine))
                    {
                        Regex major = new Regex("_[0-9]+_[0-9]+");
                        Regex minor = new Regex("_[0-9]+");

                        string ending = existingDefines[i].Replace(correctDefine, "").Trim();

                        if ((ending == "" || major.IsMatch(ending) || minor.IsMatch(ending)))
                        {
                            toRemove.Add(existingDefines[i]);
                        }
                    }
                }
            }

            return new DefineModification(defines, toRemove.ToArray());
        }
    }
}
