using UnityEngine;
using CapyScript;
using Sirenix.OdinInspector;
using System.Linq;

public class Library : SingletonMonoBehaviour<Library>
{
    [SerializeField][AssetsOnly] GameObject elementPrefab;

    protected override void Awake()
    {
        base.Awake();

        if (User.activeUser == null)
        {
            return;
        }

        foreach (LibraryGame game in User.activeUser.Library.OrderByDescending(g => g.IsFavourite).ThenBy(g => g.Game.Title))
        {
            Instantiate(elementPrefab, transform).GetComponent<LibraryElement>().Setup(game);
        }
    }
}
