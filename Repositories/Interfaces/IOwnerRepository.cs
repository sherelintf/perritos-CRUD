using CRUD.Models;

namespace CRUD.Repositories.Interfaces
{
    public interface IOwnerRepository
    {
        Task<List<Owner>> GetOwners();
        Task<Owner> GetOwner(Guid id);
        Task<Owner> InsertOwner(Owner owner);
        Task<Owner> UpdateOwner(Owner owner);
        Task<Owner> DeleteOwner(Guid id);
    }
}