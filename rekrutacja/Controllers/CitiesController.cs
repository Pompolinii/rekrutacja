using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using rekrutacja.Entities;
using rekrutacja;
using rekrutacja.Services;

namespace rekrutacja.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CitiesController(AppDbContext context)
        {
            _context = context;
        }

       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            return await _context.Cities.ToListAsync();
        }

        
        [HttpGet("{name}")]
        public async Task<ActionResult<City>> GetCityByName(string name)
        {
            var city = await _context.Cities
                                      .FirstOrDefaultAsync(c => c.Name == name);

            if (city == null)
            {
                return NotFound();
            }

            return city;
        }

        
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(City city)
        {
            
            if (await _context.Cities.AnyAsync(c => c.Name == city.Name))
            {
                return Conflict("Miasto o tej nazwie już istnieje.");
            }

            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCityByName), new { name = city.Name }, city);
        }


        [HttpPut("{name}")]
        public async Task<IActionResult> PutCity(string name, City city)
        {
            
            var existingCity = await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);

            if (existingCity == null)
            {
                return NotFound();  
            }

            
            existingCity.Name = city.Name;

            _context.Entry(existingCity).State = EntityState.Modified;

           
            await _context.SaveChangesAsync();

            return NoContent();  
        }


        
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteCity(string name)
        {
            var city = await _context.Cities
                                      .FirstOrDefaultAsync(c => c.Name == name);

            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExistsByName(string name)
        {
            return _context.Cities.Any(e => e.Name == name);
        }

        [HttpGet("{name}/weather")]
        public async Task<IActionResult> GetCityWeather(string name, [FromServices] WeatherService weatherService)
        {
            var city = await _context.Cities.FirstOrDefaultAsync(c => c.Name == name);

            if (city == null)
            {
                return NotFound($"City with name '{name}' not found.");
            }

            try
            {
               
                var coordinates = await weatherService.GetCityCoordinatesAsync(name);

                if (coordinates.lat == 0 && coordinates.lon == 0)
                {
                    return NotFound($"Coordinates for city '{name}' could not be found.");
                }

                
                var weatherData = await weatherService.GetWeatherByCoordinatesAsync(coordinates);

                
                return Ok(weatherData);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error fetching weather data: {ex.Message}");
            }
        }






    }
}
