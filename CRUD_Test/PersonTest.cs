using Entites;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace CRUD_Test
{
    public class PersonTest
    {
        private readonly ICountryService _countryService;
        private readonly IPersonService _personServices;

        public PersonTest()
        {
            _countryService = new CountryService();
            _personServices = new PersonService();
        }

        #region AddPerson
        [Fact]
        public void AddPerson_NullAddPersonRequest()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;
            //Assert
            Assert.Throws<ArgumentNullException>(
                //Act
                () => _personServices.AddPerson(personAddRequest));
        }
        [Fact]
        public void AddPerson_NullPersonName()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest { PersonName = null };
            //Assert
            Assert.Throws<ArgumentException>(
                //Act
                () => _personServices.AddPerson(personAddRequest));
        }

        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
           

            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = "Person name...", Email = "person@example.com", Address = "sample address", CountryID = Guid.NewGuid(), Gender = GenderOptions.Male, DateOfBirth = DateTime.Parse("2000-01-01"), ReceiveNewsLetters = true };

            //Act
            PersonResponse person_response_from_add = _personServices.AddPerson(personAddRequest);

            List<PersonResponse> persons_list = _personServices.GetAllPersons();

            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);

            Assert.Contains(person_response_from_add, persons_list);

        }
        #endregion
        #region GetPersonByPersonId
        [Fact]
        public void GetPersonByPersonId_NullId()
        {
            Guid? id = null;

            PersonResponse? person = _personServices.GetPersonByPersonID(id);

            Assert.Null(person);
        }

        [Fact]
        public void GetPersonByPersonId_ValidId()
        {
              CountryAddRequest country = new CountryAddRequest { CountryName = "India" };
              CountryResponse countryResponse = _countryService.AddCountry(country);
            PersonAddRequest personAddRequest = new PersonAddRequest
            {
                PersonName = "John Doe",
                CountryID = countryResponse.CountryId,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "mosi",
                ReceiveNewsLetters = false,
                Address = "Noida",
                Gender = GenderOptions.Male
            };
            PersonResponse persom_from_add = _personServices.AddPerson(personAddRequest);
            PersonResponse? person_from_get = _personServices.GetPersonByPersonID(persom_from_add.PersonID);

            Assert.Equal(persom_from_add, person_from_get);
        }
        #endregion
        #region GetAllPersons
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            List<PersonResponse> persons = _personServices.GetAllPersons();
            Assert.Empty(persons);
        }

        [Fact]
        public void GetAllPersons_AddPersons()
        {
            CountryAddRequest country1 = new CountryAddRequest { CountryName = "India" };
            CountryAddRequest country2 = new CountryAddRequest { CountryName = "Egypt" };

            CountryResponse countryResponse1 = _countryService.AddCountry(country1);
            CountryResponse countryResponse2 = _countryService.AddCountry(country2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest
            {
                PersonName = "John Doe",
                CountryID = countryResponse1.CountryId,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "mosi",
                ReceiveNewsLetters = false,
                Address = "Noida",
                Gender = GenderOptions.Male
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest
            {
                PersonName = "moustafa",
                CountryID = countryResponse2.CountryId,
                DateOfBirth = new DateTime(1997, 11, 1),
                Email = "jfkgjk",
                ReceiveNewsLetters = true,
                Address = "mansoura",
                Gender = GenderOptions.Female
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest
            {
                PersonName = "Esraa",
                CountryID = countryResponse1.CountryId,
                DateOfBirth = new DateTime(2003, 9, 24),
                Email = "esraaelsayed34@gmail.com",
                ReceiveNewsLetters = false,
                Address = "share3 3ashra",
                Gender = GenderOptions.Engineer
            };
            List<PersonAddRequest> personAddRequest = new List<PersonAddRequest> { personAddRequest1, personAddRequest2, personAddRequest3 };
            List<PersonResponse> personResponses = new List<PersonResponse>();
            foreach(PersonAddRequest person in personAddRequest)
            {
                PersonResponse response = _personServices.AddPerson(person);
                personResponses.Add(response);
            }

            List<PersonResponse> PersonsFromGet = _personServices.GetAllPersons();

            foreach(PersonResponse person1 in personResponses)
            {
                Assert.Contains(person1, PersonsFromGet);
            }
        }
        #endregion
        #region UpdatePerson
        [Fact]
        public void UpdatePerson_NullPersonUpdateRequest()
        {
            PersonUpdateRequest? person = null;

            Assert.Throws<ArgumentNullException>(
                 () => _personServices.UpdatePerson(person)
                );         
        }

        [Fact]
        public void UpdatePerson_InvalidPersonId()
        {
            PersonUpdateRequest? person = new PersonUpdateRequest { PersonID=Guid.NewGuid()};
            Assert.Throws<ArgumentException>(
                 () => _personServices.UpdatePerson(person)
                );
        }

        [Fact]
        public void UpdatePerson_InvalidPersonName()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countryService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryID = country_response_from_add.CountryId,Email="cmxocoxc",Gender=GenderOptions.Male };
            PersonResponse person_response_from_add = _personServices.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = null;


            //Assert
            Assert.Throws<ArgumentException>(() => {
                //Act
                _personServices.UpdatePerson(person_update_request);
            });
        }
        [Fact]
        public void UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "UK" };
            CountryResponse country_response_from_add = _countryService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "John", CountryID = country_response_from_add.CountryId, Address = "Abc road", DateOfBirth = DateTime.Parse("2000-01-01"), Email = "abc@example.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            PersonResponse person_response_from_add = _personServices.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();
            person_update_request.PersonName = "William";
            person_update_request.Email = "william@example.com";

            //Act
            PersonResponse person_response_from_update = _personServices.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = _personServices.GetPersonByPersonID(person_response_from_update.PersonID);

            //Assert
            Assert.Equal(person_response_from_get, person_response_from_update);

        }
        #endregion
        #region DeletePerson

        [Fact]
        public void DeletePerson_ValidDelete()
        {
            CountryAddRequest country_add_request = new CountryAddRequest() { CountryName = "USA" };
            CountryResponse country_response_from_add = _countryService.AddCountry(country_add_request);

            PersonAddRequest person_add_request = new PersonAddRequest() { PersonName = "Jones", Address = "address", CountryID = country_response_from_add.CountryId, DateOfBirth = Convert.ToDateTime("2010-01-01"), Email = "jones@example.com", Gender = GenderOptions.Male, ReceiveNewsLetters = true };

            PersonResponse person_response_from_add = _personServices.AddPerson(person_add_request);


            //Act
            bool isDeleted = _personServices.DeletePerson(person_response_from_add.PersonID);

            //Assert
            Assert.True(isDeleted);
        }

        [Fact]
        public void DeletePerson_InvalidDelete()
        {
            Guid id = Guid.NewGuid();
            bool isDeleted = _personServices.DeletePerson(id);
            Assert.False(isDeleted);
        }
        #endregion

        #region GetFilteredPersons

        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            CountryAddRequest country1 = new CountryAddRequest { CountryName = "India" };
            CountryAddRequest country2 = new CountryAddRequest { CountryName = "Egypt" };

            CountryResponse countryResponse1 = _countryService.AddCountry(country1);
            CountryResponse countryResponse2 = _countryService.AddCountry(country2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest
            {
                PersonName = "John Doe",
                CountryID = countryResponse1.CountryId,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "mosi",
                ReceiveNewsLetters = false,
                Address = "Noida",
                Gender = GenderOptions.Male
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest
            {
                PersonName = "moustafa",
                CountryID = countryResponse2.CountryId,
                DateOfBirth = new DateTime(1997, 11, 1),
                Email = "jfkgjk",
                ReceiveNewsLetters = true,
                Address = "mansoura",
                Gender = GenderOptions.Female
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest
            {
                PersonName = "Esraa",
                CountryID = countryResponse1.CountryId,
                DateOfBirth = new DateTime(2003, 9, 24),
                Email = "esraaelsayed34@gmail.com",
                ReceiveNewsLetters = false,
                Address = "share3 3ashra",
                Gender = GenderOptions.Engineer
            };
            List<PersonAddRequest> personAddRequest = new List<PersonAddRequest> { personAddRequest1, personAddRequest2, personAddRequest3 };
            List<PersonResponse> personResponses = new List<PersonResponse>();
            foreach (PersonAddRequest person in personAddRequest)
            {
                PersonResponse response = _personServices.AddPerson(person);
                personResponses.Add(response);
            }

            List<PersonResponse> PersonsFromGet = _personServices.GetFilteredPersons(nameof(Person.PersonName),"");

            foreach (PersonResponse person1 in personResponses)
            {
                Assert.Contains(person1, PersonsFromGet);
            }
        }
        [Fact]
        public void GetFilteredPersons_ContentInSearchText()
        {
            CountryAddRequest country1 = new CountryAddRequest { CountryName = "India" };
            CountryAddRequest country2 = new CountryAddRequest { CountryName = "Egypt" };

            CountryResponse countryResponse1 = _countryService.AddCountry(country1);
            CountryResponse countryResponse2 = _countryService.AddCountry(country2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest
            {
                PersonName = "John Doe",
                CountryID = countryResponse1.CountryId,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "mosi",
                ReceiveNewsLetters = false,
                Address = "Noida",
                Gender = GenderOptions.Male
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest
            {
                PersonName = "moustafa",
                CountryID = countryResponse2.CountryId,
                DateOfBirth = new DateTime(1997, 11, 1),
                Email = "jfkgjk",
                ReceiveNewsLetters = true,
                Address = "mansoura",
                Gender = GenderOptions.Female
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest
            {
                PersonName = "Esraa",
                CountryID = countryResponse1.CountryId,
                DateOfBirth = new DateTime(2003, 9, 24),
                Email = "esraaelsayed34@gmail.com",
                ReceiveNewsLetters = false,
                Address = "share3 3ashra",
                Gender = GenderOptions.Engineer
            };
            List<PersonAddRequest> personAddRequest = new List<PersonAddRequest> { personAddRequest1, personAddRequest2, personAddRequest3 };
            List<PersonResponse> personResponses = new List<PersonResponse>();
            foreach (PersonAddRequest person in personAddRequest)
            {
                PersonResponse response = _personServices.AddPerson(person);
                personResponses.Add(response);
            }

            List<PersonResponse> PersonsFromGet = _personServices.GetFilteredPersons(nameof(Person.PersonName), "aa");

            foreach (PersonResponse person1 in personResponses)
            {
                if(person1.PersonName !=null)
                if(person1.PersonName.Contains("aa",StringComparison.OrdinalIgnoreCase))
                Assert.Contains(person1, PersonsFromGet);
            }
        }
        #endregion

        #region GetSortedPersons
        [Fact]
        public void GetSortedPersons_SortByNameAsc()
        {
            CountryAddRequest country1 = new CountryAddRequest { CountryName = "India" };
            CountryAddRequest country2 = new CountryAddRequest { CountryName = "Egypt" };

            CountryResponse countryResponse1 = _countryService.AddCountry(country1);
            CountryResponse countryResponse2 = _countryService.AddCountry(country2);

            PersonAddRequest personAddRequest1 = new PersonAddRequest
            {
                PersonName = "John Doe",
                CountryID = countryResponse1.CountryId,
                DateOfBirth = new DateTime(1990, 1, 1),
                Email = "mosi",
                ReceiveNewsLetters = false,
                Address = "Noida",
                Gender = GenderOptions.Male
            };
            PersonAddRequest personAddRequest2 = new PersonAddRequest
            {
                PersonName = "moustafa",
                CountryID = countryResponse2.CountryId,
                DateOfBirth = new DateTime(1997, 11, 1),
                Email = "jfkgjk",
                ReceiveNewsLetters = true,
                Address = "mansoura",
                Gender = GenderOptions.Female
            };
            PersonAddRequest personAddRequest3 = new PersonAddRequest
            {
                PersonName = "Esraa",
                CountryID = countryResponse1.CountryId,
                DateOfBirth = new DateTime(2003, 9, 24),
                Email = "esraaelsayed34@gmail.com",
                ReceiveNewsLetters = false,
                Address = "share3 3ashra",
                Gender = GenderOptions.Engineer
            };
            List<PersonAddRequest> personAddRequest = new List<PersonAddRequest> { personAddRequest1, personAddRequest2, personAddRequest3 };
            List<PersonResponse> personResponses = new List<PersonResponse>();
            foreach (PersonAddRequest person in personAddRequest)
            {
                PersonResponse response = _personServices.AddPerson(person);
                personResponses.Add(response);
            }

            List<PersonResponse> PersonsFromGet = _personServices.GetSortedPersons(personResponses,nameof(Person.PersonName), SortOrder.ASC);

            List<PersonResponse> SortedPersons = _personServices.GetAllPersons().OrderBy(x => x.PersonName).ToList();

            for(int i= 0; i < PersonsFromGet.Count;i++ )
            {
                Assert.Equal(SortedPersons[i],PersonsFromGet[i]);
            }

        
        }
        #endregion

    }
}
