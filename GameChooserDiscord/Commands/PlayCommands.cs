using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using GameChooserDiscord.Services;

namespace GameChooserDiscord.Commands;

public class PlayCommands : ApplicationCommandModule
{
    private GameCardService games;
    private FruitService fruits;


    public PlayCommands(GameCardService service, FruitService service2)
    {
        games = service;
        fruits = service2;
    }

    [SlashCommand("test", "a testing command")]
    public async Task TestCommand(InteractionContext ctx)
    {
        var pair = games.GetRandomPair();
        var response = new DiscordInteractionResponseBuilder()
            .WithContent($"<{pair[0].WikipediaUrl}>\n{pair[0].Id.ToString()}\n<{pair[1].WikipediaUrl}>\n{pair[1].Id.ToString()}")
            .AddComponents(new DiscordComponent[]
            {
                new DiscordButtonComponent(ButtonStyle.Success, "heardof0", "I have heard of only game 1"),
                new DiscordButtonComponent(ButtonStyle.Success, "heardof1", "I have heard of only game 2")
            })
            .AddComponents(new DiscordComponent[]
            {
                new DiscordButtonComponent(ButtonStyle.Secondary, "heardofboth", "I have heard of both"),
                new DiscordButtonComponent(ButtonStyle.Secondary, "heardofnone", "I have heard of neither")
            });
        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
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