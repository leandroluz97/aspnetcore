using Models;
using ServiceContracts;


namespace Services
{
    public class WeatherService : IWeatherService, IDisposable
    {
        private List<CityWeather> _cityWeathers;
        public List<CityWeather> CitiesWeather  {
            get
                {
                    return _cityWeathers;
                }
        }

        public WeatherService()
        {
            CityWeather cityWaether1 = new CityWeather(){CityUniqueCode = "LDN", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 8:00"), TemperatureFahrenheit = 33};
            CityWeather cityWaether2 = new CityWeather() { CityUniqueCode = "NYC", CityName = "London", DateAndTime = Convert.ToDateTime("2030-01-01 3:00"), TemperatureFahrenheit = 60 };
            CityWeather cityWaether3 = new CityWeather() { CityUniqueCode = "PAR", CityName = "Paris", DateAndTime = Convert.ToDateTime("2030-01-01 9:00"), TemperatureFahrenheit = 82 };
            _cityWeathers = new List<CityWeather>();
            _cityWeathers.Add(cityWaether1);
           _cityWeathers.Add(cityWaether2);
           _cityWeathers.Add(cityWaether3);
        }
        public List<CityWeather> GetCitiesWeather()
        {
            return CitiesWeather;
        }

        public CityWeather? GetCityWeather(string cityCode)
        {
            return CitiesWeather.Find(city => city.CityUniqueCode == cityCode);
        }
        public void Dispose()
        {
                
        }
    }
}