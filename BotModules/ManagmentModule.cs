using Discord;
using System.Collections.Generic;
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
using System.Threading;
using System.Linq;
using HostCord.Utils;

namespace HostCord.BotModules
{
    [Name("managment")]
    [Summary("Contains all needed commands to manage a server.")]
    public class ManagmentModule : ModuleBase<SocketCommandContext>
    {
        [Command("ban", RunMode = RunMode.Async)]
        [Summary("Bans specified user\n+ban [user]")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have required permission to ban people!")]
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "I don't have required permission to ban people!")]
        public async Task Ban([Remainder] string command)
        {
            SocketGuildUser user = Helpers.extractUser(Context, command);

            if (user == null)
                await ReplyAsync("User not found!\n+ban [user]");
            else
                await user.BanAsync();

            await ReplyAsync(user.Username + "#" + user.DiscriminatorValue + " has been banned! ID: " + user.Id);
        }

        [Command("unban", RunMode = RunMode.Async)]
        [Summary("Removes banned specified user\n+unban [user]")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "You don't have required permission to unban people!")]
        [RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "I don't have required permission to unban people!")]
        public async Task UnBan([Remainder] string command)
        {
            SocketGuildUser user = Helpers.extractUser(Context, command);

            if (user == null)
                await ReplyAsync("User not found!\n+unban [user]");
            else
                await Context.Guild.RemoveBanAsync(user);

            await ReplyAsync(user.Username + "#" + user.DiscriminatorValue + " has been unbanned! ID: " + user.Id);
        }

        [Command("kick", RunMode = RunMode.Async)]
        [Summary("Kicks specified user\n+kick [user]")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.KickMembers, ErrorMessage = "You don't have required permission to kick people!")]
        [RequireBotPermission(GuildPermission.KickMembers, ErrorMessage = "I don't have required permission to kick people!")]
        public async Task Kick([Remainder] string command)
        {
            SocketGuildUser user = Helpers.extractUser(Context, command);

            if (user == null)
                await ReplyAsync("User not found!\n+kick [user]");
            else
                await user.KickAsync();

            await ReplyAsync(user.Username + "#" + user.DiscriminatorValue + " has been kicked!");
        }

        [Command("purge", RunMode = RunMode.Async)]
        [Summary("Deletes specified amount of messages\n+purge [amount]")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageMessages, ErrorMessage = "You don't have required permission to delete messages!")]
        [RequireBotPermission(ChannelPermission.ManageMessages, ErrorMessage = "I don't have required permission to delete messages!")]
        public async Task Purge(string i = null)
        {
            IEnumerable<IMessage> messages;


            if (Int32.TryParse(i, out _))
            {
                messages = await Context.Channel.GetMessagesAsync(Int32.Parse(i) + 1).FlattenAsync();

            }
            else
            {
                await Context.Channel.SendMessageAsync("Amount must be a number!\n+purge [amount]");
                return;
            }

            await (Context.Channel as SocketTextChannel).DeleteMessagesAsync(messages.Where(x => (DateTimeOffset.UtcNow - x.Timestamp).TotalDays <= 14));
        }

        [Command("say", RunMode = RunMode.Async)]
        [Summary("Says something as Kipo.\n+say [message]")]
        [RequireUserPermission(GuildPermission.ManageMessages, ErrorMessage = "Lack of required permission: Manage Messages")]
        public async Task Say([Remainder] string text)
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(text);
        }

        [Command("showemotes", RunMode = RunMode.Async)]
        [Summary("Lists all of a guild emotes.\n+emotes")]
        [RequireContext(ContextType.Guild)]
        [RequireUserPermission(GuildPermission.ManageEmojisAndStickers, ErrorMessage = "Lack of required permission: Manage Emojis")]
        public async Task ShowEmotes()
        {
            List<GuildEmote> animated = new List<GuildEmote>(Context.Guild.Emotes.Where(x => x.Animated));
            List<GuildEmote> standard = new List<GuildEmote>(Context.Guild.Emotes.Where(x => !x.Animated));

            string emotes = "";

            await Context.Channel.SendMessageAsync("Standard emotes | " + standard.Count);

            for (int i = 1; i <= standard.Count; i++)
            {
                if (i % 9 == 0)
                {
                    emotes += "<:" + standard[i - 1].Name + ":" + standard[i - 1].Id + ">";
                    await Context.Channel.SendMessageAsync(emotes);
                    emotes = "";
                }
                else
                {
                    emotes += "<:" + standard[i - 1].Name + ":" + standard[i - 1].Id + ">";
                }
            }

            if (emotes != "")
                await Context.Channel.SendMessageAsync(emotes);

            await Context.Channel.SendMessageAsync("\nAnimated emotes | " + animated.Count);

            emotes = "";

            for (int i = 1; i <= animated.Count; i++)
            {
                if (i % 9 == 0)
                {
                    emotes += "<a:" + animated[i - 1].Name + ":" + animated[i - 1].Id + ">";
                    await Context.Channel.SendMessageAsync(emotes);
                    emotes = "";
                }
                else
                {
                    emotes += "<a:" + animated[i - 1].Name + ":" + animated[i - 1].Id + ">";
                }
            }

            if (emotes != "")
                await Context.Channel.SendMessageAsync(emotes);
        }

        [Command("setnick", RunMode = RunMode.Async)]
        [Summary("Set new nickname for a user. No name - Resets the nicknames\n+setnick [user] (name)")]
        [RequireUserPermission(GuildPermission.ManageNicknames, ErrorMessage = "You don't have required permission: Manage Nicknames")]
        [RequireBotPermission(GuildPermission.ManageNicknames, ErrorMessage = "Lack of required permission: Manage Nicknames")]
        public async Task SetNick(string name = null, [Remainder] string command = null)
        {
            SocketGuildUser user = Helpers.extractUser(Context, command);

            if (user == null)
            {
                await Context.Channel.SendMessageAsync("User not found!");
                return;
            }

            await user.ModifyAsync(x => x.Nickname = name == null ? user.Username : name);
        }

        [Command("setnicks", RunMode = RunMode.Async)]
        [Summary("Set new nicknames for users with specific role. No name - Resets the nicknames \n +setnick [role] (name)")]
        [RequireUserPermission(GuildPermission.ManageNicknames, ErrorMessage = "You don't have required permission: Manage Nicknames")]
        [RequireBotPermission(GuildPermission.ManageNicknames, ErrorMessage = "Lack of required permission: Manage Nicknames")]
        public async Task SetNicks(SocketRole userRole = null, string name = null)
        {
            if (userRole == null)
            {
                await Context.Channel.SendMessageAsync("Role not found!\n+setnicks [role] [name]");
                return;
            }

            foreach (SocketRole role in Context.Guild.Roles)
                if (role.Name == userRole.Name)
                    foreach (SocketGuildUser user in role.Members)
                        await user.ModifyAsync(x => x.Nickname = name == null ? user.Username : name);
        }

        [Command("createrole", RunMode = RunMode.Async)]
        [Summary("Creates a role with a specified name and optional hex color.\n+createrole [name] (#hexvalue)")]
        [RequireUserPermission(GuildPermission.ManageRoles, ErrorMessage = "You don't have required permission: Manage Roles")]
        [RequireBotPermission(GuildPermission.ManageRoles, ErrorMessage = "Lack of required permission: Manage Roles")]
        public async Task CreateRole(string name = null, string color = null)
        {
            if (name == null)
            {
                await Context.Channel.SendMessageAsync("Set a role name!\n+createrole [name] (#hexvalue)");
                return;
            }

            Color newColor = new Color(UInt32.Parse(color.Substring(1), System.Globalization.NumberStyles.HexNumber));
            await Context.Guild.CreateRoleAsync(name, null, newColor, false, true);
        }

        [Command("deleterole", RunMode = RunMode.Async)]
        [Summary("Removes a role from server\n+deleterole [role]")]
        [RequireUserPermission(GuildPermission.ManageRoles, ErrorMessage = "You don't have required permission: Manage Roles")]
        [RequireBotPermission(GuildPermission.ManageRoles, ErrorMessage = "Lack of required permission: Manage Roles")]
        public async Task DelRole(SocketRole role = null)
        {
            if (role == null)
            {
                await Context.Channel.SendMessageAsync("Role not found!");
                return;
            }

            await Context.Guild.GetRole(role.Id).DeleteAsync();
        }

        [Command("roles", RunMode = RunMode.Async)]
        [Summary("Lists and pings all user's roles.\n+roles [user]")]
        [RequireUserPermission(GuildPermission.ManageRoles, ErrorMessage = "You don't have required permission: Manage Roles")]
        [RequireBotPermission(GuildPermission.ManageRoles, ErrorMessage = "Lack of required permission: Manage Roles")]
        public async Task Roles(SocketGuildUser user = null)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync("User not found!");
                return;
            }

            List<SocketRole> socketRoles = new List<SocketRole>(user.Roles.Where(x => !x.IsEveryone));

            string roles = "";

            await Context.Channel.SendMessageAsync("Roles count | " + socketRoles.Count);

            for (int i = 1; i <= socketRoles.Count; i++)
            {
                if (i % 9 == 0)
                {
                    roles += socketRoles[i - 1].Name + " ";
                    await Context.Channel.SendMessageAsync(roles);
                    roles = "";
                }
                else
                {
                    roles += socketRoles[i - 1].Name + " ";
                }
            }

            if (roles != "")
                await Context.Channel.SendMessageAsync(roles);
        }

        [Command("rolecolor", RunMode = RunMode.Async)]
        [Summary("Changes specified role's color\n+rolecolor [role] (#hexvalue)")]
        [RequireUserPermission(GuildPermission.ManageRoles, ErrorMessage = "You don't have required permission: Manage Roles")]
        [RequireBotPermission(GuildPermission.ManageRoles, ErrorMessage = "Lack of required permission: Manage Roles")]
        public async Task RoleColor(SocketRole role = null, string color = null)
        {
            if (role == null)
            {
                await Context.Channel.SendMessageAsync("Role not found!\n+rolecolor [name] (#hexvalue)");
                return;
            }

            if (UInt32.TryParse(color.Substring(1), System.Globalization.NumberStyles.HexNumber, null, out _))
            {
                await Context.Guild.GetRole(role.Id).ModifyAsync(x => x.Color = new Color(UInt32.Parse(color.Substring(1), System.Globalization.NumberStyles.HexNumber)));
                await ReplyAsync("Color has been set!");
            }
            else
            {
                await ReplyAsync("Wrong hex value!");
            }
        }

        [Command("rolename", RunMode = RunMode.Async)]
        [Summary("Changes specified role's name\n+rolename [role] [new_name]")]
        [RequireUserPermission(GuildPermission.ManageRoles, ErrorMessage = "You don't have required permission: Manage Roles")]
        [RequireBotPermission(GuildPermission.ManageRoles, ErrorMessage = "Lack of required permission: Manage Roles")]
        public async Task RoleName(SocketRole role = null, string name = null)
        {
            if (role == null)
            {
                await Context.Channel.SendMessageAsync("Role not found!");
                return;
            }

            if (name == null)
            {
                await Context.Channel.SendMessageAsync("Name not specified!");
                return;
            }

            await Context.Guild.GetRole(role.Id).ModifyAsync(x => x.Name = name);
            await Context.Channel.SendMessageAsync("New name has been set!");
        }

        [Command("giverole", RunMode = RunMode.Async)]
        [Summary("Gives role to specified user.\n+giverole [user] [role]")]
        [RequireUserPermission(GuildPermission.ManageRoles, ErrorMessage = "You don't have required permission: Manage Roles")]
        [RequireBotPermission(GuildPermission.ManageRoles, ErrorMessage = "Lack of required permission: Manage Roles")]
        public async Task GiveRole(SocketGuildUser user = null, IRole role = null)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync("User not found!");
                return;
            }

            if (role == null)
            {
                await Context.Channel.SendMessageAsync("Role not specified!");
                return;
            }

            await user.AddRoleAsync(role);
        }

        [Command("removerole", RunMode = RunMode.Async)]
        [Summary("Removes role from specified user.\n+removerole [user] [role]")]
        [RequireUserPermission(GuildPermission.ManageRoles, ErrorMessage = "You don't have required permission: Manage Roles")]
        [RequireBotPermission(GuildPermission.ManageRoles, ErrorMessage = "Lack of required permission: Manage Roles")]
        public async Task RemoveRole(SocketGuildUser user = null, IRole role = null)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync("User not found!");
                return;
            }

            if (role == null)
            {
                await Context.Channel.SendMessageAsync("Role not specified!");
                return;
            }

            await user.RemoveRoleAsync(role);
        }

        [Command("lock", RunMode = RunMode.Async)]
        [Summary("Locks a channel. Blocks permission to send messages for standard users.\n+lock [channel]")]
        [RequireUserPermission(GuildPermission.ManageChannels, ErrorMessage = "You don't have required permission: Manage Channels")]
        [RequireBotPermission(GuildPermission.ManageChannels, ErrorMessage = "Lack of required permission: Manage Channels")]
        public async Task Lock(SocketGuildChannel channel = null)
        {
            if (channel == null)
            {
                await ReplyAsync("Channel not specified!\n+lock [channel]");
                return;
            }

            await channel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new OverwritePermissions(sendMessages: PermValue.Deny));
        }

        [Command("unlock", RunMode = RunMode.Async)]
        [Summary("Unlocks a channel.\n+unlock [channel]")]
        [RequireUserPermission(GuildPermission.ManageChannels, ErrorMessage = "You don't have required permission: Manage Channels")]
        [RequireBotPermission(GuildPermission.ManageChannels, ErrorMessage = "Lack of required permission: Manage Channels")]
        public async Task UnLock(SocketGuildChannel channel)
        {
            if (channel == null)
            {
                await ReplyAsync("Channel not specified!\n+lock [channel]");
                return;
            }

            await channel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new OverwritePermissions(sendMessages: PermValue.Allow));
        }
    }
}
