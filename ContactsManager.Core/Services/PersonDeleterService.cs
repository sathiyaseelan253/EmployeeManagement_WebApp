using Entities;
using RepositoryContracts;
using ServiceContracts;

namespace Services
{
    public class PersonsDeleterService : IPersonDeleterService
    {
        //private field
        private readonly IPersonsRepository _personRepository;

        //constructor
        public PersonsDeleterService(IPersonsRepository personRepository
            )
        {
            _personRepository = personRepository;
        }

        public async Task<bool> DeletePerson(Guid? personID)
        {
            if (personID == null)
            {
                throw new ArgumentNullException(nameof(personID));
            }

            Person? person = await _personRepository.GetPersonByPersonID(personID.Value);
            if (person == null)
                return false;
            await _personRepository.DeletePersonByPersonID(personID.Value);
            return true;
        }
    }
}
