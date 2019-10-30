using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace AsyncTests
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        private const string weatherPath = "/data/2.5/weather";
        private const string appid = "b6907d289e10d714a6e88b30761fae22";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<WeatherDto> GetWeather(string city)
        {
            
            var request = CreateRequest(city, appid);
            var result = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false);
            Thread.Sleep(1000);
            //var contentStreamString  = await result.Content.ReadAsStringAsync();
            await using var contentStream = await result.Content.ReadAsStreamAsync();
            WeatherDto dto = await JsonSerializer.DeserializeAsync<WeatherDto>(contentStream, new JsonSerializerOptions() {PropertyNameCaseInsensitive = true});
            Console.WriteLine("ASYNC: " + city+ " " + dto.Main.Temp + " Thread: "+Thread.CurrentThread.ManagedThreadId);
            return dto;
        }

        public async Task GetWeatherForCities(IEnumerable<string> cities)
        {
            await GetWeather("brighton");
            List<Task<WeatherDto>> weatherTasks = cities.Select(GetWeather).ToList();
            await Task.WhenAll(weatherTasks);
            Console.WriteLine("NUMBER OF TASKS: "+ weatherTasks.Count);
        }

        private static HttpRequestMessage CreateRequest(string city, string appKey)
        {
            string reqPath = GetUrl(city, appKey);
            return new HttpRequestMessage(HttpMethod.Get, reqPath);
        }

        private static string GetUrl(string city,
            string apiKey)
        {
           
            var query = HttpUtility.ParseQueryString("");
            query["q"] = city;
            query["appid"] = apiKey;
            string url = $"{weatherPath}?{query}";
            return url;
        }
    }
}
