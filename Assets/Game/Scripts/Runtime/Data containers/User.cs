using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class User : DatabaseReference<string>
{
    public static User activeUser { get; private set; }

    public string Username
    {
        get
        {
            return key;
        }
    }

    public string Email;
    public string ProfileImageURL;
    public string ProfileDescription;
    public List<Game> Wishlist;
    public List<LibraryGame> Library;

    public User(string username) : base(username)
    {
        Get();
    }

    public override void Refresh()
    {
        Get();
    }

    async void Get()
    {
        RefreshPending = true;

        string result = await APIReference.GetData("user.php?username=" + key);
        var data = JSONTools.Convert(result);

        Email = data["email"];
        ProfileImageURL = data["image"];
        ProfileDescription = data["description"];

        string wishlistResult = await APIReference.GetData("wishlists.php?username=" + key);
        var wishlistData = JSONTools.ConvertAll(wishlistResult);
        Wishlist = new List<Game>();

        foreach (var game in wishlistData)
        {
            Wishlist.Add(new Game(Convert.ToInt32(game["id"])));
        }

        string libraryResult = await APIReference.GetData("libraries.php?username=" + key);
        var libraryData = JSONTools.ConvertAll(libraryResult);
        Library = new List<LibraryGame>();

        foreach (var game in libraryData)
        {
            Library.Add(new LibraryGame(game));
        }

        await WaitForSubsToFinish();

        RefreshPending = false;
    }

    async Task WaitForSubsToFinish()
    {
        bool waiting = true;
        do
        {
            await Task.Delay(50);
            waiting = Library.Any(g => g.RefreshPending) || Wishlist.Any(g => g.RefreshPending);
        }
        while (waiting);
    }

    public static void SetActiveUser(string username)
    {
        activeUser = new User(username);
    }
}
