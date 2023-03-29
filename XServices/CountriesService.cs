using CRUDoperations.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
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

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if(await _db.Countries.CountAsync(c => c.CountryName == countryAddRequest.CountryName) > 0) 
            {
                throw new ArgumentException("Given country name already exists!");
            }
            Country country = new Country() 
            { 
                CountryId = Guid.NewGuid(), 
                CountryName = countryAddRequest.CountryName
            };
            _db.Countries.Add(country);
            await _db.SaveChangesAsync();

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse>? GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
                throw new ArgumentNullException(nameof(countryId));
            }

            if (countryId == Guid.Empty)
            {
                throw new ArgumentException(nameof(countryId));
            }
            Country? country = await _db.Countries.FirstOrDefault(c => c.CountryId.Equals(countryId));

            if(country == null)
            {
                return null;
            }

            return country.ToCountryResponse();
        }
    }
}