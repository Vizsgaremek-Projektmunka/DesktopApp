using UnityEngine;
using CapyScript;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Collections.Generic;

public class PageManager : SingletonMonoBehaviour<PageManager>
{
    [Title("Views")]
    [SerializeField][Required] GameObject storeView;
    [SerializeField][Required] GameObject libraryView;
    [SerializeField][Required] GameObject wishlistView;
    [SerializeField][Required] GameObject profileView;
    [SerializeField][Required] GameObject gameView;

    [Title("Layouts")]
    [SerializeField] List<RectTransform> layouts;

    List<GameObject> viewList;

    private void Start()
    {
        CreateViewList();
        ToStoreView();
    }

    static void CreateViewList()
    {
        Instance.viewList = new List<GameObject>
        {
            Instance.storeView,
            Instance.libraryView,
            Instance.wishlistView,
            Instance.profileView,
            Instance.gameView
        };
    }

    public static void Refresh()
    {
        for (int i = 0; i < Instance.layouts.Count; i++)
        {
            if (Instance.layouts[i] != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(Instance.layouts[i]);
            }
        }
    }

    static void DisableAllViews()
    {
        foreach (GameObject view in Instance.viewList)
        {
            view.SetActive(false);
        }
    }

    public static void ToStoreView()
    {
        DisableAllViews();
        Instance.storeView.SetActive(true);

        Refresh();
    }

    public static void ToLibraryView()
    {
        DisableAllViews();
        Instance.libraryView.SetActive(true);

        Refresh();
    }

    public static void ToWishlistView()
    {
        DisableAllViews();
        Instance.wishlistView.SetActive(true);

        Refresh();
    }

    public static void ToProfileView()
    {
        DisableAllViews();
        Instance.profileView.SetActive(true);

        Refresh();
    }

    public static void ToGameView(Game game)
    {
        DisableAllViews();
        GameView.Instance.SetGame(game);
        Instance.gameView.SetActive(true);

        Refresh();
    }
}
