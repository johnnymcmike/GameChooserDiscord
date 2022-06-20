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
        http.DefaultRequestHeaders.Add("User-Agent", "Epic C# Discord Bot (mbjmcm@gmail.com)");
        var services = new ServiceCollection()
            .AddSingleton(rng)
            .AddSingleton(http)
            .AddSingleton<GameCardService>()
            .BuildServiceProvider();

        var slash = discord.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = services
        });
        slash.RegisterCommands<PlayCommands>();

        discord.ComponentInteractionCreated += OnDiscordOnComponentInteractionCreated;
        await discord.ConnectAsync();
        await Task.Delay(-1);
    }

    private static async Task OnDiscordOnComponentInteractionCreated(DiscordClient s, ComponentInteractionCreateEventArgs e)
    {
        await e.Interaction.CreateResponseAsync(InteractionResponseType.DeferredMessageUpdate);
        var eb = e.Message.Content.Split("\n");
        var pair = new GameCard[]
        {
            games.Get(new Guid(eb[1])),
            games.Get(new Guid(eb[3]))
        };

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

        var followup = new DiscordFollowupMessageBuilder().WithContent("Vote recorded :)");
        followup.IsEphemeral = true;
        await e.Interaction.CreateFollowupMessageAsync(followup);
    }
}