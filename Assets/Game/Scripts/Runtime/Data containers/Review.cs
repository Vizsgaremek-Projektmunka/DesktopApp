using UnityEngine;

public class Review : DatabaseReference<string, int>
{
    public User User { get; private set; }
    public Game Game { get; private set; }

    public bool IsPositive;
    public string Content;
    
    public Review(string key1, int key2) : base(key1, key2)
    {
        User = new User(key1);
        Game = new Game(key2);
    }

    public override void Refresh()
    {
        
    }
}
