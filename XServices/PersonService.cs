using Entities;
using ServicesContracts;
using ServicesContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Person person = personRequest.ToPerson();
            person.PersonId = Guid.NewGuid();
            _persons.Add(person);
            
            return ConvertPersonToPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            return _persons.Select(person => person.ToPersonResponse()).ToList();
        }
    }
}
