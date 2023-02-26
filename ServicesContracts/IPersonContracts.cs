using ServicesContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesContracts
{
    public interface IPersonContracts
    {
        PersonResponse AddPerson(PersonAddRequest personRequest);
        List<PersonResponse> GetAllPersons();

    }
}
