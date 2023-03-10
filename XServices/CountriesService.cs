using CRUDoperations.DTO;
using Entities;
using ServicesContracts;

namespace XServices
{
    public class CountriesService : ICountriesService
    {
        List<Country> _countries;
        public CountriesService(bool initialize =  true)
        {
            _countries = new List<Country>();
            if (initialize)
            {
                _countries.Add(new Country() { CountryId = Guid.Parse("515F75D9-F590-4645-A452-9346FE774466"), CountryName = "Caboverde" });
                _countries.Add(new Country() { CountryId = Guid.Parse("C5B6D48E-1013-49FA-A1EA-0DF7F8943DD2"), CountryName = "USA" });
                _countries.Add(new Country() { CountryId = Guid.Parse("8D90E67E-300F-4341-BFE4-41654F1792E5"), CountryName = "Portugal" });
                _countries.Add(new Country() { CountryId = Guid.Parse("E4B742FC-AF91-4634-9EA8-5F92E03F8AFE"), CountryName = "Brazil" });
                _countries.Add(new Country() { CountryId = Guid.Parse("A283BB1D-CB1D-4CAC-A7D0-E9B4E0721851"), CountryName = "UK" });
            }
            

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