using Discord.Commands;
using HostCord.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostCord.BotModules
{
    [Name("trivia")]
    [Summary("Contains trivia commands")]
    public class TriviaModule : ModuleBase<SocketCommandContext>
    {
        TriviaApis triviaApis = new TriviaApis();

        [Command("numbers")]
        [Summary("Posts random facts about numbers.\n+trivia numbers <number> <trivia|math|date|year>")]
        public async Task Numbers(params string[] objects)
        {
            string result = triviaApis.NumberApiResult(objects.Length > 0 ? objects[0] : string.Empty, objects.Length > 1 ? objects[1] : string.Empty).Result;
            await Context.Channel.SendMessageAsync(result);
        }

        [Command("cats")]
        [Summary("Random facts about cats. Meow!")]
        public async Task Cats() => await Context.Channel.SendMessageAsync(triviaApis.MeowApiResult().Result);

        [Command("dogs")]
        [Summary("Mad barkers trivias!")]
        public async Task Dogs() => await Context.Channel.SendMessageAsync(triviaApis.DogsApiResult().Result);

        [Command("sport")]
        [Summary("Sports facts.")]
        public async Task Sports() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("42").Result);

        [Command("animals")]
        [Summary("Everything about animals.")]
        public async Task Animals() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("21").Result);

        [Command("colors")]
        [Summary("Colors trivias!")]
        public async Task Colors() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("36").Result);

        [Command("insects")]
        [Summary("For entomologists.")]
        public async Task Insects() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("35").Result);

        [Command("tv")]
        [Summary("Random TV-series facts.")]
        public async Task Television() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("67").Result);

        [Command("business")]
        [Summary("Business & Industry trivias.")]
        public async Task Business() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("176").Result);

        [Command("words")]
        [Summary("Etymology - The origins of some words.")]
        public async Task Words() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("223").Result);

        [Command("anatomy")]
        [Summary("Leave the frogs alone!")]
        public async Task Anatomy() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("356").Result);

        [Command("measures")]
        [Summary("Trivias about all kind of units.")]
        public async Task Measures() => await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("420").Result);

        [Command("history")]
        [Summary("History facts.")]
        public async Task History()
        {
            if (new Random().Next(0, 2) == 0)
                await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("114").Result);
            else
                await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("530").Result);
        }

        [Command("food")]
        [Summary("Facts about nom noming.")]
        public async Task Food()
        {
            if (new Random().Next(0, 2) == 0)
                await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("777").Result);
            else
                await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("49").Result);
        }

        [Command("people")]
        [Summary("Reasons why famous people are famous.")]
        public async Task People()
        {
            switch (new Random().Next(0, 3))
            {
                case 0:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("442").Result);
                    break;
                case 1:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("1478").Result);
                    break;
                case 2:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("4138").Result);
                    break;
            }
        }

        [Command("literature")]
        [Summary("Category for book-worms.")]
        public async Task Literature()
        {
            switch (new Random().Next(0, 4))
            {
                case 0:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("574").Result);
                    break;
                case 1:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("10").Result);
                    break;
                case 2:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("779").Result);
                    break;
                case 3:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("484").Result);
                    break;
            }
        }

        [Command("science")]
        [Summary("Random knowledge and trivias about science.")]
        public async Task Science()
        {
            switch (new Random().Next(0, 6))
            {
                case 0:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("25").Result);
                    break;
                case 1:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("23526").Result);
                    break;
                case 2:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("579").Result);
                    break;
                case 3:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("1087").Result);
                    break;
                case 4:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("2046").Result);
                    break;
                case 5:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("950").Result);
                    break;
            }
        }

        [Command("fashion")]
        [Summary("Interesting facts about fashion.")]
        public async Task Fashion()
        {
            switch (new Random().Next(0, 4))
            {
                case 0:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("26").Result);
                    break;
                case 1:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("383").Result);
                    break;
                case 2:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("1735").Result);
                    break;
                case 3:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("953").Result);
                    break;
            }
        }

        [Command("geography")]
        [Summary("Facts about places around the globe.")]
        public async Task Geography()
        {
            switch (new Random().Next(0, 4))
            {
                case 0:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("582").Result);
                    break;
                case 1:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("534").Result);
                    break;
                case 2:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("88").Result);
                    break;
                case 3:
                    await Context.Channel.SendMessageAsync(triviaApis.JServiceApiResult("1479").Result);
                    break;
            }
        }
    }
}
