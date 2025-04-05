using UnityEngine;

public abstract class DatabaseReference
{
    public abstract void Refresh();

    public bool RefreshPending { get; protected set; }
}

public abstract class DatabaseReference<T> : DatabaseReference
{
    protected T key { get; private set; }

    public DatabaseReference(T key)
    {
        this.key = key;
    }
}

public abstract class DatabaseReference<T1, T2> : DatabaseReference
{
    protected T1 key1 { get; private set; }
    protected T2 key2 { get; private set; }

    public DatabaseReference(T1 key1, T2 key2)
    {
        this.key1 = key1;
        this.key2 = key2;
    }
}
