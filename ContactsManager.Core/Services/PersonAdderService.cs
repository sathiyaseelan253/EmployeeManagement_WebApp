using DTO;
using Entities;
using RepositoryContracts;
using ServiceContracts;
using Services;

namespace Services
{
    public class PersonsAdderService : IPersonAdderService
    {
        //private field
        private readonly IPersonsRepository _personRepository;

        //constructor
        public PersonsAdderService(IPersonsRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validation
            ValidationHelper.ModelValidation(personAddRequest);

            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonID
            person.PersonID = Guid.NewGuid();

            //add person object to persons list
            await _personRepository.AddPerson(person);

            //convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }

    }
}
