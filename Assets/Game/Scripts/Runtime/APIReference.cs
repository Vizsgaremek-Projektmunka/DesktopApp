using CapyScript;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[CreateAssetMenu(fileName = "New API Reference", menuName = "API Reference")]
public class APIReference : SingletonScriptableObject<APIReference>
{
    [Title("Settings")]
    public string APIDomain;

    public static async Awaitable<string> GetData(string path)
    {
        Uri URI = new Uri(new Uri(Instance.APIDomain), path);

        using (UnityWebRequest request = UnityWebRequest.Get(URI))
        {
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                return request.error;
            }
            else
            {
                return request.downloadHandler.text;
            }
        }
    }
}
