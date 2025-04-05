using System;
using System.Collections.Generic;
using UnityEngine;

namespace CapyScript.Threads
{
    [AddComponentMenu("CapyScript/Services/Thread Service")]
    public class ThreadService : SingletonMonoBehaviour<ThreadService>
    {
        private static Queue<ThreadJob> scheduledJobs = new Queue<ThreadJob>();
        private static List<ThreadJob> currentJobs = new List<ThreadJob>();

        public static void Init()
        {
            if (!Exists)
            {
                GameObject serviceObject = new GameObject("Thread Service");
                serviceObject.AddComponent<ThreadService>();
            }
        }

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Update()
        {
            lock (scheduledJobs)
            {
                while (scheduledJobs.Count > 0)
                {
                    ThreadJob job = scheduledJobs.Dequeue();
                    job.Run();
                    currentJobs.Add(job);
                }
            }

            List<ThreadJob> remainingJobs = new List<ThreadJob>();

            for (int i = 0; i < currentJobs.Count; i++)
            {
                currentJobs[i].UpdateState();

                if (!currentJobs[i].isCompleted)
                {
                    remainingJobs.Add(currentJobs[i]);
                }
                else
                {
                    currentJobs[i].Complete();
                }
            }

            currentJobs = remainingJobs;
        }

        public static bool EnqueueJob(ThreadJob job)
        {
            try
            {
                scheduledJobs.Enqueue(job);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
