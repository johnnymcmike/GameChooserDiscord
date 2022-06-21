using System.Reflection;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.SlashCommands;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using GameChooserDiscord.Commands;
using GameChooserDiscord.Database;
using GameChooserDiscord.Models;
using GameChooserDiscord.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameChooserDiscord;

internal class Program
{
    private static void Main()
    {
        MainAsync().GetAwaiter().GetResult();
    }

    private static GameCardService games = new GameCardService();
    private static FruitService fruits = new FruitService();

    private static async Task MainAsync()
    {
        //initialize connection to discord
        var discord = new DiscordClient(new DiscordConfiguration
        {
            Token = await File.ReadAllTextAsync("token.txt"),
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged
        });
        discord.UseInteractivity(new InteractivityConfiguration
        {
            PollBehaviour = PollBehaviour.KeepEmojis,
            Timeout = TimeSpan.FromSeconds(30)
        });
        var rng = new Random();
        var http = new HttpClient();
        GameChoiceContext rawdb = new GameChoiceContext("Data Source=Games.db");
        http.DefaultRequestHeaders.Add("User-Agent", "Epic C# Discord Bot (mbjmcm@gmail.com)");
        var services = new ServiceCollection()
            .AddSingleton(rng)
            .AddSingleton(http)
            .AddSingleton(rawdb)
            .AddSingleton<FruitService>()
            .BuildServiceProvider();

        var slash = discord.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = services
        });
        slash.RegisterCommands<PlayCommands>();
        slash.RegisterCommands<FruitCommands>();

        discord.ComponentInteractionCreated += OnDiscordOnComponentInteractionCreated;
        await discord.ConnectAsync();
        await Task.Delay(-1);
    }

    private static async Task OnDiscordOnComponentInteractionCreated(DiscordClient s, ComponentInteractionCreateEventArgs e)
    {
        if (e.Id.StartsWith("choseFruit"))
        {
            await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
            var contentSplit = e.Message.Content.Split("#");
            var id1 = int.Parse(contentSplit[0].TakeWhile(Char.IsDigit).ToString());
            var id2 = int.Parse(contentSplit[1].TakeWhile(Char.IsDigit).ToString());
            var fruitpair = new Fruit[]
            {
                fruits.Get(id1),
                fruits.Get(id2)
            };

            switch (e.Id)
            {
                case "choseFruit0":
                    fruits.Chose(id1);
                    fruits.Rejected(id2);
                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder()
                        .WithContent(
                            $"{e.User.Username} likes **{fruitpair[0].Name}** more than **{fruitpair[1].Name}**!"));
                    break;
                case "choseFruit1":
                    fruits.Chose(id1);
                    fruits.Rejected(id2);
                    await e.Interaction.CreateFollowupMessageAsync(new DiscordFollowupMessageBuilder()
                        .WithContent(
                            $"{e.User.Username} likes **{fruitpair[1].Name}** more than **{fruitpair[0].Name}**!"));
                    break;
            }
            return;
        }
        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        var messagelines = e.Message.Content.Split("\n");
        var gamepair = new GameCard[]
        {
            games.Get(new Guid(messagelines[1])),
            games.Get(new Guid(messagelines[3]))
        };

        switch (e.Id)
        {
            case "heardof0":
                games.OnlyDrew(gamepair[0].Id);
                games.DidNotHearOf(gamepair[1].Id);
                break;
            case "heardof1":
                games.OnlyDrew(gamepair[1].Id);
                games.DidNotHearOf(gamepair[0].Id);
                break;
            case "heardofboth":
                games.OnlyDrew(gamepair[0].Id);
                games.OnlyDrew(gamepair[1].Id);
                break;
            case "heardofnone":
                games.DidNotHearOf(gamepair[0].Id);
                games.DidNotHearOf(gamepair[1].Id);
                break;
        }

        var followup = new DiscordFollowupMessageBuilder().WithContent("Vote recorded :)");
        followup.IsEphemeral = true;
        await e.Interaction.CreateFollowupMessageAsync(followup);
    }
}