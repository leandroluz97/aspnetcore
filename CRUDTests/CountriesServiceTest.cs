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
using EntityFrameworkCoreMock;
using Moq;

namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesServices;
        public CountriesServiceTest()
        {
            var countriesInitalData = new List<Country>() { };
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitalData);
            _countriesServices = new CountriesService(dbContext);
        }

        #region AddCountry
        [Fact]
        public async Task AddCountry_ValidCountryDetails()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Leandro" };

            //Act
            var actual = await _countriesServices.AddCountry(request);
            List<CountryResponse> countries = await _countriesServices.GetAllCountries();

            //Assert
            //JsonConvert.SerializeObject(response);
            Assert.True(actual.CountryId != Guid.Empty);
            Assert.Contains(actual, countries);
        }

        [Fact]
        public async Task AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
           await Assert.ThrowsAsync<ArgumentNullException>(async() => {
                //Act
                await _countriesServices.AddCountry(request);
            });
        }

        [Fact]
        public async Task AddCountry_NameIsNull()
        {
            //Arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = null };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async() => {
                //Act
                await _countriesServices.AddCountry(request);
            });
        }

        [Fact]
        public  void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest request1 = new CountryAddRequest() { CountryName = "Angola" };
            CountryAddRequest request2 = new CountryAddRequest() { CountryName = "Angola" };

            //Assert
            Assert.ThrowsAsync<ArgumentException>(async () => {
                //Act
                await _countriesServices.AddCountry(request1);
                await _countriesServices.AddCountry(request2);
            });
        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task GetAllCountries_Countries()
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
                countriesResponses.Add(await _countriesServices.AddCountry(request));
            }

            //Act
            List<CountryResponse> countries = await _countriesServices.GetAllCountries();

            foreach (CountryResponse response in countriesResponses)
            {
                //Assert
                Assert.Contains(response, countries);
            }
        }

        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            //Act
            List<CountryResponse> countries = await _countriesServices.GetAllCountries();
            //Assert
            Assert.True(0 == countries.Count);
        }
        #endregion

        #region GetCountryByCountryId
        [Fact]
        public async Task GetCountryByCountryId_GuidIsNull()
        {
            //Arrange
            Guid? guid = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _countriesServices.GetCountryByCountryId(guid);
            });
        }

        [Fact]
        public async Task GetCountryByCountryId_GuidIsInvalid()
        {
            //Arrange
            Guid guid = Guid.Empty;

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _countriesServices.GetCountryByCountryId(guid);
            });
        }

        [Fact]
        public async Task GetCountryByCountryId_Notfound()
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
                countriesResponses.Add(await _countriesServices.AddCountry(request));
            }

            //Act
            CountryResponse? country = await _countriesServices.GetCountryByCountryId(Guid.NewGuid());

            //Assert 
            Assert.Null(country);

        }

        [Fact]
        public async Task GetCountryByCountryId_Found()
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
                CountryResponse response = await _countriesServices.AddCountry(request);
                countriesResponses.Add(response);
                if (randomCountry == null)
                {
                    randomCountry = response;
                }
            }

            //Act
            CountryResponse? country = await _countriesServices.GetCountryByCountryId(randomCountry.CountryId);

            //Assert 
            Assert.True(country.CountryId == randomCountry.CountryId);

        }
        #endregion

    }

}
