﻿using ServicesContracts.DTO;
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

    }
}
