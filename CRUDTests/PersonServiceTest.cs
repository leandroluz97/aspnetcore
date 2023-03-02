﻿using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XServices;
using Xunit;

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;

        public PersonServiceTest()
        {
            _personService = new PersonService();
        }

        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? request = null;
        
            //Assert
            Assert.Throws<ArgumentNullException>(() => {
                //Act 
                _personService.AddPerson(request);
            });
        }

        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest request = new PersonAddRequest() { PersonName = null};

            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act 
                _personService.AddPerson(request);
            });
        }

        [Fact]
        public void AddPerson_ValidPerson()
        {
            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = Guid.NewGuid(),
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            //Act
            PersonResponse personResponse = _personService.AddPerson(request);
            List<PersonResponse> persons = _personService.GetAllPersons();

            //Assert
            Assert.True(personResponse.PersonId != Guid.Empty);
            Assert.Contains(personResponse, persons);
        }

        [Fact]
        public void GetPersonByPersonId_FoundPerson()
        {
            //Arrange
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = Guid.NewGuid(),
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            //Act 
            PersonResponse response = _personService.AddPerson(request);
            PersonResponse person = _personService.GetPersonByPersonId(response.PersonId);

            //Assert
            Assert.True(person.PersonId != Guid.Empty);
          
        }

        [Fact]
        public void GetPersonByPersonId_PersonIdIsNull()
        {
            Guid personId = Guid.Empty;

            Assert.Throws<ArgumentNullException>(() => {
                _personService.GetPersonByPersonId(personId);
            });
        }

        [Fact]
        public void GetPersonByPersonId_NotFound()
        {
            Guid personId = Guid.NewGuid();

            PersonResponse person = _personService.GetPersonByPersonId(personId);

            Assert.Null(person);
        }


    }
}
