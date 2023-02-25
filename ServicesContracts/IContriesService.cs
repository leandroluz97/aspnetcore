using CRUDoperations.DTO;

namespace ServicesContracts
{
    public interface IContriesService
    {
        CountryResponse AddCountry(CountryAddRequest? countryAddRequest);
        List<CountryResponse> GetAllCountries();

    }
}