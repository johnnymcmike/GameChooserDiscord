namespace GameChooserDiscord.Models;

public class Fruit
{
    public int Id { get; set; }

    public string Name
    {
        get; set;
    }
    
    public int TimesChosen { get; set; }
    public int TimesDrawn { get; set; }
}