using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Game : DatabaseReference<int>
{
    public static HashSet<Game> store { get; protected set; } = new HashSet<Game>();
    public static bool StoreRefreshPending { get; protected set; }
    
    public int ID
    {
        get
        {
            return key;
        }
    }
    
    public string BannerImageURL;
    public string PreviewImageURL;
    public string IconImageURL;
    public string Title;
    public string Description;
    public float Price;
    public Specification MinimumSpecification;
    public Specification RecommendedSpecification;
    public Company Publisher;
    public Company Developer;
    public int SteamID;
    public DateTime ReleaseDate;
    public int TotalDownloads;
    public int TotalWishlists;

    public Game(int id) : base(id)
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

        string result = await APIReference.GetData("games.php?id=" + key);
        var data = JSONTools.Convert(result);

        Title = data["title"];
        Description = data["description"];
        Price = float.Parse(data["price"]);
        MinimumSpecification = new Specification(int.Parse(data["min_spec"]));
        RecommendedSpecification = new Specification(int.Parse(data["rec_spec"]));
        Publisher = new Company(int.Parse(data["publisher"]));
        Developer = new Company(int.Parse(data["developer"]));
        //ReleaseDate = DateTime.Parse(data.Find(pair => pair.Key == "release_date").Value);

        await WaitForSubsToFinish();

        RefreshPending = false;
    }

    async Task WaitForSubsToFinish()
    {
        bool waiting = true;
        do
        {
            await Task.Delay(50);
            waiting = MinimumSpecification.RefreshPending || RecommendedSpecification.RefreshPending || Publisher.RefreshPending || Developer.RefreshPending;
        }
        while (waiting);
    }

    public async static void GenerateStore()
    {
        if (StoreRefreshPending)
        {
            return;
        }

        StoreRefreshPending = true;
        
        store = new HashSet<Game>();
        string result = await APIReference.GetData("games.php");

        foreach (var game in JSONTools.ConvertAll(result))
        {
            store.Add(new Game(Convert.ToInt32(game["id"])));
        }

        await WaitForStoreGames();

        StoreRefreshPending = false;
    }

    async static Task WaitForStoreGames()
    {
        bool waiting = true;
        do
        {
            await Task.Delay(50);
            waiting = store.Any(g => g.RefreshPending);
        }
        while (waiting);
    }
}
