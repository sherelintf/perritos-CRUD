using CRUD.Repositories.Interfaces;
using CRUD.Data;
using Microsoft.EntityFrameworkCore;
using LinqKit;
using CRUD.Models;


namespace CRUD.Repositories.Implementations
{

    public class DogRepository : IDogRepository
    {
        private readonly Context _context;

        public DogRepository(Context context)
        {
            _context = context;

           
        }

        public async Task<DogsSearch> GetDogs(
           string name, string status, string offset, string limit
        )
        {
            var filter = GetFilters(name, status);
            var dogs = await _context.Dogs
                 .Where(filter)
                 .Skip(int.Parse(offset))
                 .Take(int.Parse(limit))

                 .ToListAsync();

            var total = await _context.Dogs.CountAsync(filter);

            return new DogsSearch
            {
                Dogs = dogs,
                Total = total
            };
        }

        public async Task<Dog> GetDog(Guid id)
        {
            try
            {
                var dog = await _context.Dogs.FindAsync(id);
                if (dog == null)
                {
                    throw new Exception("Dog not found");
                }
                return dog;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<Dog> InsertDog(Dog dog)
        {
            _context.Dogs.Add(dog);
            await _context.SaveChangesAsync();
            return dog;
        }

        public async Task<Dog> UpdateDog(Guid id, Dog dog)
        {
            _context.Entry(dog).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return dog;
        }

        public async Task<Dog> DeleteDog(Guid id)
        {
            try
            {
                var dog = await _context.Dogs.FindAsync(id);
                if (dog == null)
                {
                    throw new Exception("Dog not found");
                }
                _context.Dogs.Remove(dog);
                await _context.SaveChangesAsync();
                return dog;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        private static ExpressionStarter<Dog> GetFilters(string name, string status)
        {
            var filter = PredicateBuilder.New<Dog>(true);
            if (!string.IsNullOrEmpty(name))
            {
                var cleanName = name?.Trim().ToLower();
                if (cleanName != null)
                {

                    filter = filter.And(x => x.Name.ToLower().Contains(cleanName));
                }
            }

            if (!string.IsNullOrEmpty(status))
            {
                var cleanStatus = status?.Trim().ToLower();
                if (cleanStatus != null)
                {
                    filter = filter.And(x => x.Status.ToLower().Equals(cleanStatus));
                }

            }
            return filter;
        }
    }


}