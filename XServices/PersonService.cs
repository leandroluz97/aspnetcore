using CsvHelper;
using Entities;
using Microsoft.EntityFrameworkCore;
using ServicesContracts;
using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XServices.Helpers;

namespace XServices
{
    public class PersonService : IPersonService
    {
        readonly private PersonsDbContext _db;
        readonly private ICountriesService _countriesService;

        public PersonService(PersonsDbContext personsDbContext, ICountriesService countriesService)
        {
            _db = personsDbContext;
            _countriesService = countriesService;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest personRequest)
        {
            if (personRequest == null)
            {
                throw new ArgumentNullException(nameof(personRequest));
            }
            if (string.IsNullOrEmpty(personRequest.PersonName))
            {
                throw new ArgumentException("PersonName can't be empty");
            }

            ValidationHelper.ModelValidation(personRequest);

            Person person = personRequest.ToPerson();
            person.PersonId = Guid.NewGuid();

            //await _db.sp_InsertPerson(person);
            _db.Persons.Add(person);
            await _db.SaveChangesAsync();

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
            //return _db.Persons.ToList().Select(person => ConvertPersonToPersonResponse(person)).ToList();
            var persons = await _db.Persons.Include(p => p.Country).ToListAsync();
            return persons.Select(person => person.ToPersonResponse()).ToList();
            //return _db.sp_GetAllPersons().Select(person => person.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse> GetPersonByPersonId(Guid? personId)
        {
            if (personId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = await _db.Persons.Include(p => p.Country).FirstOrDefaultAsync(person => person.PersonId.Equals(personId));

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchText)
        {
            List<PersonResponse> allPersons = await GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchText))
            {
                return matchingPersons;
            }

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.PersonName) || person.PersonName.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Email) || person.Email.Contains(searchText)).ToList();
                    break;
                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(person => person.DateOfBirth == null || person.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.CountryId):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Country) || person.Country.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
                    break;
                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(person => string.IsNullOrEmpty(person.Gender) || person.Gender.Contains(searchText, StringComparison.OrdinalIgnoreCase)).ToList();
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

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder = SortOrderOptions.ASC)
        {
            if (string.IsNullOrEmpty(sortBy))
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
                _ => allPersons

            };
            return sortedPersons;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //Get matching person object to update 
            Person? matchingPerson = await _db.Persons.FirstOrDefaultAsync(person => person.PersonId == personUpdateRequest.PersonId);
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
            await _db.SaveChangesAsync();

            return matchingPerson.ToPersonResponse();


        }

        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = _db.Persons.FirstOrDefault(temp => temp.PersonId == personId);
            if (person == null)
            {
                return false;
            }
            _db.Persons.Remove(_db.Persons.First(temp => temp.PersonId == personId));
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);
            CsvWriter csvWriter= new CsvWriter(streamWriter, CultureInfo.InvariantCulture, leaveOpen:true);

            csvWriter.WriteHeader<PersonResponse>(); //PersonId, PersonName...
            csvWriter.NextRecord();

            var persons = await _db.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToListAsync();

            await csvWriter.WriteRecordsAsync(persons);

            memoryStream.Position = 0;
            return memoryStream;    

        }
    }
}
