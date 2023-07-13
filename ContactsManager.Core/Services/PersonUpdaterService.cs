using DTO;
using Entities;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace Services
{
    public class PersonsUpdaterService : IPersonUpdaterService
    {
        //private field
        private readonly IPersonsRepository _personRepository;

        //constructor
        public PersonsUpdaterService(IPersonsRepository personsRepository)
        {
            _personRepository = personsRepository;
        }

        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(Person));

            //validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //get matching person object to update
            Person? matchingPerson = await _personRepository.GetPersonByPersonID(personUpdateRequest.PersonID);
            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }

            //update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender.ToString();
            matchingPerson.CountryID = personUpdateRequest.CountryID;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;
            await _personRepository.UpdatePerson(matchingPerson);
            return matchingPerson.ToPersonResponse();
        }
    }
}
