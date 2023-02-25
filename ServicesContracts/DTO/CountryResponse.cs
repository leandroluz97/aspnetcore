using Entities;

namespace CRUDoperations.DTO
{
    public class CountryResponse
    {
        public  Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj == null)
            {
                return false;
            }
            if(obj.GetType() != typeof(CountryResponse))
            {
                return false;
            }

            CountryResponse countryToCompare = (CountryResponse)obj;
            return countryToCompare.CountryId == CountryId && countryToCompare.CountryName == CountryName;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();  
        }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse() 
            { 
                CountryId = country.CountryId,
                CountryName = country.CountryName,
            };
        }
    }
}
