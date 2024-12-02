using rekrutacja.Services;
using rekrutacja.Entities;

namespace rekrutacja.Entities
{
    public class WeatherApiResponse
    {
        public Entities.MainData Main { get; set; }
        public List<WeatherDescription> Weather { get; set; }
    }
}
