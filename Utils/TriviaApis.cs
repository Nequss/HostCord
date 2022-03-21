using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HostCord.Utils
{
    public class TriviaApis
    {
        public class DogsApi
        {
            [JsonPropertyName("facts")]
            public string[] facts { get; set; }
        }

        public class MeowApi
        {
            [JsonPropertyName("data")]
            public string[] data { get; set; }
        }

        public class NumberApi
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }
        }

        public class JServiceApi
        {
            [JsonPropertyName("clues")]
            public Clue[] Clues { get; set; }
        }

        public class Clue
        {
            [JsonPropertyName("answer")]
            public string Answer { get; set; }

            [JsonPropertyName("question")]
            public string Question { get; set; }
        }

        public async Task<string> DogsApiResult()
        {
            var response = await new HttpClient().GetStringAsync("https://dog-api.kinduff.com/api/facts");
            DogsApi dogsapi = JsonSerializer.Deserialize<DogsApi>(response);
            return dogsapi.facts[0];
        }

        public async Task<string> MeowApiResult()
        {
            var response = await new HttpClient().GetStringAsync("https://meowfacts.herokuapp.com/");
            MeowApi meowapi = JsonSerializer.Deserialize<MeowApi>(response);
            return meowapi.data[0];
        }

        public async Task<string> NumberApiResult(string number, string type)
        {
            NumberApi numberapi;

            if (int.TryParse(number, out _) | number == "random")
            {
                if (type == "trivia" | type == "math" | type == "date" | type == "year")
                {
                    var response = await new HttpClient().GetStringAsync("http://numbersapi.com/" + number + "/" + type + "?json");
                    numberapi = JsonSerializer.Deserialize<NumberApi>(response);
                }
                else
                {
                    var response = await new HttpClient().GetStringAsync("http://numbersapi.com/" + number + "?json");
                    numberapi = JsonSerializer.Deserialize<NumberApi>(response);
                }
            }
            else
            {
                var response = await new HttpClient().GetStringAsync("http://numbersapi.com/random" + "?json");
                numberapi = JsonSerializer.Deserialize<NumberApi>(response);
            }

            return numberapi.Text;
        }

        public async Task<string> JServiceApiResult(string id)
        {
            var response = await new HttpClient().GetStringAsync("http://jservice.io/api/category?id=" + id);
            JServiceApi jServiceApi = JsonSerializer.Deserialize<JServiceApi>(response);

            Random rnd = new Random();
            int i = rnd.Next(0, jServiceApi.Clues.Length - 1);

            //do
            //{
            //} while (jServiceApi.Clues[i].Question == "=" & jServiceApi.Clues[i].Answer == "=");

            return "Q: " + jServiceApi.Clues[i].Question + Environment.NewLine + "A: " + jServiceApi.Clues[i].Answer;
        }
    }
}
