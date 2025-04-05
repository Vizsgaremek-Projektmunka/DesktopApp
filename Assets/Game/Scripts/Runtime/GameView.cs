using System.Linq;
using CapyScript;
using UnityEngine;
using TMPro;

public class GameView : SingletonMonoBehaviour<GameView>
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI price;
    [SerializeField] TextMeshProUGUI developer;
    [SerializeField] TextMeshProUGUI publisher;
    [SerializeField] GameObject buy;
    [SerializeField] GameObject owned;
    [SerializeField] SpecificationView minSpec;
    [SerializeField] SpecificationView recSpec;
    int gameID;

    public void SetGame(Game game)
    {
        gameID = game.ID;
        title.text = game.Title;
        description.text = game.Description;
        price.text = game.Price + "€";
        developer.text = game.Developer.Name;
        publisher.text = game.Publisher.Name;
        minSpec.Set(game.MinimumSpecification);
        recSpec.Set(game.RecommendedSpecification);

        if (User.activeUser.Library.Count(g => g.Game.ID == game.ID) > 0)
        {
            owned.SetActive(true);
            buy.SetActive(false);
        }
        else
        {
            owned.SetActive(false);
            buy.SetActive(true);
        }
    }

    public void Redirect()
    {
        WebpageReference.Open("game_details.php?game_id=" + gameID);
    }
}
