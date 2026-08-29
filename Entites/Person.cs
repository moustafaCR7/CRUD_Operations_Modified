using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entites
{
    public class Person
    {
        [Key]
        public Guid PersonID { get; set; }

        [StringLength(30)]
        public string? PersonName { get; set; }

        [StringLength(50)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public string? Address { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; } = null!;


    }
}
