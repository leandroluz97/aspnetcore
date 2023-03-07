using CRUDoperations.DTO;
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

namespace CRUDTests
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _outputHelper;

        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonService();
            _countriesService = new CountriesService();
            _outputHelper = testOutputHelper;
        }

        #region AddPerson
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
        #endregion

        #region GetPersonByPersonId
        
        [Fact]
        public void GetPersonByPersonId_FoundPerson()
        {
            //Arrange
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Portugal" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            PersonAddRequest request = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
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
        #endregion

        #region GetAllPersons
        [Fact]
        public void GetAllPersons_IsEmpty()
        {
            List<PersonResponse> persons = _personService.GetAllPersons();
            Assert.Empty(persons);
        }

        [Fact]
        public void GetAllPersons_AllPersons()
        {
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Portugal" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest1 = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonAddRequest personRequest2 = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            
            List<PersonAddRequest> personsList = new List<PersonAddRequest>(){personRequest1, personRequest2};
            List<PersonResponse> personsResponse = new List<PersonResponse>();
            foreach (PersonAddRequest person in personsList)
            {
                personsResponse.Add(_personService.AddPerson(person));
            }
            //Equivalent to Console.WriteLine()
            foreach (PersonResponse p in personsResponse)
            {
                _outputHelper.WriteLine(p.ToString());
            }
            List<PersonResponse> persons = _personService.GetAllPersons();
            foreach (PersonResponse person in persons)
            {
                Assert.Contains(person, personsResponse);
            }
            //Assert.NotEmpty(persons);
        }
        #endregion

        #region GetFilteredPersons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Portugal" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest1 = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonAddRequest personRequest2 = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personsList = new List<PersonAddRequest>() { personRequest1, personRequest2 };
            List<PersonResponse> personsResponse = new List<PersonResponse>();
            foreach (PersonAddRequest person in personsList)
            {
                personsResponse.Add(_personService.AddPerson(person));
            }
            //Equivalent to Console.WriteLine()
            foreach (PersonResponse p in personsResponse)
            {
                _outputHelper.WriteLine(p.ToString());
            }
            List<PersonResponse> persons = _personService.GetFilteredPersons(nameof(PersonAddRequest.PersonName), "");
            foreach (PersonResponse person in persons)
            {
                Assert.Contains(person, personsResponse);
            }
        }

        [Fact]
        public void GetFilteredPersons_FilteredSearchText()
        {
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Portugal" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest1 = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonAddRequest personRequest2 = new PersonAddRequest()
            {
                PersonName = "John Doey",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonAddRequest personRequest3 = new PersonAddRequest()
            {
                PersonName = "Marta Jones",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1990-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "Martajones@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personsList = new List<PersonAddRequest>() { personRequest1, personRequest2, personRequest3 };
            List<PersonResponse> personsResponse = new List<PersonResponse>();
            foreach (PersonAddRequest person in personsList)
            {
                personsResponse.Add(_personService.AddPerson(person));
            }
            //Equivalent to Console.WriteLine()
            foreach (PersonResponse p in personsResponse)
            {
                _outputHelper.WriteLine(p.ToString());
            }
            List<PersonResponse> persons = _personService.GetFilteredPersons(nameof(PersonAddRequest.PersonName), "oe");
            foreach (PersonResponse person in persons)
            {
                if (person.PersonName != null)
                {
                    if (person.PersonName.Contains("oe", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person, personsResponse);
                    }
                }
                
                
            }
        }
        #endregion

        #region GetSortedPersons
        //When sort based on PersonName in DESC
        [Fact]
        public void GetSortedPersons()
        {
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Portugal" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest1 = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonAddRequest personRequest2 = new PersonAddRequest()
            {
                PersonName = "John Doey",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };
            PersonAddRequest personRequest3 = new PersonAddRequest()
            {
                PersonName = "Marta Jones",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1990-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "Martajones@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> personsList = new List<PersonAddRequest>() { personRequest1, personRequest2, personRequest3 };
            List<PersonResponse> personsResponse = new List<PersonResponse>();
            foreach (PersonAddRequest person in personsList)
            {
                personsResponse.Add(_personService.AddPerson(person));
            }
            //Equivalent to Console.WriteLine()
            foreach (PersonResponse p in personsResponse)
            {
                _outputHelper.WriteLine(p.ToString());
            }
            List<PersonResponse> allPersons = _personService.GetAllPersons();
            List<PersonResponse> persons = _personService.GetSortedPersons(allPersons, nameof(PersonAddRequest.PersonName), SortOrderOptions.DESC);
            personsResponse = personsResponse.OrderByDescending(temp => temp.PersonName).ToList();

            for (int i = 0; i < personsResponse.Count; i++)
            {
                Assert.Equal(personsResponse[i], persons[i]);
            }
        }
        #endregion

        #region UpdatePerson
        [Fact]
        public void UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            //Arrange
            PersonUpdateRequest personUpdateRequest = new PersonUpdateRequest() { PersonId = Guid.Empty};

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonNameIsNull()
        {
            //Arrange
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Portugal" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            PersonResponse personResponse  = _personService.AddPerson(personRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = null;

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.UpdatePerson(personUpdateRequest);
            });
        }

        [Fact]
        public void UpdatePerson_PersonFullDetails()
        {
            //Arrange
            CountryAddRequest countryRequest = new CountryAddRequest() { CountryName = "Portugal" };
            CountryResponse countryResponse = _countriesService.AddCountry(countryRequest);
            PersonAddRequest personRequest = new PersonAddRequest()
            {
                PersonName = "John Doe",
                Address = "Lisbon, Portugal",
                DateOfBirth = DateTime.Parse("1980-12-02"),
                CountryId = countryResponse.CountryId,
                Email = "johndoe@gmail.com",
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = true
            };

            PersonResponse personResponse = _personService.AddPerson(personRequest);

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();
            personUpdateRequest.PersonName = "William";
            personUpdateRequest.Email = "William@gmail.com";
            PersonResponse personUpdateResponse = _personService.UpdatePerson(personUpdateRequest);

            PersonResponse personByIdResponse = _personService.GetPersonByPersonId(personUpdateResponse.PersonId);

            //Assert
            Assert.Equal(personByIdResponse, personUpdateResponse);
        }
        #endregion
    }
}
