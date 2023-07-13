using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDExample.Controllers
{
    [Route("[controller]")]
    public class CountriesController : Controller
    {
        private readonly ICountriesGetterService _countriesGetterService;
        private readonly ICountriesUploaderService _countriesUploaderService;

        public CountriesController(ICountriesGetterService countriesService)
        {
            _countriesGetterService = countriesService;
        }


        [Route("UploadFromExcel")]
        public IActionResult UploadFromExcel()
        {
            return View();
        }


        [HttpPost]
        [Route("UploadFromExcel")]
        public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                ViewBag.ErrorMsg = "Please select an xlsx file";
                return View();
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                ViewBag.ErrorMsg = "Unsupported file. 'xlsx' file is expected";
                return View();
            }

            int countriesCountInserted = await _countriesUploaderService.UploadCountriesFromExcelFile(excelFile);

            ViewBag.InsertedCountriesCountMessage = $"{countriesCountInserted} Countries Uploaded";
            return View();
        }
    }
}