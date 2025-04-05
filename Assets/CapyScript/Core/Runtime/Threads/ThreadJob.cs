using System;
using Unity.Jobs;

namespace CapyScript.Threads
{
    public abstract class ThreadJob
    {
        enum State
        {
            Unscheduled,
            Scheduled,
            Completed
        }

        protected Func<JobHandle> initializer;
        private JobHandle handle;
        private State state = State.Unscheduled;

        public bool isCompleted
        {
            get
            {
                return state == State.Completed;
            }
        }

        public void Schedule()
        {
            ThreadService.EnqueueJob(this);
            state = State.Unscheduled;
        }

        public void Run()
        {
            handle = initializer.Invoke();
            state = State.Scheduled;
        }

        public void UpdateState()
        {
            if (state == State.Scheduled && handle.IsCompleted)
            {
                state = State.Completed;
            }
        }

        public void Complete()
        {
            if (state == State.Completed)
            {
                handle.Complete();
            }
        }
    }
    
    public class ThreadIJob<T> : ThreadJob where T : struct, IJob
    {
        public ThreadIJob(T job)
        {
            initializer = new Func<JobHandle>(() => IJobExtensions.Schedule(job));
        }
    }

    public class ThreadIJobFor<T> : ThreadJob where T : struct, IJobFor
    {
        public ThreadIJobFor(T job, int length)
        {
            initializer = new Func<JobHandle>(() => IJobForExtensions.Schedule(job, length, default));
        }
    }

    public class ThreadIJobParallelFor<T> : ThreadJob where T : struct, IJobParallelFor
    {
        public ThreadIJobParallelFor(T job, int length, int batchCount)
        {
            initializer = new Func<JobHandle>(() => IJobParallelForExtensions.Schedule(job, length, batchCount));
        }
    }
}
