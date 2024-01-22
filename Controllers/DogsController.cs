using Microsoft.AspNetCore.Mvc;
using CRUD.Models;
using CRUD.Repositories.Interfaces;
using Swashbuckle.AspNetCore.Annotations;

namespace CRUD.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class DogsController : ControllerBase
    {
        private readonly IDogRepository _dogRepository;

        public DogsController(IDogRepository dogRepository)
        {
            _dogRepository = dogRepository;
        }

        /// <summary>
        /// Get all dogs with pagination and filter by name and status
        /// </summary>
        /// <param name="name"></param>
        /// <param name="status"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>

        [HttpGet]
        [SwaggerResponse(200, "Get a paginated list of dogs", typeof(DogsSearch))]
        [SwaggerResponse(404, "The dog was not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<ActionResult<DogsSearch>> GetDogs(
            [FromQuery] string name,
            [FromQuery] string status,
            [FromQuery] string offset,
            [FromQuery] string limit
        )
        {
            try
            {
                var dogs = await _dogRepository.GetDogs(name, status, offset, limit);
                if (dogs.Total == 0)
                {
                    return NotFound("No dogs found");
                }
                return Ok(dogs);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Get a dog by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///
        [HttpGet("{id}")]
        [SwaggerResponse(200, "Get a dog by id", typeof(Dog))]
        [SwaggerResponse(404, "The dog was not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<ActionResult<Dog>> GetDog(Guid id)
        {
            try
            {
                var dog = await _dogRepository.GetDog(id);
                return Ok(dog);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Update a dog by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dog"></param>
        /// <returns></returns>
        ///

        [HttpPut("{id}")]
        [SwaggerResponse(200, "Update a dog by id", typeof(Dog))]
        [SwaggerResponse(404, "The dog was not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<IActionResult> UpdateDog(Guid id, Dog dog)
        {
            try
            {
                var dogToUpdate = await _dogRepository.GetDog(id);
                if (dogToUpdate == null)
                {
                    return NotFound("Dog not found");
                }
                dogToUpdate.Name = dog.Name;
                dogToUpdate.Breed = dog.Breed;
                dogToUpdate.Status = dog.Status;
                dogToUpdate.OwnerId = dog.OwnerId;
                await _dogRepository.UpdateDog(id, dogToUpdate);
                return Ok(dog);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Insert a dog
        /// </summary>
        /// <param name="dog"></param>
        /// <returns></returns>
        [HttpPost]
        [SwaggerResponse(200, "Insert a dog", typeof(Dog))]
        [SwaggerResponse(400, "The dog data is invalid")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<ActionResult<Dog>> InsertDog(Dog dog)
        {
            try
            {
                await _dogRepository.InsertDog(dog);
                return CreatedAtAction(nameof(GetDog), new { id = dog.Id }, dog);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Delete a dog by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>  </returns>

        [HttpDelete("{id}")]
        [SwaggerResponse(200, "Delete a dog by id", typeof(Dog))]
        [SwaggerResponse(404, "The dog was not found")]
        [SwaggerResponse(500, "Internal Server Error")]
        public async Task<ActionResult<Dog>> DeleteDog(Guid id)
        {
            try
            {
                var dog = await _dogRepository.DeleteDog(id);
                return Ok(dog);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
