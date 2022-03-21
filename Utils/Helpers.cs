using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.Utils
{
    public static class Helpers
    {
        public static readonly string WORKING_DIRECTORY = Directory.GetCurrentDirectory();

        public static SocketGuildUser extractUser(SocketCommandContext ctx, string message)
        {
            string impliedUser = message.TrimStart().TrimEnd();

            if (impliedUser.StartsWith("<@"))
            {
                return ctx.Guild.GetUser(ctx.Message.MentionedUsers.First().Id);
            }
            else
            {
                try
                {
                    return ctx.Guild.GetUser(UInt64.Parse(impliedUser));
                }
                catch
                {
                    foreach (var user in ctx.Guild.Users)
                    {
                        if (user.Nickname == impliedUser || user.Username == impliedUser)
                        {
                            return user;
                        }
                    }
                }
            }
            return null;
        }

        public static bool AssertDirectory(String path)
        {
            bool isSuccess = true;
            if (!Directory.Exists($"{WORKING_DIRECTORY}/{path}"))
            {
                try
                {
                    Directory.CreateDirectory($"{WORKING_DIRECTORY}/{path}");
                }
                catch (Exception e)
                {
                    isSuccess = false;
                }
            }
            return isSuccess;
        }

        public static async Task<string> getHttpResponseString(string url)
        {
            HttpClient client = new HttpClient();
            return await client.GetStringAsync(url);
        }

        public static bool FileExists(String path)
        {
            if (File.Exists(path))
                return true;

            return false;
        }
    }
}
