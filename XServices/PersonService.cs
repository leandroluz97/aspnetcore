using Entities;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XServices.Helpers;

namespace XServices
{
    public class PersonService : IPersonService
    {
        readonly private List<Person> _persons;
        readonly private ICountriesService _countriesService;

        public PersonService(bool initialize = true)
        {
             _persons = new List<Person>();
             _countriesService = new CountriesService();
            if (initialize)
            {
  
                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("C38959D3-6440-462A-A5FA-590667638BBA"),
                    CountryId = Guid.Parse("515F75D9-F590-4645-A452-9346FE774466"),
                    PersonName = "Currie", 
                    Email = "chorlick0@china.com.cn", 
                    DateOfBirth = DateTime.Parse("1991-02-24"),
                    Gender = GenderOptions.Male.ToString(), 
                    Address = "12 Bartillon Way", 
                    ReceiveNewsLetters = true,
                });
                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("C38959D3-6440-462A-A5FA-590667638BBA"),
                    CountryId = Guid.Parse("C5B6D48E-1013-49FA-A1EA-0DF7F8943DD2"),
                    PersonName = "Currie",
                    Email = "chorlick0@china.com.cn",
                    DateOfBirth = DateTime.Parse("1991-02-24"),
                    Gender = GenderOptions.Male.ToString(),
                    Address = "12 Bartillon Way",
                    ReceiveNewsLetters = true,
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("CE28E2F2-6991-476C-927D-FC96A7353D70"),
                    CountryId = Guid.Parse("8D90E67E-300F-4341-BFE4-41654F1792E5"),
                    PersonName = "Mireielle",
                    Email = "mreader1@jalbum.net",
                    DateOfBirth = DateTime.Parse("2018-01-06"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "32032 Schlimgen Alley",
                    ReceiveNewsLetters = false,
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("8B186BCB-6404-4998-BE0F-8AE7AB8D840D"),
                    CountryId = Guid.Parse("E4B742FC-AF91-4634-9EA8-5F92E03F8AFE"),
                    PersonName = "Carlene",
                    Email = "cpasley2@printfriendly.com",
                    DateOfBirth = DateTime.Parse("1993-10-23"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "51 Green Circle",
                    ReceiveNewsLetters = true,
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("2FCDB7F6-36BC-4D86-AA38-4EB9E2293518"),
                    CountryId = Guid.Parse("A283BB1D-CB1D-4CAC-A7D0-E9B4E0721851"),
                    PersonName = "Christabella",
                    Email = "csmithend3@cmu.edu",
                    DateOfBirth = DateTime.Parse("1994-02-26"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "46 Calypso Pass",
                    ReceiveNewsLetters = false,
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("D3CFB393-8D7C-497B-918C-2E2DCBA5D893"),
                    CountryId = Guid.Parse("A283BB1D-CB1D-4CAC-A7D0-E9B4E0721851"),
                    PersonName = "Ariel",
                    Email = "asawle4@about.com",
                    DateOfBirth = DateTime.Parse("2001-03-03"),
                    Gender = GenderOptions.Male.ToString(),
                    Address = "2873 Sachtjen Junction",
                    ReceiveNewsLetters = false,
                });

                _persons.Add(new Person()
                {
                    PersonId = Guid.Parse("3824DD2D-0D71-4A56-ADE7-830AACCAF0DF"),
                    CountryId = Guid.Parse("A283BB1D-CB1D-4CAC-A7D0-E9B4E0721851"),
                    PersonName = "Christine",
                    Email = "cfitter5@wufoo.com",
                    DateOfBirth = DateTime.Parse("2011-11-09"),
                    Gender = GenderOptions.Female.ToString(),
                    Address = "5 American Road",
                    ReceiveNewsLetters = false,
                });


            }

        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryId(personResponse.CountryId)?.CountryName;
            return personResponse;
        }
        public PersonResponse AddPerson(PersonAddRequest personRequest)
        {
           if(personRequest == null)
            {
                throw new ArgumentNullException(nameof(personRequest));
            }
           if(string.IsNullOrEmpty(personRequest.PersonName))
            {
                throw new ArgumentException("PersonName can't be empty");
            }

            ValidationHelper.ModelValidation(personRequest);

            Person person = personRequest.ToPerson();
            person.PersonId = Guid.NewGuid();

            _persons.Add(person);
            
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
        }

        public PersonResponse GetPersonByPersonId(Guid? personId)
        {
            if(personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = _persons.FirstOrDefault(person => person.PersonId.Equals(personId));

            if(person == null)
            {
                return null;
            }

            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchText)
        {
            List<PersonResponse> allPersons = GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if(string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchText))
            {
                return matchingPersons;
            }

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.PersonName) || person.PersonName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;        
                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Email) ||  person.Email.Contains(searchText)).ToList();
                    break;        
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(person => person.DateOfBirth == null || person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;        
                case nameof(PersonResponse.CountryId):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Country) || person.Country.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;        
                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Gender)  ||  person.Gender.Contains(searchText, StringComparison.OrdinalIgnoreCase) ).ToList();
                    break;        
                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Address) || person.Address.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                default:
                    matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;

        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            if(string.IsNullOrEmpty(sortBy))
                return allPersons;

            List<PersonResponse> sortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.DateOfBirth).ToList(),
                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.DateOfBirth).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Age).ToList(),
                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Age).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(person => person.ReceiveNewsLetters).ToList(),
                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(person => person.ReceiveNewsLetters).ToList(),
                _=> allPersons

            };
            return sortedPersons;
        }

        public PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person ));
            }
                
            //Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //Get matching person object to update 
            Person? matchingPerson = _persons.FirstOrDefault(person => person.PersonId == personUpdateRequest.PersonId);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }

            //Update all details
            matchingPerson.PersonId = personUpdateRequest.PersonId;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Email = personUpdateRequest.Email;

            return matchingPerson.ToPersonResponse();


        }

        public bool DeletePerson(Guid? personId)
        {
            if(personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }
            
            Person? person = _persons.FirstOrDefault(temp => temp.PersonId == personId);
            if(person == null)
            {
                return false;
            }
            _persons.Remove(person);
            return true;
        }
    }
}
