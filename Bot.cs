using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;
using HostCord.Services;
using HostCord.ViewModels;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using HostCord.BotModules;
using System.Security;
using System.Runtime.InteropServices;

namespace HostCord
{
    public class Bot
    {
        public DiscordSocketClient client;
        public ServiceProvider services;
        public CommandHandlingService commandHandler;

        public string token;
        public string prefix;

        public Bot()
        {
            services = ConfigureServices();
            client   = services.GetRequiredService<DiscordSocketClient>();
            commandHandler = services.GetRequiredService<CommandHandlingService>();
        }

        public async Task MainAsync()
        {
            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            await commandHandler.InitializeAsync();

            await Task.Delay(-1);
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Debug
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig
                {
                    LogLevel = LogSeverity.Debug
                }))
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }

        public IEnumerable<ModuleInfo> GetModules()
        {
            var service = services.GetRequiredService<CommandService>();
            service.AddModulesAsync(Assembly.GetEntryAssembly(), null);
            var modules = service.Modules;
            return modules;
        }
    }
}
