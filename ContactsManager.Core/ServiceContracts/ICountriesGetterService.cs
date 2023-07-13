using DTO;
using Microsoft.AspNetCore.Http;

namespace ServiceContracts;

public interface ICountriesGetterService
{
    /// <summary>
    /// Returns all countries from the list
    /// </summary>
    /// <returns>All countries from the list as List of CountryResponse</CountryResponse></returns>
    Task<List<CountryResponse>> GetAllCountries();


    /// <summary>
    /// Returns a country object based on the given country id
    /// </summary>
    /// <param name="countryID">CountryID (guid) to search</param>
    /// <returns>Matching country as CountryResponse object</returns>
    Task<CountryResponse?> GetCountryByCountryID(Guid? countryID);
}