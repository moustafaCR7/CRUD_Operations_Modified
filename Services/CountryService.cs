using Entites;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountryService : ICountryService
    {
        private readonly ApplicationDbContext _context;
        public CountryService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if(countryAddRequest == null)
            throw new ArgumentNullException(nameof(countryAddRequest));

            if(countryAddRequest.CountryName == null)
            throw new ArgumentException(nameof(countryAddRequest.CountryName));

            if (_context.Countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("Given country name already exists");
            }

            Country country = countryAddRequest.ToCountry();
            country.CountryId = Guid.NewGuid();

            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();

            return country.ToCountryResponse();

        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return await _context.Countries.Select(temp => temp.ToCountryResponse()).ToListAsync();
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryID)
        {
            if(countryID==null)
                throw new ArgumentNullException(nameof(countryID));
           return _context?.Countries.FirstOrDefault(x=>x.CountryId == countryID)?.ToCountryResponse();
        }
    }
}
