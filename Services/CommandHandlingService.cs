
using Discord;
using Discord.Commands;
using Discord.Commands.Builders;
using Discord.WebSocket;
using HostCord.BotModules;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;

namespace HostCord.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        private int prefixPos = 0;
        private string prefix;

        public CommandHandlingService(IServiceProvider services, DiscordSocketClient client)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = client;
            _services = services;

            _commands.CommandExecuted += CommandExecutedAsync;
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
            => await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        public void SetPrefix(string prefix)
            => this.prefix = prefix;

        public string GetPrefix()
            => prefix;

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;
            if (!message.HasCharPrefix(char.Parse(prefix), ref prefixPos)) return;

            var context = new SocketCommandContext(_discord, message);

            await _commands.ExecuteAsync(context, prefixPos, _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            if (context.Message.Author.IsBot)
                return;

            if (!command.IsSpecified)
                return;

            if (result.IsSuccess)
                return;
        }
    }
}
