using TMPro;
using UnityEngine;

public class StoreElement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI price;

    Game game;

    public void Setup(Game game)
    {
        if (game == null)
        {
            return;
        }

        this.game = game;
        title.text = game.Title;
        description.text = game.Description;
        price.text = game.Price + "€";
    }

    public void View()
    {
        if (game == null)
        {
            return;
        }

        PageManager.ToGameView(game);
    }
}
