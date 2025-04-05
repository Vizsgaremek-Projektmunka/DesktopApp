using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class LibraryElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI playtime;
    [SerializeField][ChildGameObjectsOnly] GameObject favourite;

    LibraryGame game;

    public void Setup(LibraryGame game)
    {
        if (game == null)
        {
            return;
        }

        this.game = game;
        title.text = game.Game.Title;
        description.text = game.Game.Description;
        favourite.SetActive(game.IsFavourite);

        TimeSpan playtimeSpan = new TimeSpan(0, game.Playtime, 0);

        playtime.text = "Playtime: ";
        if (playtimeSpan.TotalHours < 1f)
        {
            playtime.text += Mathf.RoundToInt((float)playtimeSpan.TotalMinutes) + " minutes";
        }
        else
        {
            playtime.text += Mathf.FloorToInt((float)playtimeSpan.TotalHours) + " hours";
        }
    }

    public void View()
    {
        if (game == null)
        {
            return;
        }

        PageManager.ToGameView(game.Game);
    }
}
