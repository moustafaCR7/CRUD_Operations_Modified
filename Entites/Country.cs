using System.ComponentModel.DataAnnotations;

namespace Entites
{
    public class Country
    {
        [Key]
        public Guid CountryId { get; set; }
        public string? CountryName { get; set; }

        public  ICollection<Person> Persons = new List<Person>();
    }
}
