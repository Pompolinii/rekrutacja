using Newtonsoft.Json;

namespace rekrutacja.Entities
{
    public class MainData
    {
        [JsonProperty("temp")]
        public double Temp { get; set; }

        [JsonProperty("temp_min")]
        public double TempMin { get; set; } 

        [JsonProperty("temp_max")]
        public double TempMax { get; set; } 

        public int Humidity { get; set; }
    }
}
