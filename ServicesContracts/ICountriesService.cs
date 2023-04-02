using CRUDoperations.DTO;
using Microsoft.AspNetCore.Http;

namespace ServicesContracts
{
    public interface ICountriesService
    {
        Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);
        Task<List<CountryResponse>> GetAllCountries();
        Task<CountryResponse>? GetCountryByCountryId(Guid? countryId);
        Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
    }
}