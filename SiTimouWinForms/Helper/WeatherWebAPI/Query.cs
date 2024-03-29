﻿using System.Collections.Generic;
using gov.minahasa.sitimou.Helper.WeatherWebAPI;
using Newtonsoft.Json.Linq;

namespace gov.minahasa.sitimou.Helper.WeatherWebAPI
{
    public class Query
    {
        public bool ValidRequest { get; }
        public Coord Coord { get; }
        public List<Weather> Weathers { get; } = new List<Weather>();
        public string Base { get; }
        public Main Main { get; }
        public double Visibility { get; }
        public Wind Wind { get; }
        public Rain Rain { get; }
        public Snow Snow { get; }
        public Clouds Clouds { get; }
        public Sys Sys { get; }
        public int Id { get; }
        public string Name { get; }
        public int Cod { get; }

        public Query(string apiKey, string queryStr)
        {
            var jsonData = JObject.Parse(new System.Net.WebClient().DownloadString(
                $"http://api.openweathermap.org/data/2.5/weather?appid={apiKey}&q={queryStr}"));
            if(jsonData.SelectToken("cod").ToString() == "200")
            {
                ValidRequest = true;
                Coord = new Coord(jsonData.SelectToken("coord"));
                foreach (var weather in jsonData.SelectToken("weather"))
                    Weathers.Add(new Weather(weather));
                Base = jsonData.SelectToken("base").ToString();
                Main = new Main(jsonData.SelectToken("main"));
                if(jsonData.SelectToken("visibility") != null)
                    Visibility = double.Parse(jsonData.SelectToken("visibility").ToString());
                Wind = new Wind(jsonData.SelectToken("wind"));
                if(jsonData.SelectToken("rain") != null)
                    Rain = new Rain(jsonData.SelectToken("rain"));
                if (jsonData.SelectToken("snow") != null)
                    Snow = new Snow(jsonData.SelectToken("snow"));
                Clouds = new Clouds(jsonData.SelectToken("clouds"));
                Sys = new Sys(jsonData.SelectToken("sys"));
                Id = int.Parse(jsonData.SelectToken("id").ToString());
                Name = jsonData.SelectToken("name").ToString();
                Cod = int.Parse(jsonData.SelectToken("cod").ToString());
            } else
            {
                ValidRequest = false;
            }
        }
    }
}
