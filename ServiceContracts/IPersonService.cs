using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceContracts
{
    public interface IPersonService
    {
       Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest);
       Task<PersonResponse?> GetPersonByPersonID(Guid? personID);
       Task<List<PersonResponse>> GetAllPersons();
       Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest);
       Task<bool> DeletePerson(Guid? personID);
       Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);
       Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons,
        string sortBy, SortOrder sortOrder);



    }
}
