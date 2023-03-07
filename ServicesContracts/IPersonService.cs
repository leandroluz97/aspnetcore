using ServicesContracts.DTO;
using ServicesContracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts
{
    public interface IPersonService
    {
        PersonResponse AddPerson(PersonAddRequest personRequest);
        List<PersonResponse> GetAllPersons();
        PersonResponse GetPersonByPersonId(Guid? personId);
        List<PersonResponse> GetFilteredPersons(string searchBy, string? searchText);
        List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
        PersonResponse UpdatePerson(PersonUpdateRequest? personUpdateRequest);

    }
}
