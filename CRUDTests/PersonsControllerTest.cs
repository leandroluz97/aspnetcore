using AutoFixture;
using Moq;
using ServicesContracts;
using ServicesContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUDoperations.Controllers;
using Xunit;
using ServicesContracts.Enums;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using CRUDoperations.DTO;
using Microsoft.Extensions.Logging;

namespace CRUDTests
{
    public class PersonsControllerTest
    {
        private readonly IPersonService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly ILogger<PersonController> _logger;

        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Mock<IPersonService> _personsServiceMock;
        private readonly Mock<ILogger<PersonController>> _loggerMock;

        private readonly Fixture _fixture;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();

            _countriesServiceMock = new Mock<ICountriesService>();
            _personsServiceMock = new Mock<IPersonService>();
            _loggerMock = new Mock<ILogger<PersonController>>();

            _countriesService = _countriesServiceMock.Object;
            _personsService = _personsServiceMock.Object;
            _logger = _loggerMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonsList()
        {
            // Arrange
            List<PersonResponse> persons_response_list = _fixture.Create<List<PersonResponse>>();

            PersonController personController = new PersonController(_personsService, _countriesService, _logger);

            _personsServiceMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(persons_response_list);

            _personsServiceMock.Setup(temp => temp.GetSortedPersons(It.IsAny<List<PersonResponse>>(), It.IsAny<string>(), It.IsAny<SortOrderOptions>()))
                .ReturnsAsync(persons_response_list);

            //act 
            IActionResult result  = await personController.Index(
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<string>(),
                _fixture.Create<SortOrderOptions>());

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);

            viewResult.ViewData.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
        }
        #endregion

        #region Create
        [Fact]
        public async Task Create_ModelErrors_ToReturnCreateView()
        {
            // Arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();
            
            _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);

            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(person_response);

            PersonController personController = new PersonController(_personsService, _countriesService, _logger);

            //act 
            personController.ModelState.AddModelError("PersonName", "Person Name can't be blank");

            IActionResult result = await personController.Create(person_add_request);

            //Assert
            ViewResult viewResult = Assert.IsType<ViewResult>(result);
             
            viewResult.ViewData.Model.Should().BeAssignableTo<PersonAddRequest>();
            viewResult.ViewData.Model.Should().Be(person_add_request);
        }

        [Fact]
        public async Task Create_NoModelErrors_ToReturnRedirectToIndexView()
        {
            // Arrange
            PersonAddRequest person_add_request = _fixture.Create<PersonAddRequest>();

            PersonResponse person_response = _fixture.Create<PersonResponse>();

            List<CountryResponse> countries = _fixture.Create<List<CountryResponse>>();

            _countriesServiceMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countries);

            _personsServiceMock.Setup(temp => temp.AddPerson(It.IsAny<PersonAddRequest>()))
                .ReturnsAsync(person_response);

            PersonController personController = new PersonController(_personsService, _countriesService, _logger);

            //act 
            IActionResult result = await personController.Create(person_add_request);

            //Assert
            RedirectToActionResult redirectResult = Assert.IsType<RedirectToActionResult>(result);

            redirectResult.ActionName.Should().Be("Index");
        }
        #endregion
    }

}
