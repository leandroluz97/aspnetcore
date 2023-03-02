using CRUDoperations.DTO;
using Entities;
using ServicesContracts;

namespace XServices
{
    public class CountriesService : ICountriesService
    {
        List<Country> _countries;
        public CountriesService()
        {
            _countries = new List<Country>();
            //_countries.Add(new Country() { CountryId = Guid.NewGuid(), CountryName = "Caboverde" });
            //_countries.Add(new Country() { CountryId = Guid.NewGuid(), CountryName = "Portugal" });
            //_countries.Add(new Country() { CountryId = Guid.NewGuid(), CountryName = "Brazil" });

        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if(_countries.Where(c => c.CountryName == countryAddRequest.CountryName).Count() > 0) 
            {
                throw new ArgumentException("Given country name already exists!");
            }
            Country country = new Country() 
            { 
                CountryId = Guid.NewGuid(), 
                CountryName = countryAddRequest.CountryName
            };
            _countries.Add(country);
            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(country => country.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
                throw new ArgumentNullException(nameof(countryId));
            }

            if (countryId == Guid.Empty)
            {
                throw new ArgumentException(nameof(countryId));
            }
            Country? country = _countries.FirstOrDefault(c => c.CountryId.Equals(countryId));

            if(country == null)
            {
                return null;
            }

            return country.ToCountryResponse();
        }
    }
}