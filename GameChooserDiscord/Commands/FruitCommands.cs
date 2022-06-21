using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using GameChooserDiscord.Database;
using GameChooserDiscord.Models;
using GameChooserDiscord.Services;
using Microsoft.VisualBasic;

namespace GameChooserDiscord.Commands;

public class FruitCommands : ApplicationCommandModule
{
    private FruitService fruits;

    public FruitCommands(FruitService f)
    {
        fruits = f;
    }

    [SlashCommand("ratefruit", "be given two random fruits, from which to choose your favorite")]
    public async Task RateFruitCommand(InteractionContext ctx)
    {
        var pair = fruits.GetRandomPair();
        var response = new DiscordInteractionResponseBuilder()
            .WithContent($"Which of these fruits do you prefer?\nShowing fruits #{pair[0].Id} and #{pair[1].Id}:")
            .AddComponents(new DiscordComponent[]
            {
                new DiscordButtonComponent(ButtonStyle.Primary, "choseFruit0", $"{pair[0].Name}"),
                new DiscordButtonComponent(ButtonStyle.Success, "choseFruit1", $"{pair[1].Name}"),
            });
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
    }
}