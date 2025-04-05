using CapyScript.Threads;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CapyScript
{
    [AddComponentMenu("CapyScript/Services/Coroutine Service")]
    public class CoroutineService : SingletonMonoBehaviour<CoroutineService>
    {
        public static void Init()
        {
            if (!Exists)
            {
                GameObject serviceObject = new GameObject("Coroutine Service");
                serviceObject.AddComponent<CoroutineService>();
            }
        }

        public static Coroutine Start(IEnumerator routine)
        {
            Init();
            
            return Instance.StartCoroutine(routine);
        }

        public static void Stop(Coroutine coroutine)
        {
            Init();

            Instance.StopCoroutine(coroutine);
        }
    }
}
