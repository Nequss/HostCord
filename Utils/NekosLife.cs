using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HostCord.Utils
{
    public static class NekosLife
    {
        public class NekosLifeApi
        {
            [JsonPropertyName("url")]
            public string Url { get; set; }
        }

        public static async Task<EmbedBuilder> CreateEmbedWithImage(string title, string url)
        {
            NekosLifeApi api = JsonSerializer.Deserialize<NekosLifeApi>(await Helpers.getHttpResponseString(url));

            var embed = new EmbedBuilder()
            {
                Title = title,
                Color = Color.Purple,
                ImageUrl = api.Url
            };
            return embed;
        }

        public static bool userNotFound(SocketCommandContext ctx, IUser impliedUser)
        {
            if (impliedUser == null)
            {
                ctx.Channel.SendMessageAsync($"User not found.");
                return true;
            }

            return false;
        }
    }
}
