using DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts;

public interface ICountriesUploaderService
{
    Task<int> UploadCountriesFromExcelFile(IFormFile formFile);
}