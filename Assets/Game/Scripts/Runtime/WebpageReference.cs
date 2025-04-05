using UnityEngine;
using CapyScript;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "New Webpage Reference", menuName = "Webpage Reference")]
public class WebpageReference : SingletonScriptableObject<WebpageReference>
{
    [Title("Settings")]
    public string WebsiteDomain;

    public static void Open(string path)
    {
        Uri URI = new Uri(new Uri(Instance.WebsiteDomain), path);
        Application.OpenURL(URI.ToString());
    }
}
