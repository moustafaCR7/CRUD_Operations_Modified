using Entites;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helper;

namespace Services
{
    public class PersonService : IPersonService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICountryService _countryService;

        public  PersonService(ApplicationDbContext context , ICountryService country)
        {
            _context = context;
            _countryService = country;
        }
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            if(personAddRequest == null) throw new ArgumentNullException(nameof(personAddRequest));

            ValidationHelper.ValidatorClass(personAddRequest);

            Person person = personAddRequest.ToPerson();

            person.PersonID = Guid.NewGuid();

            _context.Persons.Add(person);
            _context.SaveChanges();

            return  person.ToPersonResponse();

        }

        public async Task<List<PersonResponse>> GetAllPersons()
        {
           return _context.Persons.ToList().Select(x=>x.ToPersonResponse()).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if(personID == null) return null;
            Person? person = _context.Persons.FirstOrDefault(temp=> temp.PersonID == personID);
            if (person == null) return null;
            return person.ToPersonResponse();
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if(personUpdateRequest==null) throw new ArgumentNullException(nameof(personUpdateRequest));

            ValidationHelper.ValidatorClass(personUpdateRequest);

            Person? person = await _context.Persons.FirstOrDefaultAsync(x => x.PersonID == personUpdateRequest.PersonID);
            if(person==null)
                throw new ArgumentException(nameof(person));

            person.PersonID = personUpdateRequest.PersonID;
            person.PersonName = personUpdateRequest.PersonName;
            person.Email = personUpdateRequest.Email;
            person.Gender = personUpdateRequest.Gender.ToString();
            person.Address = personUpdateRequest.Address;
            person.DateOfBirth = personUpdateRequest.DateOfBirth;
            person.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            person.CountryID = personUpdateRequest.CountryID;

            await _context.SaveChangesAsync();

            return person.ToPersonResponse();

        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID==null) throw new ArgumentNullException(nameof(personID));
            Person ? person =await _context.Persons.FirstOrDefaultAsync(x=>x.PersonID == personID);
            if (person == null) return false;
            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
         List<PersonResponse> AllPersons = await GetAllPersons();
         List<PersonResponse> FilterdPersons = AllPersons;


            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return FilterdPersons;

            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    FilterdPersons = AllPersons.Where(x => x.PersonName != null ? x.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.Email):
                    FilterdPersons = AllPersons.Where(x => x.Email != null ? x.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.Address):
                    FilterdPersons = AllPersons.Where(x => x.Address != null ? x.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.CountryID):
                    FilterdPersons = AllPersons.Where(x => x.Country != null ? x.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.Gender):
                    FilterdPersons = AllPersons.Where(x => x.Gender != null ? x.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                case nameof(Person.DateOfBirth):
                    FilterdPersons = AllPersons.Where(x => x.DateOfBirth != null ? x.DateOfBirth.Value.ToString("yyyy MM dd").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;
                default:
                    return AllPersons;
            }
            return AllPersons;
        }

        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrder sortOrder)
        {

            if (sortBy == null) return allPersons;

            List<PersonResponse> SortedPersons = (sortBy, sortOrder) switch
            {
                (nameof(Person.PersonName), SortOrder.ASC) =>
                SortedPersons = allPersons.OrderBy(x => x.PersonName).ToList(),
                (nameof(Person.PersonName), SortOrder.DESC) =>
                SortedPersons = allPersons.OrderByDescending(x => x.PersonName).ToList(),

                (nameof(Person.Email), SortOrder.ASC) =>
                SortedPersons = allPersons.OrderBy(x => x.Email).ToList(),
                (nameof(Person.Email), SortOrder.DESC) =>
                SortedPersons = allPersons.OrderByDescending(x => x.Email).ToList(),

                (nameof(Person.Address), SortOrder.ASC) =>
                SortedPersons = allPersons.OrderBy(x => x.Address).ToList(),
                (nameof(Person.Address), SortOrder.DESC) =>
                SortedPersons = allPersons.OrderByDescending(x => x.Address).ToList(),

                (nameof(Person.DateOfBirth), SortOrder.ASC) =>
                SortedPersons = allPersons.OrderBy(x => x.DateOfBirth).ToList(),
                (nameof(Person.DateOfBirth), SortOrder.DESC) =>
                SortedPersons = allPersons.OrderByDescending(x => x.DateOfBirth).ToList(),

                _ => allPersons

            };
            return SortedPersons;
        }

     
    }
}
