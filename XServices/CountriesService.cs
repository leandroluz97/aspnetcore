using CRUDoperations.DTO;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using RepositoryContracts;
using ServicesContracts;

namespace XServices
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
        {
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }
            if (string.IsNullOrEmpty(countryAddRequest.CountryName))
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }
            if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
            {
                throw new ArgumentException("Given country name already exists!");
            }
            Country country = new Country()
            {
                CountryId = Guid.NewGuid(),
                CountryName = countryAddRequest.CountryName
            };
            //_db.Countries.Add(country);
            //await _db.SaveChangesAsync();
            await _countriesRepository.AddCountry(country);

            return country.ToCountryResponse();
        }

        public async Task<List<CountryResponse>> GetAllCountries()
        {
            return (await _countriesRepository.GetAllCountries()).Select(country => country.ToCountryResponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
        {
            if (countryId == null)
            {
                throw new ArgumentNullException(nameof(countryId));
            }

            if (countryId == Guid.Empty)
            {
                throw new ArgumentException(nameof(countryId));
            }
            Country? country = await _countriesRepository.GetCountryByCountryId(countryId.Value);

            if (country == null)
            {
                return null;
            }

            return country.ToCountryResponse();
        }

        public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);

            int countryInserted = 0;
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row < rowCount; row++)
                {
                    string? cellValue = Convert.ToString(worksheet.Cells[row, 1].Value);

                    if (!string.IsNullOrWhiteSpace(cellValue))
                    {
                        var countryName = cellValue;
                        if (await _countriesRepository.GetCountryByCountryName(countryName) is null)
                        {
                            var country = new Country()
                            {
                                CountryName = countryName,
                            };

                            await _countriesRepository.AddCountry(country);
                            countryInserted++;
                        }
                    }
                }
            }
            return countryInserted;
        }
    }
}