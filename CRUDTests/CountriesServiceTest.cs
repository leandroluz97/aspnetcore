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
        public void ExpectValidResponse()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Leandro" };
            CountryResponse response = new CountryResponse() { CountryName= "Leandro", CountryId = new Guid("20d5495a-1995-4f9e-827d-a99494d25cfa") };
            
            //Act
            var actual = _countriesServices.AddCountry(request);

            //Assert
            //Assert.Equal(response, actual);
            var obj1Str = JsonConvert.SerializeObject(response);
            var obj2Str = JsonConvert.SerializeObject(actual);
            Assert.Equal(obj1Str, obj2Str);
        }

        [Fact]
        public void ExpectCountryAddRequestArgumentNullException()
        {
            try
            {
                //Arrange
                CountryResponse response = new CountryResponse() { CountryName = "Leandro", CountryId = Guid.NewGuid() };
                //Act
                var actual = _countriesServices.AddCountry(null);
            }
            catch (ArgumentNullException exception)
            {
                //Assert
                Assert.IsType<ArgumentNullException>(exception);
            } 
        }

        [Fact]
        public void ExpectCountryAddRequestNameArgumentNullException()
        {
            try
            {
                //Arrange
                CountryAddRequest request = new CountryAddRequest() { CountryName = null };
                //Act
                var actual = _countriesServices.AddCountry(request);
            }
            catch (ArgumentNullException exception)
            {
                //Assert
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

        [Fact]
        public void ExpectSameNameToThrowArgumentNullException()
        {
            try
            {
                //Arrange
                CountryAddRequest request = new CountryAddRequest() { CountryName ="Caboverde" };
                //Act
                var actual = _countriesServices.AddCountry(null);
            }
            catch (ArgumentNullException exception)
            {
                //Assert
                Assert.IsType<ArgumentNullException>(exception);
            }
        }

    }
}
