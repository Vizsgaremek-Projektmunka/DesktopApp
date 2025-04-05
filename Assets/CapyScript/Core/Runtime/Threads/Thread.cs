namespace CapyScript.Threads
{
    public abstract class Thread
    {
        public enum ThreadState
        {
            Stopped,
            Running
        }

        System.Threading.Thread thread;

        public ThreadState state { get; private set; } = ThreadState.Stopped;

        public void Start()
        {
            ThreadService.Init();

            if (thread == null)
            {
                thread = new System.Threading.Thread(Main);
            }

            thread.Start();
            state = ThreadState.Running;
        }

        public void Stop()
        {
            thread.Abort();
            state = ThreadState.Stopped;
        }

        protected abstract void Main();
    }
}
