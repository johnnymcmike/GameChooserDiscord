using System.ComponentModel.DataAnnotations;

namespace GameChooserDiscord.Models;

public class GameCard
{
    public GameCard()
    {
        TimesChosen = 0;
        TimesDrawn = 0;
        TimesUnheardOf = 0;
        Id = Guid.NewGuid();
    }

    [Key] public Guid Id { get; set; }
    public int TimesDrawn { get; set; }
    public int TimesChosen { get; set; }

    public int? TimesUnheardOf { get; set; }

    public string WikipediaUrl { get; set; }

    public double PercentageChosen => (double) TimesChosen / TimesDrawn;
}