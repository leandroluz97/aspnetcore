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
        Task<PersonResponse> AddPerson(PersonAddRequest personRequest);
        Task<List<PersonResponse>> GetAllPersons();
        Task<PersonResponse> GetPersonByPersonId(Guid? personId);
        Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchText);
        Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder);
        Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
        Task<bool> DeletePerson(Guid? personId);
            

    }
}
