namespace ServiceContracts
{

    public interface IPersonDeleterService
    {
        
        /// <summary>
        /// Deletes a person based on the given person id
        /// </summary>
        /// <param name="PersonID">PersonID to delete</param>
        /// <returns>Returns true, if the deletion is successful; otherwise false</returns>
        Task<bool> DeletePerson(Guid? personID);

    }
}