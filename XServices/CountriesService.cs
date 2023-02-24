using CRUDoperations.DTO;
using Entities;
using ServicesContracts;

namespace XServices
{
    public class CountriesService : IContriesService
    {
        List<Country> _countries;
        public CountriesService()
        {
            _countries = new List<Country>();
            _countries.Add(new Country() { CountryId = Guid.NewGuid(), CountryName = "Caboverde" });
            _countries.Add(new Country() { CountryId = Guid.NewGuid(), CountryName = "Portugal" });
            _countries.Add(new Country() { CountryId = Guid.NewGuid(), CountryName = "Brazil" });

        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException();
            }
            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
            {
                throw new ArgumentException();
            }
            if(_countries.Find(c => c.CountryName == countryAddRequest.CountryName) != null) 
            {
                throw new ArgumentException();
            }
            Country country = new Country() { CountryId = new Guid("20d5495a-1995-4f9e-827d-a99494d25cfa"), CountryName = countryAddRequest.CountryName };
            _countries.Add(country);
            return country.ToCountryResponse();
        }
    }
}