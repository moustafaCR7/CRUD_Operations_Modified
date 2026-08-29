using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUD_Test
{
    public class CountryTest
    {
        private readonly ICountryService _countryService;
        public CountryTest()
        {
            _countryService = new CountryService() ;
        }


        #region AddCountry

        [Fact]
        public void AddCountry_CheckNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(
                //Act
                () => _countryService.AddCountry(countryAddRequest));

        }

        [Fact]
        public void AddCountry_NameIsNull()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest
            {
                CountryName = null
            };


            //Assert
            Assert.Throws<ArgumentException>(
                //Act
                () => _countryService.AddCountry(countryAddRequest));

        }

        [Fact]
        public void AddCountry_NameIsDuplicated()
        {
            //Arrange
            CountryAddRequest? countryAddRequest1 = new CountryAddRequest
            {
                CountryName = "USA"
            };
            CountryAddRequest? countryAddRequest2 = new CountryAddRequest
            {
                CountryName = "USA"
            };

            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countryService.AddCountry(countryAddRequest1);
                _countryService.AddCountry(countryAddRequest2);
            });

        }

        [Fact]
        public void AddCountry_CheckValid()
        {
            //Arrange
            CountryAddRequest? countryAddRequest = new CountryAddRequest
            {
                CountryName = "India"
            };
            //Act
            CountryResponse countryResponse = _countryService.AddCountry(countryAddRequest);
            List<CountryResponse> countryResponses = _countryService.GetAllCountries();
            //Assert
            Assert.True(countryResponse.CountryId != Guid.Empty);
            Assert.Contains(countryResponse, countryResponses);

        }

        #endregion

        #region GetAllCountries

        [Fact]
        public void GetAllCountries_CheckNullByDeafult()
        {
            //act 
            List<CountryResponse> countryResponses = _countryService.GetAllCountries();
            //Assert
            Assert.Empty(countryResponses);
        }

        [Fact]
        public void GetAllCountries_ProperAddedNewItem()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>() {
        new CountryAddRequest() { CountryName = "USA" },
        new CountryAddRequest() { CountryName = "UK" }
      };

            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();

            foreach (CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(_countryService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countryService.GetAllCountries();

            //read each element from countries_list_from_add_country
            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }
        #endregion

        #region GetCountryByCountryID
        [Fact]
        public void GetCountryByCountryID_CheckNull()
        {
            //Arrange
            Guid? countryID = null;
            //Assert
            Assert.Throws<ArgumentNullException>(
                //Act
                () => _countryService.GetCountryByCountryId(countryID));
        #endregion
        }

        [Fact]
        public void GetCountryByCountryID_CheckvalidID()
        {
//Arange
CountryAddRequest country_add_request = new CountryAddRequest()
{
    CountryName = "India"
};
           CountryResponse added = _countryService.AddCountry(country_add_request);
            //Act
            CountryResponse? country_response = _countryService.GetCountryByCountryId(added.CountryId);
            //Assert
            Assert.Equal(added, country_response);

        }
    }

}
