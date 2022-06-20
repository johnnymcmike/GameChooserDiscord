using GameChooserDiscord.Database;
using GameChooserDiscord.Models;

namespace GameChooserDiscord.Services;

public class GameCardService
{
    private GameChoiceContext db;
    private Random rng;
    public GameCardService()
    {
        db = new GameChoiceContext("Data Source=Games.db");
        rng = new Random();
    }

    public GameCard[] GetRandomPair()
    {
        var workingArray = db.Games.ToArray();
        int n = workingArray.Length;
        if (n < 2)
            throw new InvalidOperationException("Set has fewer than 2 items from which to draw.");
        if (n == 2)
            return workingArray;
        for (int i = 0; i < (n - 1); i++)
        {
            int r = i + rng.Next(n - i);
            //not sure if swapping via deconstruction has performance implications vs the old fashioned way of creating
            //a temporary variable. file an issue if you know for a fact that it does
            (workingArray[r], workingArray[i]) = (workingArray[i], workingArray[r]);
        }
        return new[] {workingArray[0], workingArray[1]};
    }

    public void OnlyDrew(Guid gamecardId)
    {
        var game = db.Games.Find(gamecardId);
        if(game is null)
            return;
        
        game.TimesDrawn++;
        db.SaveChanges();
    }

    public void DidNotHearOf(Guid gamecardId)
    {
        var game = db.Games.Find(gamecardId);
        if(game is null)
            return;
        
        game.TimesDrawn++;
        game.TimesUnheardOf++;
        db.SaveChanges();
    }

    public void Chose(Guid gamecardId)
    {
        var game = db.Games.Find(gamecardId);
        if(game is null)
            return;

        game.TimesDrawn++;
        game.TimesChosen++;
    }
}