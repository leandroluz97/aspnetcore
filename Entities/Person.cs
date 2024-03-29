﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Person
    {
        [Key]
        public Guid PersonId { get; set; }

        [StringLength(40)] //nvarchar(40)
        public string? PersonName { get; set; }

        [StringLength(40)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth{ get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }
        public Guid? CountryId{ get; set; }

        [StringLength(200)]
        public string? Address{ get; set; }
        public bool ReceiveNewsLetters { get; set; }
        public string? TIN { get; set; }

        public Country? Country { get; set; }

        public override string ToString()
        {
            return $"PersonId:{PersonId}, Name:{PersonName}, Email:{Email}, Gender:{Gender}, Address:{Address}, ReceiveNewsLetters:{ReceiveNewsLetters}";
        }

    }
}
