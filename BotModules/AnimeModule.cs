using Discord;
using Discord.Commands;
using HostCord.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HostCord.BotModules
{
    [Name("anime")]
    [Summary("Contains anime related commands")]
    public class AnimeModule : ModuleBase<SocketCommandContext>
    {
        [Command("neko", RunMode = RunMode.Async)]
        [Summary("Sends random anime neko image")]
        public async Task RandomNekoAsync()
        {
            var embed = await NekosLife.CreateEmbedWithImage("(≈>ܫ<≈)", "https://nekos.life/api/v2/img/neko");
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("smug", RunMode = RunMode.Async)]
        [Summary("Sends random anime smug image")]
        public async Task RandomSmugAsync()
        {
            var embed = await NekosLife.CreateEmbedWithImage(@"(ಸ‿‿ಸ)", "https://nekos.life/api/v2/img/smug");
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("slap", RunMode = RunMode.Async)]
        [Summary("Sends a random anime slap gif")]
        public async Task RandomSlapAsync([Remainder] string specifiedUser)
        {
            IUser user = Helpers.extractUser(Context, specifiedUser);
            if (NekosLife.userNotFound(Context, user))
                return;
            var embed = await NekosLife.CreateEmbedWithImage(Context.User.Username + " slaps " + user.Username + @"(ಸ‿‿ಸ)", "https://nekos.life/api/v2/img/slap");
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("kiss", RunMode = RunMode.Async)]
        [Summary("Sends a random anime kiss gif")]
        public async Task RandomKissAsync([Remainder] string specifiedUser)
        {
            IUser user = Helpers.extractUser(Context, specifiedUser);
            if (NekosLife.userNotFound(Context, user))
                return;
            var embed = await NekosLife.CreateEmbedWithImage(Context.User.Username + " kisses " + user.Username + " (ꈍᴗꈍ)ε｀*)", "https://nekos.life/api/v2/img/kiss");
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("poke", RunMode = RunMode.Async)]
        [Summary("Sends a random anime poke gif")]
        public async Task RandomPokeAsync([Remainder] string specifiedUser)
        {
            IUser user = Helpers.extractUser(Context, specifiedUser);
            if (NekosLife.userNotFound(Context, user))
                return;
            var embed = await NekosLife.CreateEmbedWithImage(Context.User.Username + " pokes " + user.Username + " ( ๑‾̀◡‾́)σ»", "https://nekos.life/api/v2/img/poke");
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("hug", RunMode = RunMode.Async)]
        [Summary("Sends a random anime hug gif")]
        public async Task RandomHugAsync([Remainder] string specifiedUser)
        {
            IUser user = Helpers.extractUser(Context, specifiedUser);
            if (NekosLife.userNotFound(Context, user))
                return;
            var embed = await NekosLife.CreateEmbedWithImage(Context.User.Username + " hugs " + user.Username + " (✿˶◕‿◕˶人◕ᴗ◕✿)", "https://nekos.life/api/v2/img/hug");
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("baka", RunMode = RunMode.Async)]
        [Summary("Sends a random anime baka gif")]
        public async Task RandomBakaAsync([Remainder] string specifiedUser)
        {
            IUser user = Helpers.extractUser(Context, specifiedUser);
            if (NekosLife.userNotFound(Context, user))
                return;
            var embed = await NekosLife.CreateEmbedWithImage(Context.User.Username + " thinks " + user.Username + " is baka! (◣_◢)", "https://nekos.life/api/v2/img/baka");
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        [Command("pat", RunMode = RunMode.Async)]
        [Summary("Sends a random anime pat gif")]
        public async Task RandomPatAsync([Remainder] string specifiedUser)
        {
            IUser user = Helpers.extractUser(Context, specifiedUser);
            if (NekosLife.userNotFound(Context, user))
                return;
            var embed = await NekosLife.CreateEmbedWithImage(Context.User.Username + " pats " + user.Username + " (；^＿^)ッ☆(　゜w゜)", "https://nekos.life/api/v2/img/pat");
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }
    }
}
