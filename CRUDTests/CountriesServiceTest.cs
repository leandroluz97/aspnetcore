using CRUDoperations.DTO;
using ServicesContracts;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XServices;
using Xunit;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly IContriesService _countriesServices;
        public CountriesServiceTest()
        {
            _countriesServices = new CountriesService();
        }

        [Fact]
        public void AddCountry_ValidCountryDetails()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Leandro" };
            CountryResponse response = new CountryResponse() { CountryName= "Leandro", CountryId = new Guid("20d5495a-1995-4f9e-827d-a99494d25cfa") };
            
            //Act
            var actual = _countriesServices.AddCountry(request);

            //Assert
            Assert.True(actual.CountryId != Guid.Empty);
            //var obj1Str = JsonConvert.SerializeObject(response);
            //var obj2Str = JsonConvert.SerializeObject(actual);
            //Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() => {
                //Act
                _countriesServices.AddCountry(request);
            });
        }

        [Fact]
        public void AddCountry_NameIsNull()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };

            //Assert
            Assert.Throws<ArgumentException>(() =>{
                //Act
                _countriesServices.AddCountry(request);
            });            
        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest() { CountryName ="Angola" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "Angola" };

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _countriesServices.AddCountry(request1);
                _countriesServices.AddCountry(request2);
            });
        }

    }
}
