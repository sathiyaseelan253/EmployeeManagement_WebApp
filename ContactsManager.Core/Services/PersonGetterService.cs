using CsvHelper;
using System.Globalization;
using OfficeOpenXml;
using System.Drawing;
using RepositoryContracts;
using ServiceContracts;
using DTO;
using Entities;

namespace Services
{
    public class PersonsGetterService : IPersonGetterService
    {
        //private field
        private readonly IPersonsRepository _personRepository;

        //constructor
        public PersonsGetterService(IPersonsRepository personRepository)
        {
            _personRepository = personRepository;
        }


        public async Task<List<PersonResponse>> GetAllPersons()
        {
            var persons = await _personRepository.GetAllPersons();
            return persons
            .Select(temp => temp.ToPersonResponse()).ToList();
            //return _db.sp_GetAllPersons()
            //.Select(temp => temp.ToPersonResponse()).ToList();
        }


        public async Task<PersonResponse?> GetPersonByPersonID(Guid? personID)
        {
            if (personID == null)
                return null;

            Person? person = await _personRepository.GetPersonByPersonID(personID.Value);
            if (person == null)
                return null;

            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = await GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
                return matchingPersons;


            switch (searchBy)
            {
                case nameof(Person.PersonName):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.PersonName) ?
                    temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Email):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Email) ?
                    temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;


                case nameof(Person.DateOfBirth):
                    matchingPersons = allPersons.Where(temp =>
                    (temp.DateOfBirth != null) ?
                    temp.DateOfBirth.Value.ToString("dd MMMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(Person.Gender):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Gender) ?
                    temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.CountryID):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Country) ?
                    temp.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                case nameof(Person.Address):
                    matchingPersons = allPersons.Where(temp =>
                    (!string.IsNullOrEmpty(temp.Address) ?
                    temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;

                default: matchingPersons = allPersons; break;
            }
            return matchingPersons;
        }

        public async Task<MemoryStream> DownloadPersonCSVFile()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter streamWriter = new StreamWriter(memoryStream);

            CsvWriter csvWriter = new CsvWriter(streamWriter,CultureInfo.InvariantCulture,leaveOpen:true);
            csvWriter.WriteHeader<PersonResponse>();
            csvWriter.NextRecord();

            List<PersonResponse> personLists = await GetAllPersons();
            await csvWriter.WriteRecordsAsync(personLists);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<MemoryStream> DownloadPersonExcelFile()
        {
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                int row = 2;
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("PersonsSheet");
                excelWorksheet.Cells["A1"].Value = nameof(PersonResponse.PersonName);
                excelWorksheet.Cells["B1"].Value = nameof(PersonResponse.Email);
                excelWorksheet.Cells["C1"].Value = nameof(PersonResponse.DateOfBirth);
                excelWorksheet.Cells["D1"].Value = nameof(PersonResponse.Age);
                excelWorksheet.Cells["E1"].Value = nameof(PersonResponse.Country);
                excelWorksheet.Cells["F1"].Value = nameof(PersonResponse.Address);
                excelWorksheet.Cells["G1"].Value = nameof(PersonResponse.Gender);
                excelWorksheet.Cells["H1"].Value = nameof(PersonResponse.ReceiveNewsLetters);

                using(ExcelRange headersRange = excelWorksheet.Cells["A1:H1"])
                {
                    headersRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    headersRange.Style.Fill.BackgroundColor.SetColor(Color.LightGray);
                    headersRange.Style.Font.Bold = true;
                }
                List<PersonResponse> personResponses = await GetAllPersons();    
                foreach(var person in personResponses)
                {
                    excelWorksheet.Cells[row, 1].Value = person.PersonName;
                    excelWorksheet.Cells[row, 2].Value = person.Email;
                    if(person.DateOfBirth != null) 
                    excelWorksheet.Cells[row, 3].Value = person.DateOfBirth.Value.ToString("dd-MM-yyyy");
                    excelWorksheet.Cells[row, 4].Value = person.Age;
                    excelWorksheet.Cells[row, 5].Value = person.Country;
                    excelWorksheet.Cells[row, 6].Value = person.Address;
                    excelWorksheet.Cells[row, 7].Value = person.Gender;
                    excelWorksheet.Cells[row, 8].Value = person.ReceiveNewsLetters;
                    row++;
                }
                excelWorksheet.Cells[$"A1:H{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();

            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        
    }
}
