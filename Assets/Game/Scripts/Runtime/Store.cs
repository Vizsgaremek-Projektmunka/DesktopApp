using CapyScript;
using Sirenix.OdinInspector;
using UnityEngine;

public class Store : SingletonMonoBehaviour<Store>
{
    [SerializeField][AssetsOnly] GameObject elementPrefab;

    protected override void Awake()
    {
        base.Awake();
        
        if (Game.store == null || Game.store.Count == 0)
        {
            return;
        }

        foreach (Game game in Game.store)
        {
            Instantiate(elementPrefab, transform).GetComponent<StoreElement>().Setup(game);
        }
    }
}
