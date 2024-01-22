using CRUD.Repositories.Interfaces;
using CRUD.Data;
using Microsoft.EntityFrameworkCore;
using CRUD.Models;

namespace CRUD.Repositories.Implementations
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly Context _context;

        public OwnerRepository(Context context)
        {
            _context = context;
        }

        public async Task<List<Owner>> GetOwners()
        {
            try{
                var owners = await _context.Owners.ToListAsync();
                return owners;
            }
            catch(Exception e){
                throw new Exception(e.Message);
            }
        }

        public async Task<Owner> GetOwner(Guid id)
        {
          try{
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
            {
                throw new Exception("Owner not found");
            }
            return owner;
          }
          catch(Exception e){
            throw new Exception(e.Message);
          }
        }

        public async Task<Owner> InsertOwner(Owner owner)
        {
           try{
            _context.Owners.Add(owner);
            await _context.SaveChangesAsync();
            return owner;
           }
           catch(Exception e){
             throw new Exception(e.Message);
           }
        }

        public async Task<Owner> UpdateOwner(Owner owner)
        {
           try{
            _context.Entry(owner).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return owner;
           }
           catch(Exception e){
             throw new Exception(e.Message);
           }
        }

        public async Task<Owner> DeleteOwner(Guid id)
        {
           try{
            var owner = await _context.Owners.FindAsync(id);
            if (owner == null)
            {
                throw new Exception("Owner not found");
            }
            _context.Owners.Remove(owner);
            await _context.SaveChangesAsync();
            return owner;
           }
           catch(Exception e){
             throw new Exception(e.Message);
           }
        }
    }
}
