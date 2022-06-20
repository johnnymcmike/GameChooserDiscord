using GameChooserDiscord.Models;
using Microsoft.EntityFrameworkCore;

namespace GameChooserDiscord.Database;

public class GameChoiceContext : DbContext
{
    public GameChoiceContext(DbContextOptions<GameChoiceContext> options) : base(options)
    {
    }

    public GameChoiceContext(string connectionString) : base(new DbContextOptionsBuilder<GameChoiceContext>()
        .UseSqlite(connectionString).Options)
    {
    }

    public DbSet<GameCard> Games { get; set; }
}