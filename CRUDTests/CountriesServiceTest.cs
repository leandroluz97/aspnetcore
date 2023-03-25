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
using Entities;
using Microsoft.EntityFrameworkCore;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesServices;
        public CountriesServiceTest()
        {
            _countriesServices = new CountriesService(new PersonsDbContext(new DbContextOptionsBuilder<PersonsDbContext>().Options));
        }

        #region AddCountry
        [Fact]
        public void AddCountry_ValidCountryDetails()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Leandro" };

            //Act
            var actual = _countriesServices.AddCountry(request);
            List<CountryResponse> countries = _countriesServices.GetAllCountries();

            //Assert
            //JsonConvert.SerializeObject(response);
            Assert.True(actual.CountryId != Guid.Empty);
            Assert.Contains(actual, countries);
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
            Assert.Throws<ArgumentException>(() => {
                //Act
                _countriesServices.AddCountry(request);
            });
        }

        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "Angola" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "Angola" };

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _countriesServices.AddCountry(request1);
                _countriesServices.AddCountry(request2);
            });
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public void GetAllCountries_Countries()
        {
            //Arrange
            List<CountryAddRequest> countriesRequestList = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "Angola" },
                new CountryAddRequest() { CountryName = "Caboverde" },
                new CountryAddRequest() { CountryName = "Portugal" }
            };

            List<CountryResponse> countriesResponses = new List<CountryResponse>();
            foreach (CountryAddRequest request in countriesRequestList)
            {
                countriesResponses.Add(_countriesServices.AddCountry(request));
            }

            //Act
            List<CountryResponse> countries = _countriesServices.GetAllCountries();

            foreach (CountryResponse response in countriesResponses)
            {
                //Assert
                Assert.Contains(response, countries);
            }
        }

        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> countries = _countriesServices.GetAllCountries();
            //Assert
            Assert.True(0 == countries.Count);
        }
        #endregion

        #region GetCountryByCountryId
        [Fact]
        public void GetCountryByCountryId_GuidIsNull()
        {
            //Arrange
            Guid? guid = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesServices.GetCountryByCountryId(guid);
            });
        }

        [Fact]
        public void GetCountryByCountryId_GuidIsInvalid()
        {
            //Arrange
            Guid guid = Guid.Empty;

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesServices.GetCountryByCountryId(guid);
            });
        }

        [Fact]
        public void GetCountryByCountryId_Notfound()
        {
            //Arrange
            List<CountryAddRequest> countriesRequestList = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "Angola" },
                new CountryAddRequest() { CountryName = "Caboverde" },
                new CountryAddRequest() { CountryName = "Portugal" }
            };

            List<CountryResponse> countriesResponses = new List<CountryResponse>();
            foreach (CountryAddRequest request in countriesRequestList)
            {
                countriesResponses.Add(_countriesServices.AddCountry(request));
            }

            //Act
            CountryResponse? country = _countriesServices.GetCountryByCountryId(Guid.NewGuid());

            //Assert 
            Assert.Null(country);

        }

        [Fact]
        public void GetCountryByCountryId_Found()
        {
            //Arrange
            List<CountryAddRequest> countriesRequestList = new List<CountryAddRequest>()
            {
                new CountryAddRequest() { CountryName = "Angola" },
                new CountryAddRequest() { CountryName = "Caboverde" },
                new CountryAddRequest() { CountryName = "Portugal" }
            };

            List<CountryResponse> countriesResponses = new List<CountryResponse>();
            CountryResponse randomCountry = null;
            foreach (CountryAddRequest request in countriesRequestList)
            {
                CountryResponse response = _countriesServices.AddCountry(request);
                countriesResponses.Add(response);
                if (randomCountry == null)
                {
                    randomCountry = response;
                }
            }

            //Act
            CountryResponse? country = _countriesServices.GetCountryByCountryId(randomCountry.CountryId);

            //Assert 
            Assert.True(country.CountryId == randomCountry.CountryId);

        }
        #endregion

    }

}
