using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.SlashCommands;
using GameChooserDiscord.Services;

namespace GameChooserDiscord.Commands;

public class PlayCommands : ApplicationCommandModule
{
    private GameCardService games;

    public PlayCommands(GameCardService service)
    {
        games = service;
    }

    [SlashCommand("test", "a testing command")]
    public async Task TestCommand(InteractionContext ctx)
    {
        var pair = games.GetRandomPair();
        var response = new DiscordInteractionResponseBuilder()
            .WithContent($"<{pair[0].WikipediaUrl}>\n<{pair[1].WikipediaUrl}>")
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

        ctx.Client.ComponentInteractionCreated += async (s, e) =>
        {
            switch (e.Id)
            {
                case "heardof0":
                    games.OnlyDrew(pair[0].Id);
                    games.DidNotHearOf(pair[1].Id);
                    break;
                case "heardof1":
                    games.OnlyDrew(pair[1].Id);
                    games.DidNotHearOf(pair[0].Id);
                    break;
                case "heardofboth":
                    games.OnlyDrew(pair[0].Id);
                    games.OnlyDrew(pair[1].Id);
                    break;
                case "heardofnone":
                    games.DidNotHearOf(pair[0].Id);
                    games.DidNotHearOf(pair[1].Id);
                    break;
            }

            await e.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage,
                new DiscordInteractionResponseBuilder().WithContent($"{e.Message.Content}\nRegistered."));
        };


        await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, response);
    }
}