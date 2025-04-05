using CapyScript;
using Sirenix.OdinInspector;
using UnityEngine;

public class Wishlist : SingletonMonoBehaviour<Wishlist>
{
    [SerializeField][AssetsOnly] GameObject elementPrefab;

    protected override void Awake()
    {
        base.Awake();
        
        if (User.activeUser == null)
        {
            return;
        }

        foreach (Game game in User.activeUser.Wishlist)
        {
            Instantiate(elementPrefab, transform).GetComponent<StoreElement>().Setup(game);
        }
    }
}
