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

        public PersonService()
        {
             _persons = new List<Person>();
             _countriesService = new CountriesService();
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
            return _persons.Select(person => person.ToPersonResponse()).ToList();
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

            return person.ToPersonResponse();
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
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.PersonName) || person.PersonName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Email) ||  person.Email.Contains(searchText)).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(person => person.DateOfBirth == null || person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.CountryId):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Country) || person.Country.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Gender)  ||  person.Gender.Contains(searchText, StringComparison.OrdinalIgnoreCase) ).ToList();
                    break;
                case nameof(Person.Address):
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
            throw new NotImplementedException();
        }
    }
}
