using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GameChooserDiscord.Database;

public class GameChoiceContextFactory : IDesignTimeDbContextFactory<GameChoiceContext>
{
    public GameChoiceContext CreateDbContext(string[] args)
    {
        return new GameChoiceContext("Data Source=Games.db");
    }
}