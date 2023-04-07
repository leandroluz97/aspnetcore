﻿using AutoFixture;
using CRUDoperations.DTO;
using Entities;
using EntityFrameworkCoreMock;
using Microsoft.EntityFrameworkCore;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XServices;
using Xunit;
using Xunit.Abstractions;
using FluentAssertions;
using RepositoryContracts;
using Moq;
using System.Linq.Expressions;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;

        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly IPersonsRepository _personsRepository;

        private readonly ITestOutputHelper _outputHelper;
        private readonly IFixture _fixture;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            var countriesInitalData = new List<Country>() { };
            var personsInitalData = new List<Person>() { };

            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            ApplicationDbContext dbContext = dbContextMock.Object;

            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitalData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitalData);

            _outputHelper = testOutputHelper;
            _fixture = new Fixture();
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;
            _countriesService = new CountriesService(null);
            _personService = new PersonService(_personsRepository);
        }

        #region AddPerson
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
        {
            //Arrange
            PersonAddRequest? request = null;

            //Act 
            Func<Task> action = async () =>
            {
                await _personService.AddPerson(request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
            //await Assert.ThrowsAsync<ArgumentNullException>
        }

        [Fact]
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange
            PersonAddRequest request = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.PersonName, string.Empty)
             .Create();

            //Act
            Func<Task> action = async () =>
            {
                await _personService.AddPerson(request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
            //Assert.ThrowsAsync<ArgumentException>
        }

        [Fact]
        public async Task AddPerson_FullPersonDetails_ToBeSuccessfull()
        {
            //Arrange
            PersonAddRequest request = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .Create();

            Person person = request.ToPerson();
            PersonResponse person_response_expected = person.ToPersonResponse();

            //IF we supply any argugment value to the AddPerson method, it should return the same return value
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>()))
                .ReturnsAsync(person);

            //Act
            PersonResponse personResponse = await _personService.AddPerson(request);
            person_response_expected.PersonId = personResponse.PersonId;
            //List<PersonResponse> persons = await _personService.GetAllPersons();

            //Assert
            //Assert.True(personResponse.PersonId != Guid.Empty);
            personResponse.PersonId.Should().NotBe(Guid.Empty);
            personResponse.Should().Be(person_response_expected);
            //Assert.Contains(personResponse, persons);
            //persons.Should().Contain(personResponse);
        }
        #endregion

        #region GetPersonByPersonId

        [Fact]
        public async Task GetPersonByPersonId_WithPersonId_Successfull()
        {
            //Arrange
            //CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
            //CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);
            Person person = _fixture.Build<Person>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .With(temp => temp.Country, null as Country)
             .Create();

            PersonResponse response = person.ToPersonResponse();
            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(person);

            //Act 
            //PersonResponse response = await _personService.AddPerson(request);
            PersonResponse foundPerson = await _personService.GetPersonByPersonId(person.PersonId);

            //Assert
            //Assert.Equal(person, response);
            foundPerson.Should().Be(response);

        }

        [Fact]
        public async Task GetPersonByPersonId_PersonIdIsNull_ToBeArgumentNullException()
        {
            Guid personId = Guid.Empty;

            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await _personService.GetPersonByPersonId(personId);
            });
        }

        [Fact]
        public async Task GetPersonByPersonId_RamdomPersonId_ToBeNull()
        {
            Guid personId = Guid.NewGuid();

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>()))
                .ReturnsAsync(default(Person)); //.ReturnsAsync(null as Person);

            PersonResponse person = await _personService.GetPersonByPersonId(personId);

            //Assert.Null(person);
            person.Should().BeNull();
        }
        #endregion

        #region GetAllPersons
        [Fact]
        public async Task GetAllPersons_ToBeEmpty()
        {
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(new List<Person>());
            List<PersonResponse> persons = await _personService.GetAllPersons();
            //Assert.Empty(persons);
            persons.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllPersons_AllPersons_ToBeSuccessfull()
        {
            PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .Create();
            PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "xjohndoe@gmail.com")
             .Create();

            Person person1 = personRequest1.ToPerson();
            Person person2 = personRequest2.ToPerson();
            List<Person> persons = new List<Person>() { person1, person2 };

            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);

            List<PersonResponse> allPersons = await _personService.GetAllPersons();

            var personsResponse = persons.Select(per => per.ToPersonResponse());
            personsResponse.Should().BeEquivalentTo(allPersons);
        }
        #endregion

        #region GetFilteredPersons
        [Fact]
        public async Task GetFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .Create();
            PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .Create();

            Person person1 = personRequest1.ToPerson();
            Person person2 = personRequest2.ToPerson();
            List<Person> personList = new List<Person>() { person1, person2 };

            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
            .ReturnsAsync(personList);

            List<PersonResponse> persons = await _personService.GetFilteredPersons(nameof(PersonAddRequest.PersonName), "");
            //List<PersonResponse> allPersons = personList.Select(temp => temp.ToPersonResponse()).ToList();

            persons.Should().OnlyContain(temp => temp.PersonName.Contains("", StringComparison.OrdinalIgnoreCase));
        }


        //public async Task GetFilteredPersons_FilteredSearchText()
        //{
        //    CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
        //    CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);
        //    PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
        //     .With(temp => temp.PersonName, "johndoe")
        //     .With(temp => temp.Email, "johndoe@gmail.com")
        //     .With(temp => temp.CountryId, countryResponse.CountryId)
        //     .Create();
        //    PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
        //     .With(temp => temp.PersonName, "johndoes")
        //     .With(temp => temp.Email, "johndoe@gmail.com")
        //     .With(temp => temp.CountryId, countryResponse.CountryId)
        //     .Create();
        //    PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
        //     .With(temp => temp.Email, "Martajones@gmail.com")
        //     .With(temp => temp.CountryId, countryResponse.CountryId)
        //     .Create();


        //    List<PersonAddRequest> personsList = new List<PersonAddRequest>() { personRequest1, personRequest2, personRequest3 };
        //    List<PersonResponse> personsResponse = new List<PersonResponse>();
        //    foreach (PersonAddRequest person in personsList)
        //    {
        //        personsResponse.Add(await _personService.AddPerson(person));
        //    }
        //Equivalent to Console.WriteLine()
        //foreach (PersonResponse p in personsResponse)
        //{
        //    _outputHelper.WriteLine(p.ToString());
        //}
        //List<PersonResponse> persons = await _personService.GetFilteredPersons(nameof(PersonAddRequest.PersonName), "oe");
        //foreach (PersonResponse person in persons)
        //{
        //    if (person.PersonName != null)
        //    {
        //        if (person.PersonName.Contains("oe", StringComparison.OrdinalIgnoreCase))
        //        {
        //            Assert.Contains(person, personsResponse);
        //        }
        //    }
        //}
        //persons.Should().OnlyContain(temp => temp.PersonName.Contains("oe", StringComparison.OrdinalIgnoreCase));
        //}
        [Fact]
        public async Task GetFilteredPersons_SearchText_ToBeSuccessful()
        {
    
            PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.PersonName, "johndoe")
             .With(temp => temp.Email, "johndoe@gmail.com")
             .Create();
            PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.PersonName, "johndoes")
             .With(temp => temp.Email, "johndoe@gmail.com")
             .Create();
            PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Martajones@gmail.com")
             .Create();

            Person person1 = personRequest1.ToPerson();
            Person person2 = personRequest2.ToPerson();
            Person person3 = personRequest3.ToPerson();
            List<Person> personList = new List<Person>() { person1, person2 };
            
            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(personList);
          
            List<PersonResponse> persons = await _personService.GetFilteredPersons(nameof(PersonAddRequest.PersonName), "oe");
         
            persons.Should().OnlyContain(temp => temp.PersonName.Contains("oe", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region GetSortedPersons
        //When sort based on PersonName in DESC
        [Fact]
        public async Task GetSortedPersons_ToBeSuccessful()
        {
            CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest1 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .With(temp => temp.CountryId, countryResponse.CountryId)
             .Create();
            PersonAddRequest personRequest2 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .With(temp => temp.CountryId, countryResponse.CountryId)
             .Create();
            PersonAddRequest personRequest3 = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "Martajones@gmail.com")
             .With(temp => temp.CountryId, countryResponse.CountryId)
             .Create();

            List<PersonAddRequest> personsList = new List<PersonAddRequest>() { personRequest1, personRequest2, personRequest3 };
            List<PersonResponse> personsResponse = new List<PersonResponse>();
            foreach (PersonAddRequest person in personsList)
            {
                personsResponse.Add(await _personService.AddPerson(person));
            }
            //Equivalent to Console.WriteLine()
            foreach (PersonResponse p in personsResponse)
            {
                _outputHelper.WriteLine(p.ToString());
            }
            List<PersonResponse> allPersons = await _personService.GetAllPersons();
            List<PersonResponse> persons = await _personService.GetSortedPersons(allPersons, nameof(PersonAddRequest.PersonName), SortOrderOptions.DESC);
            personsResponse = personsResponse.OrderByDescending(temp => temp.PersonName).ToList();

            //for (int i = 0; i < personsResponse.Count; i++)
            //{
            //    Assert.Equal(personsResponse[i], persons[i]);
            //}
            //persons.Should().BeEquivalentTo(personsResponse);
            persons.Should().BeInDescendingOrder(temp => temp.PersonName);
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = null;

            //Act
            Func<Task> action = async () =>
             {
                 await _personService.UpdatePerson(personUpdateRequest);
             };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task UpdatePerson_InvalidPersonId()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = _fixture.Build<PersonUpdateRequest>()
                 .With(temp => temp.Email, "johndoe@gmail.com")
                 .With(temp => temp.PersonId, Guid.Empty)
                 .Create();

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _personService.UpdatePerson(personUpdateRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .With(temp => temp.CountryId, countryResponse.CountryId)
             .Create();

            PersonResponse personResponse = await _personService.AddPerson(personRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //Act
            Func<Task> action = async () =>
            {
                await _personService.UpdatePerson(personUpdateRequest);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .With(temp => temp.CountryId, countryResponse.CountryId)
             .Create();

            PersonResponse personResponse = await _personService.AddPerson(personRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "William";
            personUpdateRequest.Email = "William@gmail.com";
            PersonResponse personUpdateResponse = await _personService.UpdatePerson(personUpdateRequest);

            PersonResponse personByIdResponse = await _personService.GetPersonByPersonId(personUpdateResponse.PersonId);

            //Assert
            //Assert.Equal(personByIdResponse, personUpdateResponse);
            personByIdResponse.Should().Be(personUpdateResponse);
        }
        #endregion

        #region deletePerson
        [Fact]
        public async Task DeletePerson_ValidPersonID()
        {
            //Arrange
            CountryAddRequest countryRequest = _fixture.Create<CountryAddRequest>();
            CountryResponse countryResponse = await _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest = _fixture.Build<PersonAddRequest>()
             .With(temp => temp.Email, "johndoe@gmail.com")
             .With(temp => temp.CountryId, countryResponse.CountryId)
             .Create();

            PersonResponse personResponse = await _personService.AddPerson(personRequest);

            //Act
            bool isDeleted = await _personService.DeletePerson(personResponse.PersonId);

            //Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeletePerson_InvalidPersonID()
        {
            //Act
            bool isDeleted = await _personService.DeletePerson(Guid.NewGuid());

            //Assert
            //Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }
        #endregion
    }
}
