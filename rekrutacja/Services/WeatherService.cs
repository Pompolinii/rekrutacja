using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using rekrutacja.Entities;

namespace rekrutacja.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "cb87a0e886e3104fffcc16c1219959ef";
        private const string BaseGeoUrl = "https://api.openweathermap.org/geo/1.0/direct?q={0}&limit=1&appid={1}";
        private const string BaseWeatherUrl = "https://api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}";

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        
        public async Task<(double lat, double lon, string country, string state)> GetCityCoordinatesAsync(string city)
        {
            var url = string.Format(BaseGeoUrl, WebUtility.UrlEncode(city), ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Geocoding API request failed with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

            var geoData = JsonConvert.DeserializeObject<List<GeoCoordinates>>(content);

            if (geoData != null && geoData.Count > 0)
            {
                return (geoData[0].Lat, geoData[0].Lon, geoData[0].Country, geoData[0].State);
            }

            return (0, 0, null, null);
        }

        
        public async Task<WeatherData> GetWeatherByCoordinatesAsync((double lat, double lon, string country, string state) coordinates)
        {
            var url = string.Format(BaseWeatherUrl, coordinates.lat, coordinates.lon, ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Weather API request failed with status code {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();

           
            var weatherApiResponse = JsonConvert.DeserializeObject<WeatherApiResponse>(content);

            if (weatherApiResponse == null)
            {
                throw new Exception("Failed to parse weather API response.");
            }

            
            weatherApiResponse.Main.Temp = Math.Round(ConvertKelvinToCelsius(weatherApiResponse.Main.Temp), 1);
            weatherApiResponse.Main.TempMin = Math.Round(ConvertKelvinToCelsius(weatherApiResponse.Main.TempMin), 1);
            weatherApiResponse.Main.TempMax = Math.Round(ConvertKelvinToCelsius(weatherApiResponse.Main.TempMax), 1);

            
            return new WeatherData
            {
                Temp = weatherApiResponse.Main.Temp,
                TempMin = weatherApiResponse.Main.TempMin,
                TempMax = weatherApiResponse.Main.TempMax,
                Description = weatherApiResponse.Weather[0].Description,
                Humidity = weatherApiResponse.Main.Humidity,
                Country = coordinates.country,
                State = coordinates.state 
            };
        }

        
        private double ConvertKelvinToCelsius(double kelvinTemp)
        {
            return kelvinTemp - 273.15;
        }
    }

 
   

    
    
     

  

    public class WeatherData
    {
        public double Temp { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public string Description { get; set; }
        public int Humidity { get; set; }

        public string Country { get; set; }
        public string State { get; set; } // Dodane pole stanu/regionu
    }
}
