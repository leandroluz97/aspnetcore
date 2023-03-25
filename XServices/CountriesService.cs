using CRUDoperations.DTO;
using Entities;
using ServicesContracts;

namespace XServices
{
    public class CountriesService : ICountriesService
    {
        private readonly PersonsDbContext _db;
        public CountriesService(PersonsDbContext personsDbContext)
        {
            _db = personsDbContext;
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
            if(_db.Countries.Count(c => c.CountryName == countryAddRequest.CountryName) > 0) 
            {
                throw new ArgumentException("Given country name already exists!");
            }
            Country country = new Country() 
            { 
                CountryId = Guid.NewGuid(), 
                CountryName = countryAddRequest.CountryName
            };
            _db.Countries.Add(country);
            _db.SaveChanges();

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _db.Countries.Select(country => country.ToCountryResponse()).ToList();
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
            Country? country = _db.Countries.FirstOrDefault(c => c.CountryId.Equals(countryId));

            if(country == null)
            {
                return null;
            }

            return country.ToCountryResponse();
        }
    }
}