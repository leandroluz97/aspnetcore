using CRUDoperations.DTO;

namespace ServicesContracts
{
    public interface ICountriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountries();
        CountryResponse? GetCountryByCountryId(Guid? countryId);

    }
}