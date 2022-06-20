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
        await discord.ConnectAsync();
        await Task.Delay(-1);
    }
}