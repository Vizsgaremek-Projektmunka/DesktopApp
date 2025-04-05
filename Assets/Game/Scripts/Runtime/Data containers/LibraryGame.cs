using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class LibraryGame : DatabaseReference<string, int>
{
    public User User { get; private set; }
    public Game Game { get; private set; }

    public int Playtime;
    public bool IsFavourite;
    
    public LibraryGame(string key1, int key2) : base(key1, key2)
    {
        User = new User(key1);
        Game = new Game(key2);

        Get();
    }

    public LibraryGame(Dictionary<string, string> data) : base(data["username"], Convert.ToInt32(data["game_id"]))
    {
        User = new User(data["username"]);
        Game = new Game(Convert.ToInt32(data["game_id"]));

        ApplyData(data);
    }

    public override void Refresh()
    {
        Get();
    }

    async void Get()
    {
        RefreshPending = true;

        string result = await APIReference.GetData("libraries.php?username=" + User.Username + "&game_id=" + Game.ID);
        var data = JSONTools.Convert(result);

        ApplyData(data);

        await WaitForSubsToFinish();

        RefreshPending = false;
    }

    void ApplyData(Dictionary<string, string> data)
    {
        Playtime = Convert.ToInt32(data["playtime"]);
        IsFavourite = Convert.ToBoolean(Convert.ToInt32(data["is_favourite"]));
    }

    async Task WaitForSubsToFinish()
    {
        bool waiting = true;
        do
        {
            await Task.Delay(50);
            waiting = Game.RefreshPending;
        }
        while (waiting);
    }
}
