using Discord;
using Discord.Webhook;
using Discord.WebSocket;
using Discord.Commands;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using ImageMagick;
using HostCord.Utils;
using HostCord.Services;

namespace KipoBot.Modules
{
    [Name("info")]
    [Summary("Contains all needed commands, get information about servers, users and kipo.")]
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService _service;
        private readonly DiscordSocketClient _client;
        private readonly CommandHandlingService _commandHandler;

        public InfoModule(CommandService service, DiscordSocketClient client, CommandHandlingService commandHandler)
        {
            _service = service;
            _client = client;
            _commandHandler = commandHandler;
        }

        [Command("help", RunMode = RunMode.Async)]
        public async Task Help(string arg = null)
        {
            EmbedBuilder embedBuilder = new EmbedBuilder();

            embedBuilder.Color = Color.Purple;
            embedBuilder.WithAuthor(author =>
            {
                author.WithName(_client.CurrentUser.Username);
                author.WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl());
            });

            if (arg != null)
            {
                foreach (var module in _service.Modules)
                {
                    if (arg == module.Name)
                    {
                        foreach (var command in module.Commands)
                            if (command.Name != "help")
                                embedBuilder.AddField(_commandHandler.GetPrefix() + (module.Group == null ? string.Empty : (module.Group + " ")) + command.Name, command.Summary == null ? "No Summary" : command.Summary, false);

                        await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
                        return;
                    }
                }
            }

            //if module not found, list modules avaiable
            foreach (var module in _service.Modules)
                    embedBuilder.AddField(char.ToUpper(module.Name[0]) + module.Name.Substring(1) + " Module | " + _commandHandler.GetPrefix() + "help " + module.Name, module.Summary, false);

            await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("latency", RunMode = RunMode.Async)]
        [Summary("Shows kipo's response time")]
        public async Task Latency() => await ReplyAsync("My response time is " + Context.Client.Latency + " ms.");

        [Command("kipoinfo", RunMode = RunMode.Async)]
        [Summary("Shows kipo's statistics")]
        public async Task Stats()
        {
            PerformanceMonitor performanceMonitor = PerformanceMonitor.getInstance();

            int users = 0;
            foreach (SocketGuild guild in Context.Client.Guilds)
                users += guild.Users.Count;

            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.Color = Color.Purple;

            embedBuilder.WithAuthor(author =>
            {
                author.WithName("Kipo - Cutest bot in the universe!");
                author.WithIconUrl(Context.Client.CurrentUser.GetAvatarUrl());
            });

            embedBuilder.AddField("Guilds", Context.Client.Guilds.Count, true);
            embedBuilder.AddField("Users", users, true);
            embedBuilder.AddField("Prefix", _commandHandler.GetPrefix(), true);
            embedBuilder.AddField("Created", Context.Client.CurrentUser.CreatedAt.UtcDateTime, true);
            embedBuilder.AddField("ID", Context.Client.CurrentUser.Id, true);
            embedBuilder.AddField("RAM Usage", performanceMonitor.ramUsage + " MB", true);
            embedBuilder.AddField("CPU Usage", performanceMonitor.cpuUsage + "%", true);

            await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("userinfo", RunMode = RunMode.Async)]
        [Summary("Shows user's information \n +userinfo [user]")]
        public async Task Info([Remainder] string command)
        {
            SocketGuildUser user = Helpers.extractUser(Context, command);

            string roles = "| ";
            foreach (IRole role in user.Roles)
                if (role.Name != "@everyone")
                    roles += role.Mention + " | ";

            string permissions = "| ";
            foreach (GuildPermission guildPermission in user.GuildPermissions.ToList())
                permissions += guildPermission.ToString() + " | ";

            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.Color = Color.Purple;

            embedBuilder.WithAuthor(author =>
            {
                author.WithName(user.Username + "#" + user.Discriminator);
                author.WithIconUrl(user.GetAvatarUrl());
            });

            embedBuilder.AddField("Joined", user.JoinedAt.Value.UtcDateTime, true);
            embedBuilder.AddField("Registered", user.CreatedAt.UtcDateTime, true);

            embedBuilder.AddField("Status", user.Status, false);
            embedBuilder.AddField("Roles", roles, false);
            embedBuilder.AddField("Permissions", permissions, false);
            embedBuilder.AddField("Avatar", user.GetAvatarUrl(), false);

            await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("serverinfo", RunMode = RunMode.Async)]
        [Summary("Shows information about current guild the command is executed in")]
        public async Task ServerInfo()
        {
            EmbedBuilder embedBuilder = new EmbedBuilder();
            embedBuilder.Color = Color.Purple;

            embedBuilder.WithAuthor(author =>
            {
                author.WithName(Context.Guild.Name);
                author.WithIconUrl(Context.Guild.IconUrl);
            });

            embedBuilder.AddField("Created", Context.Guild.CreatedAt.UtcDateTime, true);
            embedBuilder.AddField("Channel categories", Context.Guild.CategoryChannels.Count, true);
            embedBuilder.AddField("Text channels", Context.Guild.TextChannels.Count, true);
            embedBuilder.AddField("Voice channels", Context.Guild.VoiceChannels.Count, true);
            embedBuilder.AddField("Emotes", Context.Guild.Emotes.Count, true);
            embedBuilder.AddField("ID", Context.Guild.Id, true);
            embedBuilder.AddField("Members", Context.Guild.MemberCount, true);
            embedBuilder.AddField("Owner", Context.Guild.Owner.Username + "#" + Context.Guild.Owner.DiscriminatorValue, true);
            embedBuilder.AddField("Owner's ID", Context.Guild.Owner.Id, true);
            embedBuilder.AddField("Roles", Context.Guild.Roles.Count, true);
            embedBuilder.AddField("Boost tier", Context.Guild.PremiumTier, true);
            embedBuilder.AddField("Boosts", Context.Guild.PremiumSubscriptionCount, true);
            embedBuilder.AddField("Icon", Context.Guild.IconUrl, true);

            await Context.Channel.SendMessageAsync(embed: embedBuilder.Build());
        }

        [Command("avatar", RunMode = RunMode.Async)]
        [Summary("Sends link to user's avatar")]
        public async Task Avatar([Remainder] string command)
        {
            SocketGuildUser user = Helpers.extractUser(Context, command);
            await Context.Channel.SendMessageAsync(user.GetAvatarUrl(size: 1024));
        }


        /*
        [Command("reloadbanners", RunMode = RunMode.Async)]
        public async Task RefreshBanners()
        {
            ImageMaker.reloadBanners("banners/");
        }


        // usage +meme top text;bottom text
        [Command("meme", RunMode = RunMode.Async)]
        [Summary("Creates memes from attached image and two phrases separated by ;")]
        public async Task memeFrom([Remainder] String text)
        {
            try
            {
                HttpClient client = new HttpClient();
                var attachments = Context.Message.Attachments;
                String imageURL = Context.Message.Attachments.ElementAt(0).Url;
                String memeText = text;
                var buffer = await client.GetByteArrayAsync(imageURL);
                MagickImage memeImg = new MagickImage(buffer);
                String[] memeTextSplit = memeText.Split(';');

                var composedMeme = ImageMaker.composeMeme(memeImg, memeTextSplit[0], memeTextSplit[1]);
                await Context.Channel.SendFileAsync(composedMeme, "image.png", Context.User.Mention);
                await Context.Message.DeleteAsync();

            }
            catch (Exception e)
            {
                await Context.Channel.SendMessageAsync($"Invalid format.");
            }
        }
        */
    }
}