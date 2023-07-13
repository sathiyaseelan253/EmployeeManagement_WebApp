using ContactsManager.UI.Filters;
using DTO;
using Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using ServiceContracts;

namespace Controllers
{

    public class PersonController : Controller
    {
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonUpdaterService _personUpdaterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly IPersonSorterService _personSorterService;
        private readonly ICountriesGetterService _countriesGetterService;

        public PersonController(IPersonGetterService personService, IPersonAdderService personAdderService, IPersonUpdaterService personUpdaterService,
            IPersonDeleterService personDeleterService, IPersonSorterService personSorterService,
            ICountriesGetterService countriesGetterService)
        {
            _personGetterService = personService;
            _personAdderService= personAdderService;
            _personUpdaterService= personUpdaterService;
            _personDeleterService= personDeleterService;
            _personSorterService= personSorterService;
            _countriesGetterService= countriesGetterService;
        }
        [Route("Person/Index")]
        [TypeFilter(typeof(PersonListActionFilter))]
        public async Task<ActionResult> Index(string? searchBy, string? searchString, string sortByColumn = nameof(PersonResponse.PersonName), SortOrderOptions sortOptions = SortOrderOptions.ASC)
        {
            ViewBag.SearchFields = new Dictionary<string, string>(){
            { nameof(PersonResponse.PersonName), "Person name"},
            { nameof(PersonResponse.Email), "Email"},
            { nameof(PersonResponse.DateOfBirth), "Date of Birth"},
            { nameof(PersonResponse.Gender), "Gender"},
            { nameof(PersonResponse.CountryID), "Country ID"},
            { nameof(PersonResponse.Address), "Address"},
        };
            List<PersonResponse> persons = await _personGetterService.GetFilteredPersons(searchBy, searchString);
            ViewBag.searchBy = searchBy;
            ViewBag.searchString = searchString;
            ViewBag.sortBy = sortByColumn;
            ViewBag.sortOptions = sortOptions.ToString();
            persons = await _personSorterService.GetSortedPersons(persons, sortByColumn, sortOptions);
            return View(persons);
        }
        [HttpGet]
        [Route("Person/Create")]
        public async Task<ActionResult> Create()
        {
            List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
            ViewBag.Countries = countries;
            return View();
        }
        [HttpPost]
        [Route("Person/Create")]
        public async Task<IActionResult> Create(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(errors => errors.Errors).Select(err => err.ErrorMessage).ToList();
                List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
                ViewBag.Countries = countries;
                return View();
            }
            await _personAdderService.AddPerson(personAddRequest);
            return RedirectToAction("Index", "Person");
        }
        [HttpGet]
        [Route("[action]/{personID}")] //Eg: /persons/edit/1
        public async Task<IActionResult> Edit(Guid personID)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            PersonUpdateRequest personUpdateRequest = personResponse.ToPersonUpdateRequest();

            List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
            ViewBag.Countries = countries.Select(temp =>
            new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

            return View(personUpdateRequest);
        }


        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Edit(PersonUpdateRequest personUpdateRequest)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personUpdateRequest.PersonID);

            if (personResponse == null)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                PersonResponse updatedPerson = await _personUpdaterService.UpdatePerson(personUpdateRequest);
                return RedirectToAction("Index");
            }
            else
            {
                List<CountryResponse> countries = await _countriesGetterService.GetAllCountries();
                ViewBag.Countries = countries.Select(temp =>
                new SelectListItem() { Text = temp.CountryName, Value = temp.CountryID.ToString() });

                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return View(personResponse.ToPersonUpdateRequest());
            }
        }
        [HttpGet]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(Guid? personID)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personID);
            if (personResponse == null)
                return RedirectToAction("Index");

            return View(personResponse);
        }

        [HttpPost]
        [Route("[action]/{personID}")]
        public async Task<IActionResult> Delete(PersonUpdateRequest personUpdateResult)
        {
            PersonResponse? personResponse = await _personGetterService.GetPersonByPersonID(personUpdateResult.PersonID);
            if (personResponse == null)
                return RedirectToAction("Index");

            await _personDeleterService.DeletePerson(personUpdateResult.PersonID);
            return RedirectToAction("Index");
        }
        [Route("PDfDownload")]
        public async Task<IActionResult> PDfDownload()
        {
            //List of persons
            List<PersonResponse> persons = await _personGetterService.GetAllPersons();

            return new ViewAsPdf("PDFDownload", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Bottom = 10, Top = 10, Left = 10, Right = 10 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        public async Task<IActionResult> PersonCSVDownload()
        {
            MemoryStream data = await _personGetterService.DownloadPersonCSVFile();
            return File(data, "application/octet-stream", "persons.csv");
        }
        public async Task<IActionResult> PersonExcelDownload()
        {
            MemoryStream memoryStream = await _personGetterService.DownloadPersonExcelFile();
            return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "persons.xlsx");
        }
    }
}