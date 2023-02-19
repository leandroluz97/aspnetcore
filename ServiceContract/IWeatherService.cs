
using Models;

namespace ServiceContracts
{
    public interface IWeatherService
    {
        public List<CityWeather> GetCitiesWeather();
        public CityWeather GetCityWeather(string cityCode);
    }
}